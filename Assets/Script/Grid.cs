using UnityEngine;
using System.Collections.Generic;

public class Grid<T>
{
    private List<T> elementList;
    private int width;
    private int height;


	public int Width => width;
	public int Height => height;


	public Grid(int width, int height)
	{
		this.width = width;
        this.height = height;
        elementList = new List<T>(width * height);
	}


    public T this[int x, int y]
	{
		get => elementList[y * width + x];
		set => elementList[y * width + x] = value;
	}

    

	public bool AreValidCoordinates(int x, int y)
    {
        if (x < 0 || x >= width)
        {
            return false;
        }

        if (y < 0 || y >= height)
        {
            return false;
        }

        return true;
    }
    

    public void Swap (int firstX, int firstY, int secondX, int secondY)
    {
        (this[firstX, firstY], this[secondX, secondY]) = (this[secondX, secondY], this[firstX, firstY]);
    }
}

