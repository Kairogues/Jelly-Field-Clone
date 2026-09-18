using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct CellDataColumn
{
    public List<CellData> column;
}

[CreateAssetMenu(fileName = "CellDataGrid", menuName = "CellDataGrid")]
public class CellDataGrid : ScriptableObject
{
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public List<CellDataColumn> cellDataGrid;



    private void OnValidate()
    {
        if (width < 0)
        {
            width = 0;
        }

        if (height < 0) 
        {
            height = 0;
        }

        if (cellDataGrid == null)
        {
            cellDataGrid = new List<CellDataColumn>();
        }

        while (cellDataGrid.Count < width)
        {
            cellDataGrid.Add(new CellDataColumn { column = new List<CellData>() });
        }

        while (cellDataGrid.Count > width)
        {
            cellDataGrid.RemoveAt(cellDataGrid.Count - 1);
        }

        for (int x = 0; x < cellDataGrid.Count; x++)
        {
            CellDataColumn column = cellDataGrid[x];
            
            if (column.column == null)
            {
                column.column = new List<CellData>();
            }

            while (column.column.Count < height)
            {
                column.column.Add(new CellData());
            }
            
            while (column.column.Count > height)
            {
                column.column.RemoveAt(column.column.Count - 1);
            }

            cellDataGrid[x] = column;
        }
    }
}