using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SingleGoal
{
    public TileType tileType;
    public int goalAmount;
}

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField] public string levelText;
    [SerializeField] public List<SingleGoal> goals;
    [SerializeField] public CellDataGrid levelLayout;
}