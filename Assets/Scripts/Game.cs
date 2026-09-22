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
    private Queue<CellUpdateData> cellUpdateQueue = new();
    private bool acceptInputCell = true;
    private float idleDuration = 0f;



    public void StartNewGame()
    {
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
            gameLogic.FillGridAfterMatches();
            // Set idleDuration
            return;
        }

        if (acceptInputCell)
        {
            HandleCellUpdateQueue();
        }

        gameLogic.ScanForMatches();

        if (gameLogic.HasMatches)
        {
            acceptInputCell = false;
            gameLogic.ProcessMatches();
            // Set idleDuration
            return;
        }

        acceptInputCell = true;
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
            cell.CellData = cellData;
            cell.Coord = new((int)x, (int)y);
        }

        return cell;
    }


    public void AddCellUpdate(CellUpdateData cellUpdateData)
    {
        cellUpdateQueue.Enqueue(cellUpdateData);
    }


    public void HandleCellUpdateQueue()
    {
        if (!cellUpdateQueue.TryDequeue(out CellUpdateData cellUpdate))
        {
            return;
        }
        gameLogic.UpdateTileTypeGrid(cellUpdate);
        int2 cellCoord = cellUpdate.cellCoord;
        CellData cellData = cellUpdate.cellData;

        cellGrid[cellCoord].CellData = cellData;
        cellGrid[cellCoord].Setup();
    }
}
