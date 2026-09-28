using Unity.Mathematics;

[System.Serializable]
public struct MatchConnection
{
    public int2 firstGlobalTileCoord;
    public int2 firstCellCoord;
    public int2 secondGlobalTileCoord;
    public int2 secondCellCoord;
    public TileType tileType;
    
    
    
    public override string ToString()
    {
        return $"[(Cell:{firstCellCoord}, Tile: {firstGlobalTileCoord}) : (Cell:{secondCellCoord}, Tile: {secondGlobalTileCoord})]";
    }
}