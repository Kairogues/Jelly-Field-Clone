using System.Collections.Generic;
using Unity.Mathematics;

public struct MatchCellGraph
{
    public Dictionary<int2, List<int2>> edges;



    public void AddEdge(int2 firstCellCoord, int2 secondCellCoord)
    {
        if (!edges.TryGetValue(firstCellCoord, out List<int2> list))
        {
            list = new List<int2>();
            edges.Add(firstCellCoord, list);
        }
        edges[firstCellCoord].Add(secondCellCoord);
    }


    public override string ToString()
    {
        string returnString = "";
        foreach (KeyValuePair<int2, List<int2>> edge in edges)
        {
            returnString += "(" + edge.Key.x + "," + edge.Key.y + ") -> "; 
            for (int i = 0; i < edge.Value.Count; i++)
            {
                returnString += "(" + edge.Value[i].x + "," + edge.Value[i].y + ") ";
            }
            returnString += "\n";
        }

        return returnString;
    }
}
