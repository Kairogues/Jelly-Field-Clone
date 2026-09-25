using UnityEngine;

public class MouseDragManager : MonoBehaviour
{
    public static MouseDragManager Instance;

    private CellSpawner holdingCellOrigin;
    private GameObject holdingCell;
    private Cell currentHoveringCell;

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
        Debug.Log("Picking up...");
    }


    public void HoverOverCell(Cell cell)
    {
        currentHoveringCell = cell;
        //Debug.Log("Hovering over " + cell.Coord.x + ":" + cell.Coord.y);
    }


    public void DropOverCell()
    {
        if (currentHoveringCell && holdingCellOrigin)
        {
            Debug.Log("Dropped success");
            currentHoveringCell.DropCellSuccess();
            currentHoveringCell.Setup(holdingCellOrigin.CellData);
        }

        Debug.Log("No cell to drop");
        holdingCellOrigin = null;
        holdingCell = null;
        currentHoveringCell = null;
    }
}
