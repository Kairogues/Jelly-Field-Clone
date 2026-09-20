using System.Collections.Generic;
using UnityEngine;



public class Game : MonoBehaviour
{
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private Cell cellPrefab;
    private Grid2D<Cell> cellGrid;
    private Vector2 cellOffset; // Offset to make the [0, 0] cell stay on the left bot



    public void StartNewGame()
    {
        gameLogic.SetupNewGame();

        Grid2D<CellData> cellDataGridPrototype = gameLogic.CellDataGridPrototype.cellDataGrid;

        cellGrid = new Grid2D<Cell>(new(cellDataGridPrototype.SizeX, cellDataGridPrototype.SizeY));

        cellOffset.x = -0.5f * (cellGrid.SizeX - 1);
        cellOffset.y = -0.5f * (cellGrid.SizeY - 1);
        for (int x = 0; x < cellGrid.SizeX; x++)
        {
            for (int y = 0; y < cellGrid.SizeY; y++)
            {
                cellGrid[x][y] = SpawnCell(cellDataGridPrototype[x][y], x + cellOffset.x, y + cellOffset.y);
            }
        }
    }

    public void Process()
    {
        
    }


    private Cell SpawnCell(CellData cellData, float x, float y)
    {
        GameObject cellInstance = PoolManager.Instance.Spawn(
                cellPrefab.gameObject,
                new Vector3(x, 0, y),
                Quaternion.identity
            );

        if (cellInstance.TryGetComponent(out Cell cell))
        {
            cell.CellData = cellData;
        }

        return cell;
    }
}
