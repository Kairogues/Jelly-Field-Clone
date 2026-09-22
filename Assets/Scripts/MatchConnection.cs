using Unity.Mathematics;

[System.Serializable]
public struct MatchConnection
{
    public int2 firstTileCoord;
    public int2 firstCellCoord;
    public int2 secondTileCoord;
    public int2 secondCellCoord;
    public TileType tileType;
    
    
    
    public override string ToString()
    {
        return $"[(Cell:{firstCellCoord}, Tile: {firstTileCoord}) : (Cell:{secondCellCoord}, Tile: {secondTileCoord})]";
    }
}