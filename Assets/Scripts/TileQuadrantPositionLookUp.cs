using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum TileQuadrant
{
	TOP_LEFT,
    TOP_RIGHT,
    BOT_LEFT,
    BOT_RIGHT
}

public class TileQuadrantPositionLookUp
{
    public static Dictionary<TileQuadrant, Vector2Int> TileQuadrantPositionLookUpDictionary = new Dictionary<TileQuadrant, Vector2Int>
    {
        { TileQuadrant.TOP_LEFT, new int2(0, 1) },
        { TileQuadrant.TOP_RIGHT, new int2(1, 1) },
        { TileQuadrant.BOT_LEFT, new int2(0, 0) },
        { TileQuadrant.BOT_RIGHT, new int2(1, 0) }
    };
}
