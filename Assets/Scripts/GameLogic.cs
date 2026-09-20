using UnityEngine;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private CellDataGrid cellDataGridPrototype;
    private Grid2D<TileType> tileTypeGrid;
    private List<Vector2Int> matchedCell;
    

    public int TileGridWidth => tileTypeGrid.SizeX;
    public int TileGridHeight => tileTypeGrid.SizeY;
    public int CellGridWidth => TileGridWidth / CellData.CELL_SIZE;
    public int CellGridHeight => TileGridHeight / CellData.CELL_SIZE;
    public Grid2D<TileType> TileTypeGrid => tileTypeGrid;
    public CellDataGrid CellDataGridPrototype => cellDataGridPrototype;



    public void SetupNewGame()
    {
        if (cellDataGridPrototype == null)
        {
            Debug.LogError("The currentCellDataGrid ScriptableObject is missing!");
            return;
        }

        tileTypeGrid = new Grid2D<TileType>(cellDataGridPrototype.Size * CellData.CELL_SIZE);

        for (int x = 0; x < tileTypeGrid.SizeX; x++)
        {
            for (int y = 0; y < tileTypeGrid.SizeY; y++)
            {
                // Goodluck understanding this :D
                tileTypeGrid[x][y] = cellDataGridPrototype[x / CellData.CELL_SIZE, y / CellData.CELL_SIZE][x % CellData.CELL_SIZE, y % CellData.CELL_SIZE];
            }
        }

        PrintAllElement(false);
    }


    private void ScanForMatches()
    {
        for (int x = 0; x < TileGridWidth; x++)
        {
            for (int y = 0; y < TileGridHeight; y++)
            {
                for (int i = 0; i < 4; i++)
                {
                    
                }
            }
        }
    }


    private bool IsValidMatch()
    {
        return true;
    }


    private void ProcessMatches()
    {
        
    }


    private void FillGridAfterMatches()
    {
        
    }


    public bool HasMatches()
    {
        if (matchedCell.Count == 0)
        {
            return false;
        }

        return true;
    }


    private void PrintAllElement(bool inTileCoord)
    {
        if (inTileCoord)
        {
            for (int x = 0; x < TileGridWidth; x++)
            {
                for (int y = 0; y < TileGridHeight; y++)
                {
                    Debug.Log("[" + x + "," + y + "]: " + TileTypeGrid[x, y]);
                }

            }
        } else
        {
            for (int x = 0; x < TileGridWidth; x += CellData.CELL_SIZE)
            {
                for (int y = 0; y < TileGridHeight; y += CellData.CELL_SIZE)
                {
                    Debug.Log("Cell (" + (x / CellData.CELL_SIZE) + "," + (y / CellData.CELL_SIZE) + "):");
                    Debug.Log("[0,0]: " + TileTypeGrid[x, y]);
                    Debug.Log("[0,1]: " + TileTypeGrid[x, y + 1]);
                    Debug.Log("[1,0]: " + TileTypeGrid[x + 1, y]);
                    Debug.Log("[1,1]: " + TileTypeGrid[x + 1, y + 1]);
                }
            }
        }
    }
}
