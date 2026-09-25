using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> tileBlocks = new List<GameObject>(4);
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject cell;
    [SerializeField] private BoxCollider collider;
    private Queue<GameObject> availableBlocks = new Queue<GameObject>(4);
    private CellData cellData;


    public CellData CellData
    {
        get => cellData;
        set => cellData = value;
    }
    public GameObject Cell
    {
        get => cell;
    }


    
    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            availableBlocks.Enqueue(tileBlocks[i]);
        }

        CellData cellData = new CellData(TileType.PINK, TileType.ORANGE, TileType.YELLOW, TileType.GREEN);
        SetupCell(cellData);
    }


    public void SetupCellBlock(GameObject availableCellBlock, Vector3 positionOffset, Vector3 scale, TileType tileType)
    {
        MeshRenderer mesh = availableCellBlock.GetComponentInChildren<MeshRenderer>();
        availableCellBlock.transform.localPosition = positionOffset;
        availableCellBlock.transform.localScale = scale;
        if (MaterialLoader.Instance.materialDictionary.TryGetValue(tileType, out Material material))
        {
            mesh.sharedMaterial = material;
        } else
        {
            Debug.LogError("No material for " + tileType +" assigned in the Material Loader!");
        }
        
        availableCellBlock.gameObject.SetActive(true);
    }


    private void SetupCell(CellData cellData)
    {
        this.cellData = cellData;

        if (cellData[new(0, 0)] == TileType.NONE || cellData[new(0, 0)] == TileType.EMPTY)
        {
            Debug.LogError("Spawned a NONE or EMPTY tile!");

            return;
        }


        // HARD CODE INCOMING
        // 1 2 3
        if (cellData[new(0, 0)] == cellData[new(0, 1)] && cellData[new(0, 1)] == cellData[new(1, 0)])
        {
            GameObject availableBlock = availableBlocks.Dequeue();
            SetupCellBlock(
                availableBlock,
                Vector3.zero, 
                new Vector3(1f, 1f, 1f), 
                cellData[new(0, 0)]
            );
        }
        // 1 2 !3
        else if (cellData[new(0, 0)] == cellData[new(0, 1)] && cellData[new(0, 1)] != cellData[new(1, 0)])
        {
            GameObject availableBlock = availableBlocks.Dequeue();
            SetupCellBlock(
                availableBlock,
                new Vector3(-0.25f, 0f, 0f), 
                new Vector3(0.5f, 1f, 1f), 
                cellData[new(0, 0)]
            );

            // 3 4
            if (cellData[new(1, 0)] == cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                SetupCellBlock(
                    availableBlock,
                    new Vector3(0.25f, 0f, 0f),  
                    new Vector3(0.5f, 1f, 1f), 
                    cellData[new(1, 0)]
                );
            }
            // 3 !4
            else if (cellData[new(1, 0)] != cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                SetupCellBlock(
                    availableBlock,
                    new Vector3(0.25f, 0f, -0.25f),  
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 0)]
                );

                availableBlock = availableBlocks.Dequeue();
                SetupCellBlock(
                    availableBlock,
                    new Vector3(0.25f, 0f, 0.25f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 1)]
                );
            }
        }
        // 1 !2 3
        else if (cellData[new(0, 0)] != cellData[new(0, 1)] && cellData[new(0, 0)] == cellData[new(1, 0)])
        {
            GameObject availableBlock = availableBlocks.Dequeue();
            SetupCellBlock(
                availableBlock,
                new Vector3(0f, 0f, -0.25f), 
                new Vector3(1f, 1f, 0.5f), 
                cellData[new(0, 0)]
            );
            // 2 4
            if (cellData[new(0, 1)] == cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                SetupCellBlock(
                    availableBlock,
                    new Vector3(0f, 0f, 0.25f), 
                    new Vector3(1f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );
            }
            // 2 !4
            else if (cellData[new(0, 1)] != cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                SetupCellBlock(
                    availableBlock,
                    new Vector3(-0.25f, 0f, 0.25f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );

                availableBlock = availableBlocks.Dequeue();
                SetupCellBlock(
                    availableBlock,
                    new Vector3(0.25f, 0f, 0.25f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 1)]
                );
            }
        }
        // 1 !2 !3
        else if (cellData[new(0, 0)] != cellData[new(0, 1)] && cellData[new(0, 0)] != cellData[new(1, 0)])
        {
            GameObject availableBlock = availableBlocks.Dequeue();
            SetupCellBlock(
                availableBlock,
                new Vector3(-0.25f, 0f, -0.250f), 
                new Vector3(0.5f, 1f, 0.5f), 
                cellData[new(0, 0)]
            );

            // 2 4
            if (cellData[new(0, 1)] == cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                SetupCellBlock(
                    availableBlock,
                    new Vector3(0f, 0f, 0.25f), 
                    new Vector3(1f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );

                availableBlock = availableBlocks.Dequeue();
                SetupCellBlock(
                    availableBlock,
                    new Vector3(0.25f, 0f, -0.25f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 0)]
                );
            }
            // 2 !4
            else if (cellData[new(0, 1)] != cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                SetupCellBlock(
                    availableBlock,
                    new Vector3(-0.25f, 0f, 0.25f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );

                // 3 4
                if (cellData[new(1, 0)] == cellData[new(1, 1)])
                {
                    availableBlock = availableBlocks.Dequeue();
                    SetupCellBlock(
                        availableBlock,
                        new Vector3(0.25f, 0f, 0f), 
                        new Vector3(0.5f, 1f, 1f), 
                        cellData[new(1, 0)]
                    );
                }
                // 3 !4
                else if (cellData[new(1, 0)] != cellData[new(1, 1)])
                {
                    availableBlock = availableBlocks.Dequeue();
                    SetupCellBlock(
                        availableBlock,
                        new Vector3(0.25f, 0f, -0.25f), 
                        new Vector3(0.5f, 1f, 0.5f), 
                        cellData[new(1, 0)]
                    );

                    availableBlock = availableBlocks.Dequeue();
                    SetupCellBlock(
                        availableBlock,
                        new Vector3(0.25f, 0f, 0.25f), 
                        new Vector3(0.5f, 1f, 0.5f), 
                        cellData[new(1, 1)]
                    );
                }
            }
        }
    }


    private void OnMouseDown()
    {
        MouseDragManager.Instance.PickUpCell(this);
    }


    private void OnMouseUp()
    {
        MouseDragManager.Instance.DropOverCell();
    }
}
