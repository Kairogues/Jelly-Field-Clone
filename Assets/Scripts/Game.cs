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

        Grid2D<CellData> cellDataGridPrototype = gameLogic.CellDataGrid.cellDataGrid;

        cellGrid = new Grid2D<Cell>(new(cellDataGridPrototype.SizeX, cellDataGridPrototype.SizeY));

        cellOffset.x = -0.5f * (cellGrid.SizeX - 1);
        cellOffset.y = -0.5f * (cellGrid.SizeY - 1);
        for (int x = 0; x < cellGrid.SizeX; x++)
        {
            for (int y = 0; y < cellGrid.SizeY; y++)
            {
                cellGrid[x][y] = SpawnCell(cellDataGridPrototype[x][y], x, y);
                cellGrid[x][y].DroppedCell += AddCell;
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
        } else
        {
            //Debug.Log("No input");
        }

        gameLogic.ScanForMatches();

        if (gameLogic.HasMatches)
        {
            acceptInputCell = false;
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

        foreach (int2 cellCoord in gameLogic.CellToFill)
        {
            if (cellCoord.x == 1 && cellCoord.y == 0)
            {
                Debug.Log("This is it");
                Debug.Log("NEW");
                for (int x = 0; x < CellData.CELL_SIZE; x++)
                {
                    for (int y = 0; y < CellData.CELL_SIZE; y++)
                    {
                        int2 globalTileCoord = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(cellCoord, new(x, y));
                        Debug.Log("[" + x + "," + y + "]: " + gameLogic.TileTypeGrid[globalTileCoord]);
                    }
                }

                Debug.Log("OLD");
                for (int x = 0; x < CellData.CELL_SIZE; x++)
                {
                    for (int y = 0; y < CellData.CELL_SIZE; y++)
                    {
                        Debug.Log("[" + x + "," + y + "]: " +  cellGrid[cellCoord].CellData[x, y]);
                    }
                }

                cellGrid[cellCoord].FillEmpty(gameLogic.GetCellData(cellCoord), true);
                continue;
            } else
            {
                cellGrid[cellCoord].FillEmpty(gameLogic.GetCellData(cellCoord), false);
            }
            
            
        }
    }


    private Cell SpawnCell(CellData cellData, float x, float y)
    {
        GameObject cellInstance = Instantiate(
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
