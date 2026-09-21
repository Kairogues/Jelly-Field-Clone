using Unity.Mathematics;
using UnityEngine;

public class Cell : MonoBehaviour, IPoolable
{
    [SerializeField] private CellData cellData;
    private int2 cellCoord;

    public CellData CellData
    {
        get => cellData;
        set => cellData = value;
    }

    public int2 CellCoord => cellCoord;

    [SerializeField] private Tile topLeft;
    [SerializeField] private Tile topRight;
    [SerializeField] private Tile botLeft;
    [SerializeField] private Tile botRight;



    public void OnSpawn()
    {
        
    }


    public void OnDespawn()
    {
        
    }


    public void UpdateCell()
    {
        
    }
}