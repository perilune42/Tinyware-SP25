using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public static Color AlphaBlend(this Color bg, Color fg)
    {
        float alpha = fg.a;
        float invAlpha = 1f - alpha;

        Color result;
        result.r = fg.r * alpha + bg.r * invAlpha;
        result.g = fg.g * alpha + bg.g * invAlpha;
        result.b = fg.b * alpha + bg.b * invAlpha;
        result.a = fg.a + bg.a;
        return result;
    }

    public static TKey GetWeightedRandomKey<TKey, TValue>(
        this Dictionary<TKey, TValue> dict,
        Func<TValue, float> weightSelector)
    {
        if (dict.Count == 0)
            throw new ArgumentException("Dictionary is empty");

        float totalWeight = 0f;
        foreach (var kvp in dict)
        {
            float weight = weightSelector(kvp.Value);
            if (weight < 0f)
                throw new ArgumentException("Weights must be non-negative");
            totalWeight += weight;
        }

        if (totalWeight == 0f)
            throw new ArgumentException("Total weight is zero — all weights are zero?");

        float rand = UnityEngine.Random.value * totalWeight;

        foreach (var kvp in dict)
        {
            float weight = weightSelector(kvp.Value);
            if (rand < weight)
                return kvp.Key;
            rand -= weight;
        }

        // Fallback in case of floating-point rounding issues
        foreach (var kvp in dict)
            if (weightSelector(kvp.Value) > 0)
                return kvp.Key;

        throw new InvalidOperationException("No valid weighted key found");
    }

    public static E RandomElement<E>(this List<E> list)
    {
        if (list.Count == 0) return default;
        return list[Random.Range(0, list.Count)];
    }

}

public static class Util
{
    public static readonly Vector2Int[,] StdPos3x3 = new Vector2Int[3, 3]
    { {new(0,0), new(1,0),new(2,0)},
    {new(0,1), new(1,1),new(2,1)},
    {new(0,2), new(1,2),new(2,2)}};

    public static IEnumerator DelayedCall(float time, Action func)
    {
        yield return new WaitForSeconds(time);
        func?.Invoke();
    }

    // onUpdate param: t (0 to 1)
    public static IEnumerator ContinuousCall(float time, Action<float> onUpdate, Action onEnd)
    {
        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            onUpdate?.Invoke((Time.time - startTime) / time);
            yield return new WaitForEndOfFrame();
        }
        onEnd?.Invoke();
    }
}