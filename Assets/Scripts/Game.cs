using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CellColumn
{
    public List<Cell> column;
}

public class Game : MonoBehaviour
{
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private Cell cellPrefab;
    // [SerializeField] private CellPoolManager
    private List<CellColumn> cellGrid;
    private Vector2 cellOffset;


    public void StartNewGame()
    {
        gameLogic.SetupNewGame();

        cellGrid = new List<CellColumn>(gameLogic.Width);
        for (int i = 0; i < gameLogic.Width; i++)
        {
            CellColumn column = new CellColumn();
            column.column = new List<Cell>(gameLogic.Height);
            cellGrid.Add(column);
        }


        CellDataGrid cellDataGrid = gameLogic.CurrentCellDataGrid;

        cellOffset.x = -0.5f * (gameLogic.Width - 1);
        cellOffset.y = -0.5f * (gameLogic.Height - 1);
        for (int x = 0; x < gameLogic.Width; x++)
        {
            for (int y = 0; y < gameLogic.Height; y++)
            {
                cellGrid[x].column.Add(SpawnCell(cellDataGrid.cellDataGrid[x].column[y], x, y));
            }
        }
    }


    public void Process()
    {
        
    }


    private Cell SpawnCell(CellData cellData, int x, int y)
    {
        GameObject cellInstance = PoolManager.Instance.Spawn(
                cellPrefab.gameObject,
                new Vector3(x + cellOffset.x, 0, y + cellOffset.y),
                Quaternion.identity
            );

        if (cellInstance.TryGetComponent(out Cell cell))
        {
            cell.CellData = cellData;
        }

        return cell;
    }
}
