using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDragManager : MonoBehaviour
{
    public static MouseDragManager Instance;

    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] private AudioClip dropSFX;
    private CellSpawner holdingCellOrigin;
    private GameObject holdingCell;
    private Cell currentHoveringCell;

    private Vector3 holdingCellStartPosition;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void PickUpCell(CellSpawner cellSpawner)
    {
        holdingCellOrigin = cellSpawner;
        holdingCell = holdingCellOrigin.Cell;
        holdingCellStartPosition = holdingCell.transform.position;
        AudioManager.Instance.PlaySoundFX(pickupSFX, transform, 1.0f);
        //Debug.Log("Picking up...");
    }


    public void DragCell(Vector2 screenPosition)
    {
        if (!holdingCell)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        Plane boardPlane = new Plane(Vector3.up, holdingCellStartPosition);

        if (boardPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);

            holdingCell.transform.position = new Vector3(
                worldPosition.x,
                holdingCellStartPosition.y + 1f,
                worldPosition.z
            );
        }
    }


    public bool IsHoldingValidCell()
    {
        if (holdingCell == null)
        {
            return false;
        }

        return true;
    }


    public void HoverOverCell(Cell cell)
    {
        currentHoveringCell = cell;
        //Debug.Log("Hovering over " + cell.Coord.x + ":" + cell.Coord.y);
    }


    public void StopHoveringCell(Cell cell)
    {
        if (currentHoveringCell == cell)
        {
            currentHoveringCell = null;
        }
    }


    public void DropOverCell()
    {
        if (currentHoveringCell && holdingCellOrigin)
        {
            //Debug.Log("Dropped success");
            currentHoveringCell.DropCellSuccess();
            currentHoveringCell.Setup(holdingCellOrigin.CellData);
            holdingCellOrigin.DropSuccess();
            AudioManager.Instance.PlaySoundFX(dropSFX, transform, 1.0f);
            Destroy(holdingCell);
        } else
        {
            holdingCell.transform.position = holdingCellOrigin.transform.position;
        }

        //Debug.Log("No cell to drop");
        holdingCellOrigin = null;
        holdingCell = null;
        currentHoveringCell = null;
    }
}
