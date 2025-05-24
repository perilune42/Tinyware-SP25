using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance;


    public const int gridWidth = 5;
    public const int gridHeight = 5;
    GridTile[,] tiles = new GridTile[gridWidth,gridHeight];
    Unit[,] units = new Unit[gridWidth,gridHeight];

    private void Awake()
    {
        Instance = this;
        InitTiles();
    }

    private void InitTiles()
    {
        GridTile[] foundTiles = GetComponentsInChildren<GridTile>();
        for (int y = 0; y < gridHeight; y++) { 
            for (int x = 0; x < gridWidth; x++)
            {
                GridTile tile = foundTiles[x + y * gridWidth];
                Vector2Int pos = new Vector2Int(x, y);
                tile.Init(pos);
                tiles[x,y] = tile;

                // register pre-placed units to board
                Unit unit = tile.GetComponentInChildren<Unit>();
                if (unit != null) {
                    SetUnit(unit, pos); 
                }
                
            }

        }
    }

    // does not do any checks
    public void SetUnit(Unit unit, Vector2Int pos)
    {
        units[pos.x, pos.y] = unit;
        if (unit != null)
        {
            unit.SetPos(pos);
            GridTile tile = GetTile(pos);
            unit.transform.SetParent(tile.transform, false);
        }
    }

    public Unit GetUnit(Vector2Int pos)
    {
        return units[pos.x, pos.y];
    }

    public GridTile GetTile(Vector2Int pos)
    {
        return tiles[pos.x, pos.y];
    }

    public static bool IsValidPos(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    // move from one tile to another, must be a valid empty tile
    public void MoveUnit(Unit unit, Vector2Int pos)
    {
        if (!IsValidPos(pos) || GetUnit(pos) != null) {
            Debug.LogError("Invalid Move");
        }
        var oldPos = unit.Pos;
        SetUnit(unit, pos);
        SetUnit(null, oldPos);
    }

}