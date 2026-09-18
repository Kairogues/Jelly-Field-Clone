using UnityEngine;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private CellDataGrid cellDataGridPrototype;
    private List<CellDataColumn> cellDataGrid;
    



    public int Width
    {
        get => cellDataGrid.Count;
    }
    public int Height
    {
        get => cellDataGrid[0].column.Count;
    }
    public List<CellDataColumn> CellDataGrid
    {
        get => cellDataGrid;
    }



    public void SetupNewGame()
    {
        if (cellDataGridPrototype == null)
        {
            Debug.LogError("The currentCellDataGrid ScriptableObject is missing!");
            return;
        }

        cellDataGrid = new List<CellDataColumn>(cellDataGridPrototype.width);

        for (int x = 0; x < cellDataGridPrototype.width; x++)
        {
            CellDataColumn runtimeColumn = new CellDataColumn
            {
                column = new List<CellData>(cellDataGridPrototype.height)
            };

            for (int y = 0; y < cellDataGridPrototype.height; y++)
            {
                CellData runtimeCell = cellDataGridPrototype.cellDataGrid[x].column[y];

                runtimeColumn.column.Add(runtimeCell);
            }

            cellDataGrid.Add(runtimeColumn);
        }

        // PrintAllElement();
    }


    private void ScanForMatches()
    {
        
    }


    private void ProcessMatches()
    {
        
    }


    private void FillGridAfterMatches()
    {
        
    }


    private void PrintAllElement()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Debug.Log(x + "," + y + ": "
                        + CellDataGrid[x].column[y].topLeft + " "      
                        + CellDataGrid[x].column[y].topRight + " "
                        + CellDataGrid[x].column[y].botLeft + " "
                        + CellDataGrid[x].column[y].botRight);
            }

        }
    }
}
