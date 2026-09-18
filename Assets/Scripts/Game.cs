using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private Cell cellPrefab;
    // [SerializeField] private CellPoolManager
    private List<Cell> cellGrid;
    private Vector2 cellOffset;


    public void StartNewGame()
    {
        gameLogic.SetupNewGame();

        int totalCells = gameLogic.Width * gameLogic.Height;
        cellGrid = new List<Cell>(new Cell[totalCells]);


        CellDataGrid cellDataGrid = gameLogic.CellDataGrid;

        cellOffset.x = -0.5f * (gameLogic.Width - 1);
        cellOffset.y = -0.5f * (gameLogic.Height - 1);
        for (int x = 0; x < gameLogic.Width; x++)
        {
            for (int y = 0; y < gameLogic.Height; y++)
            {
                Debug.Log("Pos " + x + ":" + y);
                cellGrid[gameLogic.Height * x + y] = SpawnCell(cellDataGrid.cellDataGrid[y].column[x], x, y);
            }
        }
    }


    public void Process()
    {
        
    }


    private Cell SpawnCell(CellData cellData, int x, int y)
    {
        Cell newCell = Instantiate(cellPrefab, new Vector3(x + cellOffset.x, 0, y + cellOffset.y), Quaternion.identity);
        newCell.CellData = cellData;

        return newCell;
    }
}
