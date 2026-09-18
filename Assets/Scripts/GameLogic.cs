using UnityEngine;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private CellDataGrid currentCellDataGrid;
    



    public int Width
    {
        get => currentCellDataGrid.width;
    }
    public int Height
    {
        get => currentCellDataGrid.height;
    }
    public CellDataGrid CurrentCellDataGrid
    {
        get => currentCellDataGrid;
    }



    public void SetupNewGame()
    {
        
    }


    private void ScanForMatches()
    {
        
    }


    private void ProcessMatches()
    {
        
    }


    private void FillGridAfterMatches()
    {
        
    }
}
