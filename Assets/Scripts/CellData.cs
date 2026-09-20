using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

// A grid of 2x2 TileType
[System.Serializable]
public class CellData
{
    public static int CELL_SIZE = 2;
    [SerializeField] private Grid2D<TileType> innerTile = new Grid2D<TileType>(new(CELL_SIZE, CELL_SIZE));

    public TileType this[int x, int y]
    {
        get => innerTile[x][y];
        set => innerTile[x][y] = value;
    }

    public TileType this[int2 c]
    {
        get => innerTile[c.x][c.y];
        set => innerTile[c.x][c.y] = value;
    }

    public Grid2D<TileType> InnerTile
    {
        get => innerTile;
        set => innerTile = value;
    }

    public CellData(
            TileType leftBot = TileType.EMPTY,
            TileType rightBot = TileType.EMPTY,
            TileType leftTop = TileType.EMPTY,
            TileType rightTop = TileType.EMPTY)
    {
        innerTile = new Grid2D<TileType>(new(CELL_SIZE, CELL_SIZE));

        innerTile[0][0] = leftBot;
        innerTile[1][0] = rightBot;
        innerTile[0][1] = leftTop;
        innerTile[1][1] = rightTop;
    }


    public void Resize()
    {
        innerTile = new Grid2D<TileType>(new(CELL_SIZE, CELL_SIZE));
    }
}
