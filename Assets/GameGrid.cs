using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance;


    public const int gridWidth = 5;
    public const int gridHeight = 5;

    private void Awake()
    {
        Instance = this;
        InitTiles();
    }

    private void InitTiles()
    {
        GridTile[] tiles = GetComponentsInChildren<GridTile>();
        for (int y = 0; y < gridHeight; y++) { 
            for (int x = 0; x < gridWidth; x++)
            {
                tiles[x + y * gridWidth].Init(new Vector2Int(x, y));
            }        
        
        }
    }
}