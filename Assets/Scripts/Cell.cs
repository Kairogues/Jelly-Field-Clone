using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<Cell> DroppedCell;

    [SerializeField] private CellBlock cellBlockPrefab;

    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private Material floorMaterialNormal;
    [SerializeField] private Material floorMaterialOnCursorHovering;
    [SerializeField] private Material floorMaterialNoneTile;
    private Grid2D<CellBlock> cellBlockPointer = new(new(2,2));
    private CellData cellData;
    private int2 coord;
    private bool isNoneTile = false;
    private bool isEmptyTile = false;


    public CellData CellData
    {
        get => cellData;
        set => cellData = value;
    }
    public int2 Coord
    {
        get => coord;
        set => coord = value;
    }
    public bool IsNoneTile
    {
        get => isNoneTile;
    }
    public bool IsEmptyTile
    {
        get => isEmptyTile;
    }



    private void Awake()
    {
        for (int x = 0; x < CellData.CELL_SIZE; x++)
        {
            for (int y = 0; y < CellData.CELL_SIZE; y++)
            {
                cellBlockPointer[x, y] = null;
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse enters " + Coord);
        if (!isEmptyTile || isNoneTile)
        {
            MouseDragManager.Instance.HoverOverCell(null);
        }
        else
        {
            MouseDragManager.Instance.HoverOverCell(this);
            if (MouseDragManager.Instance.IsHoldingValidCell())
            {
                floorMeshRenderer.sharedMaterial = floorMaterialOnCursorHovering;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse exits " + Coord);
        if (!isNoneTile)
        {
            MouseDragManager.Instance.StopHoveringCell(this);
            floorMeshRenderer.sharedMaterial = floorMaterialNormal;
        }
    }


    public void DropCellSuccess()
    {
        DroppedCell?.Invoke(this);
    }

    


    public void Setup(CellData cellData)
    {
        this.cellData = cellData;
        isEmptyTile = false;

        if (cellData[new(0, 0)] == TileType.NONE)
        {
            isNoneTile = true;
            collider.enabled = false;
            if (MaterialLoader.Instance.materialDictionary.TryGetValue(cellData[new(0, 0)], out Material material))
            {
                floorMeshRenderer.sharedMaterial = material;
            } else
            {
                Debug.LogError("No material for NONE assigned in the Material Loader!");
            }

            return;
        }

        if (cellData[new(0, 0)] == TileType.EMPTY)
        {
            isEmptyTile = true;
            if (MaterialLoader.Instance.materialDictionary.TryGetValue(cellData[new(0, 0)], out Material material))
            {
                floorMeshRenderer.sharedMaterial = material;
            } else
            {
                Debug.LogError("No material for EMPTY assigned in the Material Loader!");
            }

            return;
        }

        // HARD CODE INCOMING
        // 1 2 3
        if (cellData[new(0, 0)] == cellData[new(0, 1)] && cellData[new(0, 1)] == cellData[new(1, 0)])
        {
            CellBlock cellBlockA = Instantiate(cellBlockPrefab, transform);
            cellBlockA.Setup(
                Vector3.zero, 
                Vector3.zero, 
                new Vector3(1f, 1f, 1f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = cellBlockA;
            cellBlockPointer[0, 1] = cellBlockA;
            cellBlockPointer[1, 0] = cellBlockA;
            cellBlockPointer[1, 1] = cellBlockA;
        }
        // 1 2 !3
        else if (cellData[new(0, 0)] == cellData[new(0, 1)] && cellData[new(0, 1)] != cellData[new(1, 0)])
        {
            CellBlock cellBlockA = Instantiate(cellBlockPrefab, transform);
            cellBlockA.Setup(
                new Vector3(-0.25f, 0f, 0f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(0.5f, 1f, 1f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = cellBlockA;
            cellBlockPointer[0, 1] = cellBlockA;

            // 3 4
            if (cellData[new(1, 0)] == cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, transform);
                cellBlockB.Setup(
                    new Vector3(0.25f, 0f, 0f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 1f), 
                    cellData[new(1, 0)]
                );
                cellBlockPointer[1, 0] = cellBlockB;
                cellBlockPointer[1, 1] = cellBlockB;

            }
            // 3 !4
            else if (cellData[new(1, 0)] != cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, transform);
                cellBlockB.Setup(
                    new Vector3(0.25f, 0f, -0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 0)]
                );
                cellBlockPointer[1, 0] = cellBlockB;

                CellBlock cellBlockC = Instantiate(cellBlockPrefab, transform);
                cellBlockC.Setup(
                    new Vector3(0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 1)]
                );
                cellBlockPointer[1, 1] = cellBlockC;

            }
        }
        // 1 !2 3
        else if (cellData[new(0, 0)] != cellData[new(0, 1)] && cellData[new(0, 0)] == cellData[new(1, 0)])
        {
            CellBlock cellBlockA = Instantiate(cellBlockPrefab, transform);
            cellBlockA.Setup(
                new Vector3(0f, 0f, -0.25f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(1f, 1f, 0.5f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = cellBlockA;
            cellBlockPointer[1, 0] = cellBlockA;

            // 2 4
            if (cellData[new(0, 1)] == cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, transform);
                cellBlockB.Setup(
                    new Vector3(0f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = cellBlockB;
                cellBlockPointer[1, 1] = cellBlockB;

            }
            // 2 !4
            else if (cellData[new(0, 1)] != cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, transform);
                cellBlockB.Setup(
                    new Vector3(-0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = cellBlockB;

                CellBlock cellBlockC = Instantiate(cellBlockPrefab, transform);
                cellBlockC.Setup(
                    new Vector3(0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 1)]
                );
                cellBlockPointer[1, 1] = cellBlockC;

            }
        }
        // 1 !2 !3
        else if (cellData[new(0, 0)] != cellData[new(0, 1)] && cellData[new(0, 0)] != cellData[new(1, 0)])
        {
            CellBlock cellBlockA = Instantiate(cellBlockPrefab, transform);
            cellBlockA.Setup(
                new Vector3(-0.25f, 0f, -0.250f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(0.5f, 1f, 0.5f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = cellBlockA;

            // 2 4
            if (cellData[new(0, 1)] == cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, transform);
                cellBlockB.Setup(
                    new Vector3(0f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = cellBlockB;
                cellBlockPointer[1, 1] = cellBlockB;

                CellBlock cellBlockC = Instantiate(cellBlockPrefab, transform);
                cellBlockC.Setup(
                    new Vector3(0.25f, 0f, -0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 0)]
                );
                cellBlockPointer[1, 0] = cellBlockC;

            }
            // 2 !4
            else if (cellData[new(0, 1)] != cellData[new(1, 1)])
            {
                CellBlock cellBlockB = Instantiate(cellBlockPrefab, transform);
                cellBlockB.Setup(
                    new Vector3(-0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = cellBlockB;

                // 3 4
                if (cellData[new(1, 0)] == cellData[new(1, 1)])
                {
                    CellBlock cellBlockC = Instantiate(cellBlockPrefab, transform);
                    cellBlockC.Setup(
                        new Vector3(0.25f, 0f, 0f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(0.5f, 1f, 1f), 
                        cellData[new(1, 0)]
                    );
                    cellBlockPointer[1, 0] = cellBlockC;

                }
                // 3 !4
                else if (cellData[new(1, 0)] != cellData[new(1, 1)])
                {
                    CellBlock cellBlockC = Instantiate(cellBlockPrefab, transform);
                    cellBlockC.Setup(
                        new Vector3(0.25f, 0f, -0.25f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(0.5f, 1f, 0.5f), 
                        cellData[new(1, 0)]
                    );
                    cellBlockPointer[1, 0] = cellBlockC;

                    CellBlock cellBlockD = Instantiate(cellBlockPrefab, transform);
                    cellBlockD.Setup(
                        new Vector3(0.25f, 0f, 0.25f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(0.5f, 1f, 0.5f), 
                        cellData[new(1, 1)]
                    );
                    cellBlockPointer[1, 1] = cellBlockD;

                }
            }
        }
        
    }


    public void ProcessMatch(CellData cellData)
    {
        this.cellData = cellData;
        for (int x = 0; x < CellData.CELL_SIZE; x++)
        {
            for (int y = 0; y < CellData.CELL_SIZE; y++)
            {
                if (cellData[x, y] == TileType.EMPTY && !cellBlockPointer[x, y].IsProcessing)
                {
                    if (cellBlockPointer[x, y] != null)
                    {
                        cellBlockPointer[x, y].ProcessMatch();
                    }
                }
            }
        }
    }


    public void FillEmpty(CellData newCellData)
    {
        List<int2> filledTile = new();
        int emptyCount = 0;
        
        for (int x = 0; x < CellData.CELL_SIZE; x++)
        {
            for (int y = 0; y < CellData.CELL_SIZE; y++)
            {
                if (cellData[x, y] == TileType.EMPTY && newCellData[x, y] != TileType.EMPTY)
                {
                    filledTile.Add(new(x, y));
                } else if (cellData[x, y] != TileType.EMPTY && newCellData[x, y] != TileType.EMPTY)
                {
                    
                } 
                // cellData[x, y] != TileType.EMPTY && newCellData[x, y] == TileType.EMPTY
                // cellData[x, y] == TileType.EMPTY && newCellData[x, y] == TileType.EMPTY
                else if (cellData[x, y] == TileType.EMPTY && newCellData[x, y] == TileType.EMPTY)
                {
                    emptyCount++;
                    //Debug.Log("Unknown Case");
                }
            }
        }

        if (emptyCount == 4)
        {
            isEmptyTile = true;
            return;
        }

        if (filledTile.Count == 0)
        {
            return;
        }
        

        if (filledTile.Count == 1)
        {
            if (newCellData[filledTile[0]] == newCellData[1 - filledTile[0].x, filledTile[0].y])
            {
                FillBlock(GrowType.HORIZONTAL, new(1 - filledTile[0].x, filledTile[0].y));
            } else if (newCellData[filledTile[0]] == newCellData[filledTile[0].x, 1 - filledTile[0].y])
            {
                FillBlock(GrowType.VERTICAL, new(filledTile[0].x, 1 - filledTile[0].y));
            }
            return;
        } else if (filledTile.Count == 2)
        {
            if (filledTile[0].x == filledTile[1].x) // Same col
            {
                if (newCellData[filledTile[0]] == newCellData[filledTile[1]])
                {
                    int2 start = new(1 - filledTile[0].x, filledTile[0].y);
                    if (cellBlockPointer[start] != null)
                    {
                        FillBlock(GrowType.HORIZONTAL, start);
                    } else
                    {
                        FillBlock(GrowType.HORIZONTAL, new(start.x, 1 - start.y));
                    }
                } else
                {
                    FillBlock(GrowType.HORIZONTAL, new(1 - filledTile[0].x, filledTile[0].y));
                    FillBlock(GrowType.HORIZONTAL, new(1 - filledTile[1].x, filledTile[1].y));
                }
            } else // Same row
            {
                if (newCellData[filledTile[0]] == newCellData[filledTile[1]])
                {
                    int2 start = new(filledTile[0].x, 1 - filledTile[0].y);
                    if (cellBlockPointer[start] != null)
                    {
                        FillBlock(GrowType.VERTICAL, start);
                    } else
                    {
                        FillBlock(GrowType.VERTICAL, new(1 - start.x, start.y));
                    }
                } else
                {
                    FillBlock(GrowType.VERTICAL, new(filledTile[0].x, 1 - filledTile[0].y));
                    FillBlock(GrowType.VERTICAL, new(filledTile[1].x, 1 - filledTile[1].y));
                }
            }
            return;
        } else // 3
        {
            for (int x = 0; x < CellData.CELL_SIZE; x++)
            {
                for (int y = 0; y < CellData.CELL_SIZE; y++)
                {
                    if (cellData[x, y] != TileType.EMPTY)
                    {
                        FillBlock(GrowType.DIAGONAL, new(x, y));
                        return;
                    }
                }
            }
        }

        Debug.LogError("Something is wrong!");
    }


    private void FillBlock(GrowType growType, int2 start)
    {
        cellBlockPointer[start].GrowBlock(growType, start);
    }
}