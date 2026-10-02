using Unity.Mathematics;
using UnityEngine;

public enum GrowType
{
    HORIZONTAL,
    VERTICAL,
    DIAGONAL
}


public class CellBlock : MonoBehaviour
{
    public static float growTime = 0.5f;
    public static float popTime = 0.5f;
    [SerializeField] private MeshRenderer visual;
    private bool isProcessing = false;
    private TileType tileType;

    public TileType TileType
    {
        get => tileType;
        set => tileType = value;
    }
    public bool IsProcessing
    {
        get => isProcessing;
        set => isProcessing = value;
    }


    private void Awake()
    {
        isProcessing = false;
    }


    private void Update()
    {
        
    }


    public void Setup(Vector3 positionOffset, Vector3 visualPositionOffset, Vector3 scale, TileType tileType)
    {
        transform.localPosition += positionOffset;
        visual.transform.localPosition += visualPositionOffset;
        transform.localScale = scale;
        this.tileType = tileType;
        isProcessing = false;
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
        isProcessing = true;
        Destroy(gameObject);
    }



    public void GrowBlock(GrowType growType, int2 start)
    {
        switch (growType)
        {
            case GrowType.HORIZONTAL:
                GrowHorizontal(start);
                break;
            case GrowType.VERTICAL:
                GrowVertical(start);
                break;
            case GrowType.DIAGONAL:
                GrowDiagonal(start);
                break;
            default:
                Debug.LogError("GrowType is invalid");
                break;
        }
    }



    public void GrowHorizontal(int2 start)
    {
        int signX = start.x == 0 ? -1 : 1;
        int signY = start.y == 0 ? -1 : 1;
        float amount = transform.localScale.x;

        transform.position = new Vector3(
                transform.position.x + signX * amount * 0.5f,
                transform.position.y,
                transform.position.z
            );

        

        visual.gameObject.transform.localPosition = new Vector3(
                visual.gameObject.transform.localPosition.x - signX * amount,
                visual.gameObject.transform.localPosition.y,
                visual.gameObject.transform.localPosition.z
            );

        transform.localScale = new Vector3(
                transform.localScale.x * 2f,
                transform.localScale.y,
                transform.localScale.z
            );
        
        transform.position = new Vector3(
                transform.position.x - signX * amount,
                transform.position.y,
                transform.position.z
            );

        visual.gameObject.transform.localPosition = new Vector3(
                visual.gameObject.transform.localPosition.x + signX * amount,
                visual.gameObject.transform.localPosition.y,
                visual.gameObject.transform.localPosition.z
            );

    }


    public void GrowVertical(int2 start)
    {
        int signX = start.x == 0 ? -1 : 1;
        int signY = start.y == 0 ? -1 : 1;
        float amount = transform.localScale.z;

        transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z + signY * amount * 0.5f
            );

        visual.gameObject.transform.localPosition = new Vector3(
                visual.gameObject.transform.localPosition.x,
                visual.gameObject.transform.localPosition.y,
                visual.gameObject.transform.localPosition.z - signY * amount
            );

        transform.localScale = new Vector3(
                transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z * 2f
            );

        transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z - signX * amount
            );

        visual.gameObject.transform.localPosition = new Vector3(
                visual.gameObject.transform.localPosition.x,
                visual.gameObject.transform.localPosition.y,
                visual.gameObject.transform.localPosition.z + signX * amount
            );
    }


    public void GrowDiagonal(int2 start)
    {
        int signX = start.x == 0 ? -1 : 1;
        int signY = start.y == 0 ? -1 : 1;
        float amount = transform.localScale.x;

        transform.position = new Vector3(
                transform.position.x + signX * amount * 0.5f,
                transform.position.y,
                transform.position.z + signY * amount * 0.5f
            );

        visual.gameObject.transform.localPosition = new Vector3(
                visual.gameObject.transform.localPosition.x - signX * amount,
                visual.gameObject.transform.localPosition.y,
                visual.gameObject.transform.localPosition.z - signY * amount
            );

        transform.localScale = new Vector3(
                transform.localScale.x * 2f,
                transform.localScale.y,
                transform.localScale.z * 2f
            );

        transform.position = new Vector3(
                transform.position.x - signX * amount,
                transform.position.y,
                transform.position.z - signY * amount
            );

        visual.gameObject.transform.localPosition = new Vector3(
                visual.gameObject.transform.localPosition.x + signX * amount,
                visual.gameObject.transform.localPosition.y,
                visual.gameObject.transform.localPosition.z + signY * amount
            );
    }
}