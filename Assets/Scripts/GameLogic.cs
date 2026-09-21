using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private CellDataGrid cellDataGridPrototype;
    private Grid2D<TileType> tileTypeGrid;
    private Dictionary<TileType, List<MatchConnection>> matchConnections = new();
    private Dictionary<TileType, MatchCellGraph> matchCellGraph = new();
    private Dictionary<TileType, HashSet<int2>> matchGroupByCell = new();
    private Dictionary<TileType, List<int2>> matchGroupByTile = new();
    

    public int TileGridWidth => tileTypeGrid.SizeX;
    public int TileGridHeight => tileTypeGrid.SizeY;
    public int2 TileGridSize => new(TileGridWidth, TileGridHeight);
    public int CellGridWidth => GlobalTileCoordToCellCoord(TileGridWidth).x;
    public int CellGridHeight => GlobalTileCoordToCellCoord(TileGridWidth).y;
    public int2 CellGridSize => new(CellGridWidth, CellGridHeight);
    public Grid2D<TileType> TileTypeGrid => tileTypeGrid;
    public CellDataGrid CellDataGridPrototype => cellDataGridPrototype;



    public void SetupNewGame()
    {
        if (cellDataGridPrototype == null)
        {
            Debug.LogError("The currentCellDataGrid ScriptableObject is missing!");
            return;
        }

        tileTypeGrid = new Grid2D<TileType>(cellDataGridPrototype.Size * CellData.CELL_SIZE);

        for (int x = 0; x < tileTypeGrid.SizeX; x++)
        {
            for (int y = 0; y < tileTypeGrid.SizeY; y++)
            {
                int2 globalTileCoord = new(x, y);
                int2 cellCoord = GlobalTileCoordToCellCoord(globalTileCoord);
                int2 localTileCoord = GlobalTileCoordToLocalTileCoord(globalTileCoord);
                tileTypeGrid[globalTileCoord] = cellDataGridPrototype[cellCoord][localTileCoord];
            }
        }

        //PrintAllElement(true);
    }


    public void ScanForMatches()
    {
        matchConnections.Clear();

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

        //PrintMatchConnectionsDict();
        ConstructMatchCellGraph();
    }


    private void CheckForConnection(int2 firstTileCoord, int2 secondTileCoord)
    {
        if (!tileTypeGrid.AreInRangeCoordinates(firstTileCoord) || !tileTypeGrid.AreInRangeCoordinates(secondTileCoord))
        {
            return;
        }

        if (IsValidMatch(firstTileCoord, secondTileCoord))
        {
            MatchConnection matchConnection = new()
            {
                firstTileCoord = firstTileCoord,
                firstCellCoord = GlobalTileCoordToCellCoord(firstTileCoord),
                secondTileCoord = secondTileCoord,
                secondCellCoord = GlobalTileCoordToCellCoord(secondTileCoord),
                tileType = tileTypeGrid[firstTileCoord]
            };

            if (!matchConnections.TryGetValue(tileTypeGrid[firstTileCoord], out List<MatchConnection> list))
            {
                list = new List<MatchConnection>();
                matchConnections.Add(tileTypeGrid[firstTileCoord], list);
            }

            list.Add(matchConnection);
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
        if (GlobalTileCoordToCellCoord(firstTileCoord).Equals(GlobalTileCoordToCellCoord(secondTileCoord)))
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


    private void ConstructMatchCellGraph()
    {
        matchCellGraph.Clear();
        
        foreach (KeyValuePair<TileType, List<MatchConnection>> item in matchConnections)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                if (!matchCellGraph.TryGetValue(item.Key, out MatchCellGraph graph))
                {
                    graph = new MatchCellGraph()
                    {
                        edges = new()
                    };
                    matchCellGraph.Add(item.Key, graph);
                }

                graph.AddEdge(item.Value[i].firstCellCoord, item.Value[i].secondCellCoord);
                graph.AddEdge(item.Value[i].secondCellCoord, item.Value[i].firstCellCoord);
            }
        }

        //PrintMatchCellGraph();
    }


    private void ProcessMatches()
    {
        
    }


    private void FillGridAfterMatches()
    {
        
    }


    public bool HasMatches()
    {
        if (matchConnections.Count == 0)
        {
            return false;
        }

        return true;
    }


    private int2 GlobalTileCoordToCellCoord(int2 coord)
    {
        return new(coord.x / CellData.CELL_SIZE, coord.y / CellData.CELL_SIZE);
    }


    private int2 GlobalTileCoordToLocalTileCoord(int2 coord)
    {
        return new(coord.x % CellData.CELL_SIZE, coord.y % CellData.CELL_SIZE);
    }


    private int2 CellCoordWithLocalTileCoordToGlobalTileCoord(int2 cellCoord, int2 tileCoord)
    {
        return new(cellCoord.x * CellData.CELL_SIZE + tileCoord.x, cellCoord.y * CellData.CELL_SIZE + tileCoord.y);
    }


    private void PrintAllElement(bool inTileCoord)
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
            for (int x = 0; x < CellGridWidth; x += CellData.CELL_SIZE)
            {
                for (int y = 0; y < CellGridHeight; y += CellData.CELL_SIZE)
                {
                    Debug.Log("Cell (" + (x / CellData.CELL_SIZE) + "," + (y / CellData.CELL_SIZE) + "):");
                    Debug.Log("[0,0]: " + TileTypeGrid[x, y]);
                    Debug.Log("[0,1]: " + TileTypeGrid[x, y + 1]);
                    Debug.Log("[1,0]: " + TileTypeGrid[x + 1, y]);
                    Debug.Log("[1,1]: " + TileTypeGrid[x + 1, y + 1]);
                }
            }
        }
    }


    private void PrintMatchConnectionsDict()
    {
        Debug.Log("MATCH CONNECTIONS");
        foreach (KeyValuePair<TileType, List<MatchConnection>> kvp in matchConnections)
        {
            string items = kvp.Value != null ? string.Join(", ", kvp.Value) : "null";
            Debug.Log($"Color [{kvp.Key}]: [{items}]");
        }
    }


    private void PrintMatchCellGraph()
    {
        Debug.Log("MATCH CELL GRAPH");
        foreach (KeyValuePair<TileType, MatchCellGraph> kvp in matchCellGraph)
        {
            Debug.Log($"Color [{kvp.Key}]:\n{kvp.Value}");
        }
    }
}
