using System.Collections.Generic;
using Unity.Mathematics;

public struct MatchCellGraph
{
    public Dictionary<int2, HashSet<int2>> edges;



    public void AddEdge(int2 firstCellCoord, int2 secondCellCoord)
    {
        if (!edges.TryGetValue(firstCellCoord, out HashSet<int2> list))
        {
            list = new HashSet<int2>();
            edges.Add(firstCellCoord, list);
        }
        edges[firstCellCoord].Add(secondCellCoord);
    }


    public override string ToString()
    {
        string returnString = "";
        foreach (KeyValuePair<int2, HashSet<int2>> edge in edges)
        {
            returnString += "(" + edge.Key.x + "," + edge.Key.y + ") -> ";
            foreach (int2 edgePosition in edge.Value)
            {
                returnString += "(" + edgePosition.x + "," + edgePosition.y + ") ";
            }
            returnString += "\n";
        }

        return returnString;
    }
}
