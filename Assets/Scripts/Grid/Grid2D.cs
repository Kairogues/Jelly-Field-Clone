using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class GridColumn<T>
{
    public T[] column;

    public T this[int y]
	{
		get => column[y];
		set => column[y] = value;
	}
}


[System.Serializable]
public class Grid2D<T>
{
	[SerializeField] GridColumn<T>[] grid;

	int2 size;

    public GridColumn<T> this[int x] => grid[x];

	public T this[int x, int y]
	{
		get => grid[x][y];
		set => grid[x][y] = value;
	}

	public T this[int2 c]
	{
		get => grid[c.x][c.y];
		set => grid[c.x][c.y] = value;
	}

	public int2 Size => new(SizeX, SizeY);
	public int SizeX => grid.Length;
	public int SizeY => grid[0].column.Length;



	public Grid2D (int2 size)
	{
        Resize(size);
	}


    public void Resize(int2 size)
    {
        this.size = size;
        grid = new GridColumn<T>[size.x];

        for (int x = 0; x < size.x; x++)
        {
            grid[x] = new GridColumn<T>
            {
                column = new T[size.y]
            };
        }
    }


	public bool AreValidCoordinates (int2 c)
    {
		if (0 <= c.x && c.x < size.x && 0 <= c.y && c.y < size.y)
        {
            return true;
        }

        return false;
    }
}