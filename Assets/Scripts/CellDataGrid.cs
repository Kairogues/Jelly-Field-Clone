using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct ColumnData
{
    public List<CellData> column;
}

[CreateAssetMenu(fileName = "CellDataGrid", menuName = "CellDataGrid")]
public class CellDataGrid : ScriptableObject
{
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public List<ColumnData> cellDataGrid;
}