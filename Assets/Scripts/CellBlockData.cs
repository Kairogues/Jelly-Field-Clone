using UnityEngine;

public struct CellBlockData
{
    Vector3 positionOffset;
    Vector3 visualPositionOffset;
    Vector3 scale;
    TileType tileType;

    public void Setup(
        Vector3 positionOffset,
        Vector3 visualPositionOffset,
        Vector3 scale,
        TileType tileType)
    {
        this.positionOffset = positionOffset;
        this.visualPositionOffset = visualPositionOffset;
        this.scale = scale;
        this.tileType = tileType;
    }
}