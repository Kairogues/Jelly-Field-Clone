using UnityEngine;

public class CellBlock : MonoBehaviour
{
    [SerializeField] private MeshRenderer visual;
    private bool isDisplay = true;
    private TileType tileType;

    public TileType TileType
    {
        get => tileType;
        set => tileType = value;
    }
    public bool IsDisplay
    {
        get => isDisplay;
        set => isDisplay = value;
    }



    public void Setup(Vector3 positionOffset, Vector3 visualPositionOffset, Vector3 scale, TileType tileType)
    {
        transform.position += positionOffset;
        visual.transform.position += visualPositionOffset;
        transform.localScale = scale;
        this.tileType = tileType;
        isDisplay = true;
        if (MaterialLoader.Instance.materialDictionary.TryGetValue(this.tileType, out Material material))
        {
            visual.sharedMaterial = material;
        } else
        {
            Debug.LogError("No material for " + tileType +" assigned in the Material Loader!");
        }
        
        visual.gameObject.SetActive(true);
    }


    public void ProcessMatch()
    {
        // Play animation
        visual.gameObject.SetActive(false);
        isDisplay = false;
    }


    public void FillEmpty()
    {
        
    }
}