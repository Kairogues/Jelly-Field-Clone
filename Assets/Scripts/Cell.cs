using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Cell : MonoBehaviour
{
    [SerializeField] private List<CellBlock> tileBlocks = new List<CellBlock>(4);
    private Queue<CellBlock> availableBlocks = new Queue<CellBlock>(4);
    private Grid2D<CellBlock> cellBlockPointer;
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



    public void Setup()
    {
        // HARD CODE INCOMING
        // 1 2 3
        if (cellData[new(0, 0)] == cellData[new(0, 1)] && cellData[new(0, 1)] == cellData[new(1, 0)])
        {
            CellBlock availableBlock = availableBlocks.Dequeue();
            availableBlock.Setup(
                Vector3.zero, 
                Vector3.zero, 
                Vector3.one * 2, 
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
                new Vector3(-0.5f, 0f, 0f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(1f, 1f, 2f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = availableBlock;
            cellBlockPointer[0, 1] = availableBlock;

            // 3 4
            if (cellData[new(1, 0)] == cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0.5f, 0f, 0f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 2f), 
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
                    new Vector3(0.5f, 0f, -0.5f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 1f), 
                    cellData[new(1, 0)]
                );
                cellBlockPointer[1, 0] = availableBlock;

                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0.5f, 0f, 0.5f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 1f), 
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
                new Vector3(0f, 0f, -0.5f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(2f, 1f, 1f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = availableBlock;
            cellBlockPointer[1, 0] = availableBlock;

            // 2 4
            if (cellData[new(0, 1)] == cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0f, 0f, 0.5f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(2f, 1f, 1f), 
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
                    new Vector3(-0.5f, 0f, 0.5f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 1f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = availableBlock;

                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0.5f, 0f, 0.5f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 1f), 
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
                new Vector3(-0.5f, 0f, -0.50f), 
                new Vector3(0f, 0f, 0f), 
                new Vector3(1f, 1f, 1f), 
                cellData[new(0, 0)]
            );
            cellBlockPointer[0, 0] = availableBlock;

            // 2 4
            if (cellData[new(0, 1)] == cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0f, 0f, 0.5f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(2f, 1f, 1f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = availableBlock;
                cellBlockPointer[1, 1] = availableBlock;

                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(0.5f, 0f, -0.5f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 1f), 
                    cellData[new(1, 0)]
                );
                cellBlockPointer[1, 0] = availableBlock;
            }
            // 2 !4
            else if (cellData[new(0, 1)] != cellData[new(1, 1)])
            {
                availableBlock = availableBlocks.Dequeue();
                availableBlock.Setup(
                    new Vector3(-0.5f, 0f, 0.5f), 
                    new Vector3(0f, 0f, 0f), 
                    new Vector3(1f, 1f, 1f), 
                    cellData[new(0, 1)]
                );
                cellBlockPointer[0, 1] = availableBlock;

                // 3 4
                if (cellData[new(1, 0)] == cellData[new(1, 1)])
                {
                    availableBlock = availableBlocks.Dequeue();
                    availableBlock.Setup(
                        new Vector3(0.5f, 0f, 0f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(1f, 1f, 2f), 
                        cellData[new(1, 0)]
                    );
                    cellBlockPointer[1, 0] = availableBlock;
                }
                // 3 !4
                else if (cellData[new(1, 0)] == cellData[new(1, 1)])
                {
                    availableBlock = availableBlocks.Dequeue();
                    availableBlock.Setup(
                        new Vector3(0.5f, 0f, -0.5f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(1f, 1f, 1f), 
                        cellData[new(1, 0)]
                    );
                    cellBlockPointer[1, 0] = availableBlock;

                    availableBlock = availableBlocks.Dequeue();
                    availableBlock.Setup(
                        new Vector3(0.5f, 0f, 0.5f), 
                        new Vector3(0f, 0f, 0f), 
                        new Vector3(1f, 1f, 1f), 
                        cellData[new(1, 1)]
                    );
                    cellBlockPointer[1, 1] = availableBlock;
                }
            }
        }
        
    }



    public void ProcessMatch()
    {
        
    }


    public void FillEmpty()
    {
        
    }
}