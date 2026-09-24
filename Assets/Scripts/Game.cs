using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;



public class Game : MonoBehaviour
{
    [SerializeField] private bool isTesting;



    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private Cell cellPrefab;
    private Grid2D<Cell> cellGrid;
    private Vector2 cellOffset; // Offset to make the [0, 0] cell stay on the left bot
    private Queue<Cell> incomingCellQueue = new();
    private bool acceptInputCell = true;
    private float idleDuration = 0f;



    public void StartNewGame()
    {
        idleDuration = 2f;
        gameLogic.SetupNewGame();

        Grid2D<CellData> cellDataGridPrototype = gameLogic.LevelLayout.cellDataGrid;

        cellGrid = new Grid2D<Cell>(new(cellDataGridPrototype.SizeX, cellDataGridPrototype.SizeY));

        cellOffset.x = -0.5f * (cellGrid.SizeX - 1);
        cellOffset.y = -0.5f * (cellGrid.SizeY - 1);
        for (int x = 0; x < cellGrid.SizeX; x++)
        {
            for (int y = 0; y < cellGrid.SizeY; y++)
            {
                cellGrid[x][y] = SpawnCell(cellDataGridPrototype[x][y], x, y);
            }
        }
    }

    public void Process()
    {
        if (isTesting)
        {
            gameLogic.Test();
            return;
        }

        if (idleDuration > 0f)
        {
            idleDuration -= Time.deltaTime;
            if (idleDuration > 0f)
            {
                return;
            }
        }

        if (gameLogic.NeedsFilling)
        {
            FillGridAfterMatches();
            // Set idleDuration
            idleDuration = 0.5f;
            return;
        }

        if (acceptInputCell)
        {
            HandleIncomingCell();
        }

        gameLogic.ScanForMatches();

        if (gameLogic.HasMatches)
        {
            acceptInputCell = false;
            Debug.Log("Hey");
            ProcessMatches();
            // Set idleDuration
            idleDuration = 0.5f;
            return;
        }

        acceptInputCell = true;
    }


    private void ProcessMatches()
    {
        gameLogic.ProcessMatches();

        foreach (int2 cell in gameLogic.CellToFill)
        {
            CellData cellData = gameLogic.GetCellData(cell);
            cellGrid[cell].ProcessMatch(cellData);
        }
    }


    private void FillGridAfterMatches()
    {
        gameLogic.FillGridAfterMatches();

        for (int x = 0; x < cellGrid.SizeX; x++)
        {
            for (int y = 0; y < cellGrid.SizeY; y++)
            {   
                cellGrid[x][y].FillEmpty(gameLogic.GetCellData(new(x, y)));
            }
        }

    }


    private Cell SpawnCell(CellData cellData, float x, float y)
    {
        GameObject cellInstance = PoolManager.Instance.Spawn(
                cellPrefab.gameObject,
                new Vector3(x + cellOffset.x, 0, y + cellOffset.y),
                Quaternion.identity
            );

        if (cellInstance.TryGetComponent(out Cell cell))
        {
            cell.Setup(cellData);
            cell.Coord = new((int)x, (int)y);
        }

        return cell;
    }


    public void AddCellUpdate(CellUpdateData cellUpdateData)
    {
        //incomingCellQueue.Enqueue(cellUpdateData);
    }

    public void AddCell(Cell cell)
    {
        incomingCellQueue.Enqueue(cell);
    }


    public void HandleIncomingCell()
    {
        if (!incomingCellQueue.TryDequeue(out Cell cell))
        {
            return;
        }

        CellUpdateData cellUpdateData = new()
        {
            cellCoord = cell.Coord,
            cellData = cell.CellData
        };

        gameLogic.UpdateTileTypeGrid(cellUpdateData);

        cellGrid[cell.Coord] = cell;
    }
}
