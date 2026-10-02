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
    public static float GROW_DURATION = 0.5f; // Constant
    public static float POP_DURATION = 0.5f; // Constant
    [SerializeField] private MeshRenderer visual;
    private bool isProcessing = false;
    private float growProgress = -1f;
    private Vector3 growStartScale;
    private Vector3 growTargetScale;
    private Vector3 growTargetTransform;
    private Vector3 growTargetVisualTransform;
    private float popProgress = -1f;
    private Vector3 currentScale;
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
        if (popProgress >= 0f)
        {
            popProgress += Time.deltaTime;

            float expandDuration = POP_DURATION * 0.20f; // 20% Expanding
            float pauseDuration  = POP_DURATION * 0.40f; // 40% Holding
            float delayDuration  = POP_DURATION * 0.40f; // 40% Idling

            if (popProgress <= expandDuration)
            {
                float t = popProgress / expandDuration;
                transform.localScale = currentScale * Mathf.Lerp(1f, 1.15f, t);
            }
            else if (popProgress <= expandDuration + pauseDuration)
            {
                transform.localScale = currentScale * 1.15f;
            }
            else if (popProgress <= expandDuration + pauseDuration + delayDuration)
            {
                if (visual.enabled)
                {
                    visual.enabled = false;
                }
            }
            else
            {
                ProcessMatch();
                return;
            }
        }

        if (growProgress >= 0f)
        {
            growProgress += Time.deltaTime;

            float growDuration = GROW_DURATION * 0.50f; // 50% Growing
            float delayDuration  = GROW_DURATION * 0.50f; // 50% Idling

            float t = Mathf.Clamp01(growProgress / growDuration);

            if (growProgress <= growDuration)
            {
                transform.localScale = Vector3.Lerp(growStartScale, growTargetScale, t);
            } else
            {
                // Doing nothing
            }

            if (growProgress >= GROW_DURATION)
            {
                transform.position = growTargetTransform;
                visual.gameObject.transform.localPosition = growTargetVisualTransform;
                transform.localScale = growTargetScale;
                growProgress = -1f;
            }
        }
    }


    public void StartGrowing()
    {
        growProgress = 0;
    }


    public void StartPopping()
    {
        popProgress = 0;
        isProcessing = true;
        currentScale = transform.localScale;
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
        Destroy(gameObject);
    }


    public void GrowBlock(GrowType growType, int2 start)
    {
        growStartScale = transform.localScale;

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

        growTargetScale = transform.localScale;
        transform.localScale = growStartScale;
        StartGrowing();
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
        
        growTargetTransform = new Vector3(
                transform.position.x - signX * amount,
                transform.position.y,
                transform.position.z
            );

        growTargetVisualTransform = new Vector3(
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

        growTargetTransform = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z - signX * amount
            );

        growTargetVisualTransform = new Vector3(
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
        
        growTargetTransform = new Vector3(
                transform.position.x - signX * amount,
                transform.position.y,
                transform.position.z - signY * amount
            );
        
        growTargetVisualTransform = new Vector3(
                visual.gameObject.transform.localPosition.x + signX * amount,
                visual.gameObject.transform.localPosition.y,
                visual.gameObject.transform.localPosition.z + signY * amount
            );
    }
}