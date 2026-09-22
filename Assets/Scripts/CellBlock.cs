using UnityEngine;

public class CellBlock : MonoBehaviour
{
    [SerializeField] private MeshFilter visual;
    private TileType tileType;

    public TileType TileType
    {
        get => tileType;
        set => tileType = value;
    }



    public void Setup(Vector3 positionOffset, Vector3 visualPositionOffset, Vector3 scale, TileType tileType)
    {
        transform.position += positionOffset;
        visual.transform.position += visualPositionOffset;
        transform.localScale = scale;
        this.tileType = tileType;
    }


    public void ProcessMatch()
    {
        
    }


    public void FillEmpty()
    {
        
    }
}