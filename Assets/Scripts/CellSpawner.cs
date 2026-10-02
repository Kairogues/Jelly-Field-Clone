using UnityEngine;
using UnityEngine.EventSystems;

public class CellSpawner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private CellBlock cellBlockPrefab;
    [SerializeField] private GameObject cellBlockHolder;
    private CellData cellData;
    private GameObject cell;

    public CellData CellData
    {
        get => cellData;
        set => cellData = value;
    }
    public GameObject Cell
    {
        get => cell;
        set => cell = value;
    }


    private void Start()
    {
        SpawnNewCellData();
    }


    private void SpawnNewCellData()
    {
        cellData = GameManager.Instance.CellGenerator.GetRandomCellData();
        Setup(cellData);
    }


    public void Setup(CellData cellData)
    {
        this.cellData = cellData;
        cell = Instantiate(cellBlockHolder, transform);
        
        if (cellData[new(0, 0)] == TileType.NONE || cellData[new(0, 0)] == TileType.EMPTY)
        {
            Debug.LogError("The spawned Cell is either EMPTY or NONE cell");

            return;
        }

        // HARD CODE INCOMING
        // 1 2 3
        if (cellData[new(0, 0)] == cellData[new(0, 1)] && cellData[new(0, 1)] == cellData[new(1, 0)])
        {
            CellBlock cellBlockA = Instantiate(cellBlockPrefab, cell.transform);
            cellBlockA.Setup(
                Vector3.zero, 
                Vector3.zero, 
                new Vector3(1f, 1f, 1f), 
                cellData[new(0, 0)]
            );
        }
        // 1 2 !3
        else if (cellData[new(0, 0)] == cellData[new(0, 1)] && cellData[new(0, 1)] != cellData[new(1, 0)])
        {
            CellBlock cellBlockA = Instantiate(cellBlockPrefab, cell.transform);
            cellBlockA.Setup(
                new Vector3(-0.25f, 0f, 0f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(0.5f, 1f, 1f), 
                cellData[new(0, 0)]
            );

            // 3 4
            if (cellData[new(1, 0)] == cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, cell.transform);
                cellBlockB.Setup(
                    new Vector3(0.25f, 0f, 0f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 1f), 
                    cellData[new(1, 0)]
                );

            }
            // 3 !4
            else if (cellData[new(1, 0)] != cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, cell.transform);
                cellBlockB.Setup(
                    new Vector3(0.25f, 0f, -0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 0)]
                );

                CellBlock cellBlockC = Instantiate(cellBlockPrefab, cell.transform);
                cellBlockC.Setup(
                    new Vector3(0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 1)]
                );

            }
        }
        // 1 !2 3
        else if (cellData[new(0, 0)] != cellData[new(0, 1)] && cellData[new(0, 0)] == cellData[new(1, 0)])
        {
            CellBlock cellBlockA = Instantiate(cellBlockPrefab, cell.transform);
            cellBlockA.Setup(
                new Vector3(0f, 0f, -0.25f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(1f, 1f, 0.5f), 
                cellData[new(0, 0)]
            );

            // 2 4
            if (cellData[new(0, 1)] == cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, cell.transform);
                cellBlockB.Setup(
                    new Vector3(0f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );

            }
            // 2 !4
            else if (cellData[new(0, 1)] != cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, cell.transform);
                cellBlockB.Setup(
                    new Vector3(-0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );

                CellBlock cellBlockC = Instantiate(cellBlockPrefab, cell.transform);
                cellBlockC.Setup(
                    new Vector3(0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 1)]
                );

            }
        }
        // 1 !2 !3
        else if (cellData[new(0, 0)] != cellData[new(0, 1)] && cellData[new(0, 0)] != cellData[new(1, 0)])
        {
            CellBlock cellBlockA = Instantiate(cellBlockPrefab, cell.transform);
            cellBlockA.Setup(
                new Vector3(-0.25f, 0f, -0.250f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(0.5f, 1f, 0.5f), 
                cellData[new(0, 0)]
            );

            // 2 4
            if (cellData[new(0, 1)] == cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, cell.transform);
                cellBlockB.Setup(
                    new Vector3(0f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );

                CellBlock cellBlockC = Instantiate(cellBlockPrefab, cell.transform);
                cellBlockC.Setup(
                    new Vector3(0.25f, 0f, -0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 0)]
                );

            }
            // 2 !4
            else if (cellData[new(0, 1)] != cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, cell.transform);
                cellBlockB.Setup(
                    new Vector3(-0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );

                // 3 4
                if (cellData[new(1, 0)] == cellData[new(1, 1)])
                {
                    CellBlock cellBlockC = Instantiate(cellBlockPrefab, cell.transform);
                    cellBlockC.Setup(
                        new Vector3(0.25f, 0f, 0f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(0.5f, 1f, 1f), 
                        cellData[new(1, 0)]
                    );

                }
                // 3 !4
                else if (cellData[new(1, 0)] != cellData[new(1, 1)])
                {
                    CellBlock cellBlockC = Instantiate(cellBlockPrefab, cell.transform);
                    cellBlockC.Setup(
                        new Vector3(0.25f, 0f, -0.25f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(0.5f, 1f, 0.5f), 
                        cellData[new(1, 0)]
                    );

                    CellBlock cellBlockD = Instantiate(cellBlockPrefab, cell.transform);
                    cellBlockD.Setup(
                        new Vector3(0.25f, 0f, 0.25f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(0.5f, 1f, 0.5f), 
                        cellData[new(1, 1)]
                    );

                }
            }
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Mouse Down");
        if (cellData == null)
        {
            Debug.Log("Nothing to pickup");
            return;
        }
        MouseDragManager.Instance.PickUpCell(this);
    }


    public void OnDrag(PointerEventData eventData)
    {
        MouseDragManager.Instance.DragCell(eventData.position);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Mouse Up");
        MouseDragManager.Instance.DropOverCell();
    }


    public void DropSuccess()
    {
        cellData = null;
        SpawnNewCellData();
    }
}
