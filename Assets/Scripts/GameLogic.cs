using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using System;

public class GameLogic : MonoBehaviour
{
    public event Action<TileType> PoppedTile;

    private CellDataGrid cellDataGrid;
    private Grid2D<TileType> tileTypeGrid;
    private Dictionary<TileType, MatchCellGraph> matchCellGraph = new();
    private List<MatchGroup> matchGroups = new();
    private HashSet<int2> cellToFill = new();

    private CellFiller cellFiller = new();

    public int TileGridWidth => tileTypeGrid.SizeX;
    public int TileGridHeight => tileTypeGrid.SizeY;
    public int2 TileGridSize => new(TileGridWidth, TileGridHeight);
    public int CellGridWidth => CoordinateConverter.GlobalTileCoordToCellCoord(TileGridWidth).x;
    public int CellGridHeight => CoordinateConverter.GlobalTileCoordToCellCoord(TileGridHeight).y;
    public int2 CellGridSize => new(CellGridWidth, CellGridHeight);
    public Grid2D<TileType> TileTypeGrid => tileTypeGrid;
    public CellDataGrid CellDataGrid
    {
        get => cellDataGrid;
        set => cellDataGrid = value;
    }
    public List<MatchGroup> MatchGroups => matchGroups;
    public HashSet<int2> CellToFill => cellToFill;
    public bool HasMatches => !(matchCellGraph.Count == 0);
    public bool NeedsFilling { get; private set; }



    #region Setup
    public void SetupNewGame()
    {
        if (cellDataGrid == null)
        {
            Debug.LogError("The currentLevel ScriptableObject is missing!");
            return;
        }

        tileTypeGrid = new Grid2D<TileType>(CellDataGrid.Size * CellData.CELL_SIZE);

        for (int x = 0; x < tileTypeGrid.SizeX; x++)
        {
            for (int y = 0; y < tileTypeGrid.SizeY; y++)
            {
                int2 globalTileCoord = new(x, y);
                int2 cellCoord = CoordinateConverter.GlobalTileCoordToCellCoord(globalTileCoord);
                int2 localTileCoord = CoordinateConverter.GlobalTileCoordToLocalTileCoord(globalTileCoord);
                tileTypeGrid[globalTileCoord] = CellDataGrid[cellCoord][localTileCoord];
            }
        }

        //PrintAllElement(true);
    }
    #endregion


    #region ScanForMatches
    public void ScanForMatches()
    {
        ConstructMatchCellGraph();
        //PrintMatchCellGraph();

        if (!HasMatches)
        {
            return;
        }

        ConstructMatchGroup();
        //PrintMatchGroups();
    }


    private void ConstructMatchCellGraph()
    {
        matchCellGraph.Clear();

        for (int x = 0; x < TileGridWidth; x++)
        {
            for (int y = 0; y < TileGridHeight; y++)
            {
                int2 globalTileCoord = new(x, y);

                int2 globalTileCoordUp = new(globalTileCoord.x, globalTileCoord.y + 1);
                CheckForConnection(globalTileCoord, globalTileCoordUp);

                int2 globalTileCoordRight = new(globalTileCoord.x + 1, globalTileCoord.y);
                CheckForConnection(globalTileCoord, globalTileCoordRight);
            }
        }
    }


    private void CheckForConnection(int2 firstTileCoord, int2 secondTileCoord)
    {
        if (!tileTypeGrid.AreInGridRange(firstTileCoord) || !tileTypeGrid.AreInGridRange(secondTileCoord))
        {
            return;
        }

        if (IsValidMatch(firstTileCoord, secondTileCoord))
        {
            if (!matchCellGraph.TryGetValue(tileTypeGrid[firstTileCoord], out MatchCellGraph graph))
            {
                graph = new MatchCellGraph()
                {
                    edges = new Dictionary<int2, HashSet<int2>>()
                };
                matchCellGraph.Add(tileTypeGrid[firstTileCoord], graph);
            }

            int2 firstCellCoord = CoordinateConverter.GlobalTileCoordToCellCoord(firstTileCoord);
            int2 secondCellCoord = CoordinateConverter.GlobalTileCoordToCellCoord(secondTileCoord);

            graph.AddEdge(firstCellCoord, secondCellCoord);
            graph.AddEdge(secondCellCoord, firstCellCoord);
        }
    }


    private bool IsValidMatch(int2 firstTileCoord, int2 secondTileCoord)
    {
        // Any of them is empty or none tile
        if (tileTypeGrid[firstTileCoord] == TileType.EMPTY || tileTypeGrid[firstTileCoord] == TileType.NONE ||
                tileTypeGrid[secondTileCoord] == TileType.EMPTY || tileTypeGrid[secondTileCoord] == TileType.NONE)
        {
            return false;
        }

        // Belongs to the same cell
        if (CoordinateConverter.GlobalTileCoordToCellCoord(firstTileCoord).Equals(CoordinateConverter.GlobalTileCoordToCellCoord(secondTileCoord)))
        {
            return false;
        }

        // Not adjacent
        int2 difference = firstTileCoord - secondTileCoord;
        if (math.lengthsq(difference) != 1)
        {
            return false;
        }

        // Not same color
        if (tileTypeGrid[firstTileCoord] != tileTypeGrid[secondTileCoord])
        {
            return false;
        }

        return true;
    }


    private void ConstructMatchGroup()
    {
        matchGroups.Clear();

        foreach (KeyValuePair<TileType, MatchCellGraph> graph in matchCellGraph)
        {
            ConstructMatchGroupSingleTileType(graph.Key);
        }
    }


    private void ConstructMatchGroupSingleTileType(TileType tileType)
    {
        Queue<int2> trackingCell = new();
        HashSet<int2> visitedCell = new();

        while (visitedCell.Count < matchCellGraph[tileType].edges.Count)
        {
            // Find one random unvisited cell
            int2 startCell = new();
            foreach (KeyValuePair<int2, HashSet<int2>> item in matchCellGraph[tileType].edges)
            {
                if (!visitedCell.Contains(item.Key))
                {
                    startCell = item.Key;
                    break;
                }
            }

            MatchGroup matchGroup = new()
            {
                matchGroupByCell = new(),
                matchGroupByTile = new(),
                tileType = tileType
            };

            trackingCell.Enqueue(startCell);
            visitedCell.Add(startCell);

            while (trackingCell.Count > 0)
            {
                int2 currentCell = trackingCell.Dequeue();

                matchGroup.matchGroupByCell.Add(currentCell);

                for (int x = 0; x < CellData.CELL_SIZE; x++)
                {
                    for (int y = 0; y < CellData.CELL_SIZE; y++)
                    {
                        int2 globalTileCoord = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(currentCell, new(x, y));
                        if (tileTypeGrid[globalTileCoord] == tileType)
                        {
                            matchGroup.matchGroupByTile.Add(globalTileCoord);
                        }
                    }
                }

                foreach (int2 neighbor in matchCellGraph[tileType].edges[currentCell])
                {
                    if (visitedCell.Contains(neighbor))
                    {
                        continue;
                    }

                    visitedCell.Add(neighbor);
                    trackingCell.Enqueue(neighbor);
                }
            }

            matchGroups.Add(matchGroup);
        }
    }
    #endregion


    #region Process Matches
    public void ProcessMatches()
    {
        cellToFill.Clear();

        for (int i = 0; i < matchGroups.Count; i++)
        {
            foreach (int2 tileCoord in matchGroups[i].matchGroupByTile)
            {
                cellToFill.Add(CoordinateConverter.GlobalTileCoordToCellCoord(tileCoord));
                PoppedTile?.Invoke(tileTypeGrid[tileCoord]);
                tileTypeGrid[tileCoord] = TileType.EMPTY;
            }
        }

        NeedsFilling = true;
    }
    #endregion


    #region Recover After Matches
    public void FillGridAfterMatches()
    {
        foreach (int2 cellCoord in cellToFill)
        {
            cellFiller.FillCell(cellCoord, tileTypeGrid);
        }

        NeedsFilling = false;
    }
    #endregion

    bool hasDropped = false;
    public void UpdateTileTypeGrid(CellUpdateData cellUpdateData)
    {
        int2 cellCoord = cellUpdateData.cellCoord;
        CellData cellData = cellUpdateData.cellData;

        for (int x = 0; x < CellData.CELL_SIZE; x++)
        {
            for (int y = 0; y < CellData.CELL_SIZE; y++)
            {
                int2 localTileCoord = new(x, y);
                int2 globalTileCoord = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(cellCoord, localTileCoord);
                tileTypeGrid[globalTileCoord] = cellData[localTileCoord];
                Debug.Log("Success at cell (" + cellCoord.x + "," + cellCoord.y + "), pos " + x + ":" + y + " " + cellData[localTileCoord]);
            }
        }


        hasDropped = true;
    }


    public CellData GetCellData(int2 cellCoord)
    {
        TileType[] tileType = new TileType[4];
        int index = 0;
        for (int x = 0; x < CellData.CELL_SIZE; x++)
        {
            for (int y = 0; y < CellData.CELL_SIZE; y++)
            {
                int2 tileCoord = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(cellCoord, new(x, y));
                tileType[index] = tileTypeGrid[tileCoord];
                index++;
            }
        }
        return new CellData(tileType[0], tileType[1], tileType[2], tileType[3]);
    }


    #region Debug function
    public void Test()
    {
        int hasShown = 0;
        if (hasShown != 2)
        {
            hasShown += 1;
            Debug.Log("BEFORE");
            Debug.Log(tileTypeGrid[0, 0]);
            Debug.Log(tileTypeGrid[0, 1]);
            Debug.Log(tileTypeGrid[1, 0]);
            Debug.Log(tileTypeGrid[1, 1]);
        }
        cellToFill.Add(new(0,0));
        FillGridAfterMatches();
        if (hasShown != 2)
        {
            hasShown += 1;
            Debug.Log("AFTER");
            Debug.Log(tileTypeGrid[0, 0]);
            Debug.Log(tileTypeGrid[0, 1]);
            Debug.Log(tileTypeGrid[1, 0]);
            Debug.Log(tileTypeGrid[1, 1]);
        }
    }


    public void PrintAllElement(bool inTileCoord)
    {
        if (inTileCoord)
        {
            for (int x = 0; x < TileGridWidth; x++)
            {
                for (int y = 0; y < TileGridHeight; y++)
                {
                    int2 globalTileCoord = new(x, y);
                    Debug.Log("[" + x + "," + y + "]: " + TileTypeGrid[globalTileCoord]);
                }

            }
        } else
        {
            for (int x = 0; x < CellGridWidth; x++)
            {
                for (int y = 0; y < CellGridHeight; y++)
                {
                    Debug.Log("Cell (" + x + "," + y + "):");
                    for (int i = 0; i < CellData.CELL_SIZE; i++)
                    {
                        for (int j = 0; j < CellData.CELL_SIZE; j++)
                        {
                            int2 coord = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(new(x, y), new(i, j));
                            Debug.Log("[" + i + "," + j + "]: " + TileTypeGrid[coord]);
                        }
                    }
                }
            }
        }
    }


    public void PrintMatchCellGraph()
    {
        Debug.Log("MATCH CELL GRAPH");
        foreach (KeyValuePair<TileType, MatchCellGraph> kvp in matchCellGraph)
        {
            Debug.Log($"Color [{kvp.Key}]:\n{kvp.Value}");
        }
    }


    public void PrintMatchGroups()
    {
        Debug.Log("MATCH GROUPS");
        for (int i = 0; i < matchGroups.Count; i++)
        {
            Debug.Log(matchGroups[i]);
        }
    }


    public void PrintCellToFill()
    {
        Debug.Log("CELL TO FILL");
        foreach (int2 cellCoord in cellToFill)
        {
            Debug.Log("(" + cellCoord.x + "," + cellCoord.y + "): ");
            for (int x = 0; x < CellData.CELL_SIZE; x++)
            {
                for (int y = 0; y < CellData.CELL_SIZE; y++)
                {
                    int2 globalTileCoord = CoordinateConverter.CellCoordWithLocalTileCoordToGlobalTileCoord(cellCoord, new(x, y));
                    Debug.Log("[" + x + "," + y + "]: " + TileTypeGrid[globalTileCoord]);
                }
            }
        }
    }
    #endregion
}
