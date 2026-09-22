using Unity.Mathematics;

[System.Serializable]
public class CoordinateConverter
{
    public static int2 GlobalTileCoordToCellCoord(int2 coord)
    {
        return new(coord.x / CellData.CELL_SIZE, coord.y / CellData.CELL_SIZE);
    }


    public static int2 GlobalTileCoordToLocalTileCoord(int2 coord)
    {
        return new(coord.x % CellData.CELL_SIZE, coord.y % CellData.CELL_SIZE);
    }


    public static int2 CellCoordWithLocalTileCoordToGlobalTileCoord(int2 cellCoord, int2 tileCoord)
    {
        return new(cellCoord.x * CellData.CELL_SIZE + tileCoord.x, cellCoord.y * CellData.CELL_SIZE + tileCoord.y);
    }
}