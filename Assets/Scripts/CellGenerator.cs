using UnityEngine;

public class CellGenerator : MonoBehaviour
{
    private int giantChance = 20;
    private int twoBarChance = 30;
    private int oneBarChance = 30;
    private int fourDifferentChance = 20;
    private System.Random randomGen;



    private void Awake()
    {
        SetSeed(System.Environment.TickCount);
    }


    public void SetSeed(int seed)
    {
        randomGen = new System.Random(seed);
    }


    public CellData GetRandomCellData()
    {
        bool isValid = false;
        CellData cellData = new CellData(TileType.RED, TileType.RED, TileType.RED, TileType.RED);
        while (!isValid)
        {
            int cellLayout = PickCellLayout();

            switch (cellLayout)
            {
                case 1:
                    cellData = GetRandomOneBig();
                    break;
                case 2:
                    cellData = GetRandomTwoBar();
                    break;
                case 3:
                    cellData = GetRandomOneBar();
                    break;
                case 4:
                    cellData = GetRandomFourDifferentTile();
                    break;

                default:
                    cellData = GetRandomOneBig();
                    break;
            }

            isValid = IsValidCell(cellData);
        }
        
        return cellData;
    }


    private int PickCellLayout()
    {
        int accumulation = giantChance;
        int randomGenNum = randomGen.Next(0, 100);

        if (randomGenNum < accumulation)
        {
            return 1;
        } else
        {
            accumulation += twoBarChance;
        }

        if (randomGenNum < accumulation)
        {
            return 2;
        } else
        {
            accumulation += oneBarChance;
        }

        if (randomGenNum < accumulation)
        {
            return 3;
        }

        return 4;
    }


    private CellData GetRandomOneBig()
    {
        TileType tileType = GetRandomTileType();
        return new CellData(tileType, tileType, tileType, tileType);
    }


    private CellData GetRandomTwoBar()
    {
        TileType tileTypeOne = GetRandomTileType();
        TileType tileTypeTwo = GetRandomTileType();
        CellData cellData = new CellData();

        int randomGenSide = randomGen.Next(0, 2);
        if (randomGenSide == 0)
        {
            cellData[0, 0] = tileTypeOne;
            cellData[1, 0] = tileTypeOne;
            cellData[0, 1] = tileTypeTwo;
            cellData[1, 1] = tileTypeTwo;
        } else
        {
            cellData[0, 0] = tileTypeOne;
            cellData[1, 0] = tileTypeTwo;
            cellData[0, 1] = tileTypeOne;
            cellData[1, 1] = tileTypeTwo;
        }

        return cellData;
    }


    private CellData GetRandomOneBar()
    {
        CellData cellData = new CellData(GetRandomTileType(), GetRandomTileType(), GetRandomTileType(), GetRandomTileType());

        int randomGenNum = randomGen.Next(0, 4);
        int x = randomGenNum / CellData.CELL_SIZE;
        int y = randomGenNum % CellData.CELL_SIZE;

        int randomGenSide = randomGen.Next(0, 2);
        if (randomGenSide == 0)
        {
            cellData[x, y] = cellData[1 - x, y];
        } else
        {
            cellData[x, y] = cellData[x, 1 - y];
        }

        return cellData;
    }


    private CellData GetRandomFourDifferentTile()
    {
        return new CellData(GetRandomTileType(), GetRandomTileType(), GetRandomTileType(), GetRandomTileType());
    }


    private bool IsValidCell(CellData cellData)
    {
        // Diag
        if (cellData[0, 0] == cellData[1, 1] || cellData[0, 1] == cellData[1, 0])
        {
            return false;
        }

        // 3 same 1 dif
        int sameCount = 0;
        TileType currentTile = cellData[0, 0];
        for (int x = 0; x < CellData.CELL_SIZE; x++)
        {
            for (int y = 0; y < CellData.CELL_SIZE; y++)
            {
                if (cellData[x, y] == currentTile)
                {
                    sameCount++;
                }
            }
        }

        if (sameCount == 3)
        {
            return false;
        }

        sameCount = 0;
        currentTile = cellData[0, 1];
        for (int x = 0; x < CellData.CELL_SIZE; x++)
        {
            for (int y = 0; y < CellData.CELL_SIZE; y++)
            {
                if (cellData[x, y] == currentTile)
                {
                    sameCount++;
                }
            }
        }

        if (sameCount == 3)
        {
            return false;
        }

        return true;
    }


    private TileType GetRandomTileType()
    {
        return (TileType)randomGen.Next(
            (int)TileType.RED,
            (int)TileType.PURPLE + 1
        );
    }
}
