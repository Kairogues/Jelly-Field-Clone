using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] public int level;
    [SerializeField] public int cellSpawnerCount;
    [SerializeField] public Dictionary<TileType, int> goals;
    [SerializeField] public CellDataGrid levelLayout;
}