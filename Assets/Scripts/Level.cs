using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField] public string levelText;
    [SerializeField] public Dictionary<TileType, int> goals;
    [SerializeField] public CellDataGrid levelLayout;
}