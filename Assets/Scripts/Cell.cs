using UnityEngine;

public class Cell : MonoBehaviour
{
    private CellData cellData;
    public CellData CellData
    {
        get => cellData;
        set => cellData = value;
    }
}