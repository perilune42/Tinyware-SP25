using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Vector2Int Pos;

    public void Init(Vector2Int pos)
    {
        this.Pos = pos;
    }
}
