using UnityEngine;

public class CellGenerator : MonoBehaviour
{
    private const int ONE_BIG_CHANCE = 20;
    private const int TWO_BAR_CHANCE = 30;
    private const int ONE_BAR_CHANCE = 30;
    private const int FOR_DIFFERENT_CHANCE = 20;
    private System.Random random;



    public void SetSeed(int seed)
    {
        random = new System.Random(seed);
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
        int accumulation = ONE_BIG_CHANCE;
        int randomNum = random.Next(0, 100);

        if (randomNum < accumulation)
        {
            return 1;
        } else
        {
            accumulation += TWO_BAR_CHANCE;
        }

        if (randomNum < accumulation)
        {
            return 2;
        } else
        {
            accumulation += ONE_BAR_CHANCE;
        }

        if (randomNum < accumulation)
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

        int randomSide = random.Next(0, 2);
        if (randomSide == 0)
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

        int randomNum = random.Next(0, 4);
        int x = randomNum / CellData.CELL_SIZE;
        int y = randomNum % CellData.CELL_SIZE;

        int randomSide = random.Next(0, 2);
        if (randomSide == 0)
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
        return (TileType)random.Next(
            (int)TileType.RED,
            (int)TileType.PURPLE + 1
        );
    }
}
