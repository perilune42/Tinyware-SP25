using UnityEngine;

public enum DamageType
{
    Ballistic, Energy, Explosive, None, Friendly
}

public static class Directions
{
    public static readonly Vector2Int None = new Vector2Int(0, 0);
    public static readonly Vector2Int Up = new Vector2Int(0, 1);
    public static readonly Vector2Int Down = new Vector2Int(0, -1);
    public static readonly Vector2Int Left = new Vector2Int(-1, 0);
    public static readonly Vector2Int Right = new Vector2Int(1, 0);
}
