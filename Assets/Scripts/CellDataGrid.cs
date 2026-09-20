using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

[System.Serializable]
public struct CellDataColumn
{
    public List<CellData> column;
}

[CreateAssetMenu(fileName = "CellDataGrid", menuName = "CellDataGrid")]
public class CellDataGrid : ScriptableObject
{

    [Range(1, 20)]
    [SerializeField] public int width;
    [Range(1, 20)]
    [SerializeField] public int height;
    [SerializeField] public Grid2D<CellData> cellDataGrid;



    public CellData this[int x, int y]
    {
        get => cellDataGrid[x, y];
        set => cellDataGrid[x, y] = value;
    }
    public int2 Size => new(width, height);


    private void OnValidate()
    {
        int2 targetSize = new int2(width, height);

        // Make sure cellDataGrid exists
        if (cellDataGrid == null)
        {
            cellDataGrid = new Grid2D<CellData>(targetSize);
        }
        // Make sure cellDataGrid has correct width
        if (cellDataGrid.SizeX != width || cellDataGrid.SizeY != height)
        {
            cellDataGrid.Resize(targetSize);
        }
        // Make sure cellDataGrid has correct height
        for (int x = 0; x < width; x++)
        {
            if (cellDataGrid[x].column.Length != height)
            {
                cellDataGrid.Resize(targetSize);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Make sure all CellData in cellDataGrid exists
                if (cellDataGrid[x][y] == null)
                {
                    cellDataGrid[x][y] = new CellData();
                }

                // Make sure all CellData in cellDataGrid has correct size on the X
                if (cellDataGrid[x][y].InnerTile.SizeX != CellData.CELL_SIZE)
                {
                    cellDataGrid[x][y].Resize();
                }

                // Make sure all CellData in cellDataGrid has correct size on the Y
                for (int i = 0; i < CellData.CELL_SIZE; i++)
                {
                    if (cellDataGrid[x][y].InnerTile[i].column.Length != CellData.CELL_SIZE)
                    {
                        cellDataGrid[x][y].Resize();
                    }
                }
            }
        }
    }
}