using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static E GetNextWraparound<E>(this List<E> list, E item)
    {
        return list[(list.IndexOf(item) + 1) % list.Count];
    }

    public static E[,] Make2DArray<E>(this E[] array, int width, int height) {
        Debug.Assert(array.Length == width * height);
        E[,] newArr = new E[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                newArr[x, y] = array[y * width + x];
            }
        }
        return newArr;
    }

    public static E[] LinearizeArray<E>(this E[,] array)
    {
        int width = array.GetLength(0);
        int height = array.GetLength(1);
        int size = width * height;
        E[] newArr = new E[size];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                newArr[y * width + x] = array[x, y];
            }
        }
        return newArr;
    }

    public static void Iterate2D<E>(this E[,] array, Action<E, int, int> func, bool topDown = false)
    {
        if (topDown)
        {
            for (int y = array.GetLength(1)-1; y >= 0; y--)
            {
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    func?.Invoke(array[x, y], x, y);
                }
            }
        }
        else
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    func?.Invoke(array[x, y], x, y);
                }
            }
        }

    }

}

public static class Util
{
    public static readonly Vector2Int[,] StdPos3x3 = new Vector2Int[3, 3]
    { {new(0,0), new(1,0),new(2,0)},
    {new(0,1), new(1,1),new(2,1)},
    {new(0,2), new(1,2),new(2,2)}};
}