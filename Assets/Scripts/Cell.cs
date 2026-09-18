using UnityEngine;

public class Cell : MonoBehaviour, IPoolable
{
    [SerializeField] private CellData cellData;
    public CellData CellData
    {
        get => cellData;
        set => cellData = value;
    }



    public void OnSpawn()
    {
        
    }


    public void OnDespawn()
    {
        
    }
}