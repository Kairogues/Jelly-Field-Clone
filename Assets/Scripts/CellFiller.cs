using System.Collections.Generic;
using Unity.Mathematics;

public class CellFiller
{
    private int2[] CellTileCoordinates =
    {
        new(0, 0),
        new(1, 0),
        new(0, 1),
        new(1, 1)
    };

    public void FillCell(int2 cellCoord, Grid2D<TileType> tileTypeGrid)
    {
        List<int2> survivors = new();

        foreach (int2 localCoord in CellTileCoordinates)
        {
            int2 globalCoord = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(cellCoord, localCoord);

            if (tileTypeGrid[globalCoord] != TileType.EMPTY)
            {
                survivors.Add(globalCoord);
            }
        }

        switch (survivors.Count)
        {
            case 0:
                return;

            case 1:
                FillSingleSurvivor(survivors, cellCoord, tileTypeGrid);
                return;

            case 2:
                FillTwoSurvivors(survivors, cellCoord, tileTypeGrid);
                return;

            case 3:
                FillThreeSurvivors(survivors, cellCoord, tileTypeGrid);
                return;

            case 4:
                return;
        }
    }

    private void FillSingleSurvivor(List<int2> survivors, int2 cellCoord, Grid2D<TileType> tileTypeGrid)
    {
        TileType tileType = tileTypeGrid[survivors[0]];

        FillEntireCell(tileType, cellCoord, tileTypeGrid);
    }

    private void FillTwoSurvivors(List<int2> survivors, int2 cellCoord, Grid2D<TileType> tileTypeGrid)
    {
        TileType firstTileType = tileTypeGrid[survivors[0]];
        TileType secondTileType = tileTypeGrid[survivors[1]];

        if (firstTileType == secondTileType)
        {
            FillEntireCell(firstTileType, cellCoord, tileTypeGrid);
        }
        else
        {
            FillTwoDifferentColors(survivors, cellCoord, tileTypeGrid);
        }
    }

    private void FillTwoDifferentColors(List<int2> survivors, int2 cellCoord, Grid2D<TileType> tileTypeGrid)
    {
        int2 firstSurvivor = survivors[0];
        int2 secondSurvivor = survivors[1];

        if (firstSurvivor.y == secondSurvivor.y)
        {
            GrowVertical(firstSurvivor, cellCoord, tileTypeGrid);

            GrowVertical(secondSurvivor, cellCoord, tileTypeGrid);
        }
        else
        {
            GrowHorizontal(firstSurvivor, cellCoord, tileTypeGrid);

            GrowHorizontal(secondSurvivor, cellCoord, tileTypeGrid);
        }
    }

    private void FillThreeSurvivors(List<int2> survivors, int2 cellCoord, Grid2D<TileType> tileTypeGrid)
    {
        int2 emptyTile = new(-1, -1);
        foreach (int2 localCoord in CellTileCoordinates)
        {
            int2 globalCoord = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(cellCoord, localCoord);

            if (tileTypeGrid[globalCoord] == TileType.EMPTY)
            {
                emptyTile = globalCoord;
            }
        }

        if (emptyTile.x == -1)
        {
            return;
        }

        Dictionary<TileType, List<int2>> uniqueTileType = new();

        foreach (int2 survivor in survivors)
        {
            TileType tileType = tileTypeGrid[survivor];

            if (!uniqueTileType.TryGetValue(tileType, out List<int2> tiles))
            {
                tiles = new List<int2>();
                uniqueTileType.Add(tileType, tiles);
            }

            tiles.Add(survivor);
        }

        // Two cells have the same color, one cell has a different color.
        if (uniqueTileType.Count == 2)
        {
            foreach (KeyValuePair<TileType, List<int2>> kvp in uniqueTileType)
            {
                if (kvp.Value.Count == 1)
                {
                    tileTypeGrid[emptyTile] = kvp.Key;
                    return;
                }
            }
        }

        // Three different colors, prioritize horizontal growth.
        foreach (int2 survivor in survivors)
        {
            if (survivor.y == emptyTile.y)
            {
                tileTypeGrid[emptyTile] = tileTypeGrid[survivor];
                return;
            }
        }
    }

    private void FillEntireCell(TileType tileType, int2 cellCoord, Grid2D<TileType> tileTypeGrid)
    {
        foreach (int2 localCoord in CellTileCoordinates)
        {
            int2 globalCoord = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(cellCoord, localCoord);

            tileTypeGrid[globalCoord] = tileType;
        }
    }

    private void GrowVertical(int2 tileCoord, int2 cellCoord, Grid2D<TileType> tileTypeGrid)
    {
        TileType tileType = tileTypeGrid[tileCoord];

        int2 localCoord = CoordinateConverter.GlobalTileCoordToLocalTileCoord(tileCoord);

        int2 targetLocalCoord = new(localCoord.x, 1 - localCoord.y);

        int2 target = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(cellCoord, targetLocalCoord);

        tileTypeGrid[target] = tileType;
    }

    private void GrowHorizontal(int2 tileCoord, int2 cellCoord, Grid2D<TileType> tileTypeGrid)
    {
        TileType tileType = tileTypeGrid[tileCoord];

        int2 localCoord = CoordinateConverter.GlobalTileCoordToLocalTileCoord(tileCoord);

        int2 targetLocalCoord = new(1 - localCoord.x, localCoord.y);

        int2 target = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(cellCoord, targetLocalCoord);

        tileTypeGrid[target] = tileType;
    }
}