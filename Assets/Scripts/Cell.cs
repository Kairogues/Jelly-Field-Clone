using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private List<CellBlock> tileBlocks = new List<CellBlock>(4);
    private Queue<CellBlock> availableBlocks = new Queue<CellBlock>(4);
    private Grid2D<CellBlock> cellBlockPointer = new(new(2,2));
    private CellData cellData;
    private int2 coord;


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


    public void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            availableBlocks.Enqueue(tileBlocks[i]);
        }


    }



    public void Setup(CellData cellData)
    {
        this.cellData = cellData;
        // HARD CODE INCOMING
        // 1 2 3
        if (cellData[new(0, 0)] == cellData[new(0, 1)] && cellData[new(0, 1)] == cellData[new(1, 0)])
        {
            CellBlock availableBlock = availableBlocks.Dequeue();
            availableBlock.Setup(
                Vector3.zero, 
                Vector3.zero, 
                new Vector3(1f, 1f, 1f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = availableBlock;
            cellBlockPointer[0, 1] = availableBlock;
            cellBlockPointer[1, 0] = availableBlock;
            cellBlockPointer[1, 1] = availableBlock;
        }
        // 1 2 !3
        else if (cellData[new(0, 0)] == cellData[new(0, 1)] && cellData[new(0, 1)] != cellData[new(1, 0)])
        {
            CellBlock availableBlock = availableBlocks.Dequeue();
            availableBlock.Setup(
                new Vector3(-0.25f, 0f, 0f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(0.5f, 1f, 1f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = availableBlock;
            cellBlockPointer[0, 1] = availableBlock;

            // 3 4
            if (cellData[new(1, 0)] == cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0.25f, 0f, 0f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 1f), 
                    cellData[new(1, 0)]
                );
                cellBlockPointer[1, 0] = availableBlock;
                cellBlockPointer[1, 1] = availableBlock;
            }
            // 3 !4
            else if (cellData[new(1, 0)] != cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0.25f, 0f, -0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 0)]
                );
                cellBlockPointer[1, 0] = availableBlock;

                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 1)]
                );
                cellBlockPointer[1, 1] = availableBlock;
            }
        }
        // 1 !2 3
        else if (cellData[new(0, 0)] != cellData[new(0, 1)] && cellData[new(0, 0)] == cellData[new(1, 0)])
        {
            CellBlock availableBlock = availableBlocks.Dequeue();
            availableBlock.Setup(
                new Vector3(0f, 0f, -0.25f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(1f, 1f, 0.5f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = availableBlock;
            cellBlockPointer[1, 0] = availableBlock;

            // 2 4
            if (cellData[new(0, 1)] == cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = availableBlock;
                cellBlockPointer[1, 1] = availableBlock;
            }
            // 2 !4
            else if (cellData[new(0, 1)] != cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(-0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = availableBlock;

                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 1)]
                );
                cellBlockPointer[1, 1] = availableBlock;
            }
        }
        // 1 !2 !3
        else if (cellData[new(0, 0)] != cellData[new(0, 1)] && cellData[new(0, 0)] != cellData[new(1, 0)])
        {
            CellBlock availableBlock = availableBlocks.Dequeue();
            availableBlock.Setup(
                new Vector3(-0.25f, 0f, -0.250f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(0.5f, 1f, 0.5f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = availableBlock;

            // 2 4
            if (cellData[new(0, 1)] == cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = availableBlock;
                cellBlockPointer[1, 1] = availableBlock;

                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0.25f, 0f, -0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(1, 0)]
                );
                cellBlockPointer[1, 0] = availableBlock;
            }
            // 2 !4
            else if (cellData[new(0, 1)] != cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(-0.25f, 0f, 0.25f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(0.5f, 1f, 0.5f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = availableBlock;

                // 3 4
                if (cellData[new(1, 0)] == cellData[new(1, 1)])
                {
                    availableBlock = availableBlocks.Dequeue();
                    availableBlock.Setup(
                        new Vector3(0.25f, 0f, 0f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(0.5f, 1f, 1f), 
                        cellData[new(1, 0)]
                    );
                    cellBlockPointer[1, 0] = availableBlock;
                }
                // 3 !4
                else if (cellData[new(1, 0)] != cellData[new(1, 1)])
                {
                    availableBlock = availableBlocks.Dequeue();
                    availableBlock.Setup(
                        new Vector3(0.25f, 0f, -0.25f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(0.5f, 1f, 0.5f), 
                        cellData[new(1, 0)]
                    );
                    cellBlockPointer[1, 0] = availableBlock;

                    availableBlock = availableBlocks.Dequeue();
                    availableBlock.Setup(
                        new Vector3(0.25f, 0f, 0.25f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(0.5f, 1f, 0.5f), 
                        cellData[new(1, 1)]
                    );
                    cellBlockPointer[1, 1] = availableBlock;
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
                if (cellData[x, y] == TileType.EMPTY && cellBlockPointer[x, y].IsDisplay)
                {
                    cellBlockPointer[x, y].ProcessMatch();
                    availableBlocks.Enqueue(cellBlockPointer[x, y]);
                } else if (!cellBlockPointer[x, y].isActiveAndEnabled)
                {
                    cellBlockPointer[x, y] = null;
                }
            }
        }
    }


    public void FillEmpty(CellData newCellData)
    {
        List<int2> fillTile = new();
        
        for (int x = 0; x < CellData.CELL_SIZE; x++)
        {
            for (int y = 0; y < CellData.CELL_SIZE; y++)
            {
                if (cellData[x, y] == TileType.EMPTY && newCellData[x, y] != TileType.EMPTY)
                {
                    fillTile.Add(new(x, y));
                }
            }
        }

        if (fillTile.Count == 0)
        {
            return;
        }
        

        if (fillTile.Count == 1)
        {
            if (newCellData[fillTile[0]] == newCellData[1 - fillTile[0].x, fillTile[0].y])
            {
                FillHorizontal(new(1 - fillTile[0].x, fillTile[0].y));
            } else if (newCellData[fillTile[0]] == newCellData[fillTile[0].x, 1 - fillTile[0].y])
            {
                FillVertical(new(fillTile[0].x, 1 - fillTile[0].y));
            }
            return;
        } else if (fillTile.Count == 2)
        {
            if (fillTile[0].x == fillTile[1].x) // Same col
            {
                if (cellData[fillTile[0]] == cellData[fillTile[1]])
                {
                    FillHorizontal(new(1 - fillTile[0].x, fillTile[0].y));
                } else
                {
                    FillHorizontal(new(1 - fillTile[0].x, fillTile[0].y));
                    FillHorizontal(new(1 - fillTile[1].x, fillTile[1].y));
                }
            } else // Same row
            {
                if (cellData[fillTile[0]] == cellData[fillTile[1]])
                {
                    FillVertical(new(fillTile[0].x, 1 - fillTile[0].y));
                } else
                {
                    FillVertical(new(fillTile[0].x, 1 - fillTile[0].y));
                    FillVertical(new(fillTile[1].x, 1 - fillTile[1].y));
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
                        FillDiagonal(new(x, y));
                        return;
                    }
                }
            }
        }

        Debug.LogError("Something is wrong!");
    }


    private void FillDiagonal(int2 start)
    {
        cellBlockPointer[start].GrowDiagonal(start);
    }


    private void FillHorizontal(int2 start)
    {
        cellBlockPointer[start].GrowHorizontal(start);
    }


    private void FillVertical(int2 start)
    {
        cellBlockPointer[start].GrowVertical(start);
    }
}