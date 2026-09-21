using System.Collections.Generic;
using Unity.Mathematics;

public struct MatchGroup
{
    public HashSet<int2> matchGroupByCell;
    public HashSet<int2> matchGroupByTile;
    public TileType tileType;



    public override string ToString()
    {
        string returnString = "Color " + tileType + ":\n";
        string byCellString = "By cell: ";
        string byTileString = "By tile: ";

        foreach (int2 cell in matchGroupByCell)
        {
            byCellString += "(" + cell.x + "," + cell.y + ") ";
        }
        byCellString += "\n";

        foreach (int2 tile in matchGroupByTile)
        {
            byTileString += "(" + tile.x + "," + tile.y + ") ";
        }

        returnString += byCellString + byTileString;

        return returnString;
    }
}
