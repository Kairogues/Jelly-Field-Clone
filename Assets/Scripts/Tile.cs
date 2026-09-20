using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    private TileQuadrant tileQuadrant;
    private Cell owner;
    private TileType tileType;
    public TileQuadrant TileQuadrant
    {
        get => tileQuadrant;
        set => tileQuadrant = value;
    }
    public Cell Owner
    {
        get => owner;
        set => owner = value;
    }
    public TileType TileType
    {
        get => tileType;
        set => tileType = value;
    }



    public void UpdateTile()
    {
        
    }
}
