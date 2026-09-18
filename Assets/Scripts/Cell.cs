using UnityEngine;

public class Cell : MonoBehaviour, IPoolable
{
    [SerializeField] private CellData cellData;
    public CellData CellData
    {
        get => cellData;
        set => cellData = value;
    }

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