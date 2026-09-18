using UnityEngine;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private CellDataGrid cellDataGrid;



    public int Width
    {
        get => cellDataGrid.width;
    }
    public int Height
    {
        get => cellDataGrid.height;
    }
    public CellDataGrid CellDataGrid
    {
        get => cellDataGrid;
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
