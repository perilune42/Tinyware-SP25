using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance;


    public const int gridWidth = 5;
    public const int gridHeight = 5;
    public GridTile[,] tiles = new GridTile[gridWidth,gridHeight];
    Dictionary<Vector2Int, BoundaryTile> boundaryTiles = new();
    Unit[,] units = new Unit[gridWidth,gridHeight];

    [SerializeField] GridLayoutGroup tileContainer;

    [SerializeField] List<GridTile> possibleTiles = new();
    [SerializeField] TileGenerationData[] tileGeneration;
    private Dictionary<TileType, TileGenerationData> tileGenDict;

    [HideInInspector] public bool MovingUnits;

    [Serializable]
    private struct TileGenerationData
    {
        public TileType tileType;
        public float weight;
        public int min;
        public int max;
    }

    


    private void Awake()
    {
        Instance = this;
        tileGenDict = new Dictionary<TileType, TileGenerationData>();
        foreach (var tgd in tileGeneration)
        {
            tileGenDict.Add(tgd.tileType, tgd);
        }
        InitTiles();
    }

    public void InitBoard(List<UnitSpawn> spawns)
    {
        units = new Unit[gridWidth, gridHeight];
        foreach (var unitSpawn in spawns)
        {
            GridTile tile = GetTile(unitSpawn.pos);
            var newUnit = Instantiate(unitSpawn.unit, tile.transform);
            SetUnit(newUnit, unitSpawn.pos);
            GameManager.Instance.AddUnit(newUnit);
        }
    }

    private GridTile ChooseTile(Vector2Int pos)
    {
        // choose biome
        // BiomeType chosenBiome = BiomeType.Stone;

        // choose tiletype
        TileType chosenTileType = tileGenDict.GetWeightedRandomKey((tgd) => tgd.weight);
        List<GridTile> availableTiles = new List<GridTile>();
        foreach (var tile in possibleTiles)
        {
            if (tile.Type == chosenTileType)
            {
                availableTiles.Add(tile);
            }
        }
        if (availableTiles.Count > 0)
        {
            return availableTiles.RandomElement();
        }
        Debug.LogError("Failed to find a tile");
        return null;
    }

    private void InitTiles()
    {
        GridTile[] foundTiles = GetComponentsInChildren<GridTile>();
        foreach (var tile in foundTiles)
        {
            Destroy(tile.gameObject);
        }

        int iter = 0;
        while (iter++ < 20)
        {
            foreach (var tile in GetComponentsInChildren<GridTile>())
            {
                Destroy(tile.gameObject);
            }
            Dictionary<TileType, int> tileTypeCounts = new();
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    GridTile tile = Instantiate(ChooseTile(pos), tileContainer.transform);
                    tile.Init(pos);
                    tiles[x, y] = tile;
                    if (tileTypeCounts.ContainsKey(tile.Type))
                    {
                        tileTypeCounts[tile.Type]++;
                    }
                    else
                    {
                        tileTypeCounts.Add(tile.Type, 1);
                    }
                }
            }
            bool failed = false;
            foreach (var tType in tileGenDict.Keys)
            {
                if (!tileTypeCounts.ContainsKey(tType) || tileTypeCounts[tType] < tileGenDict[tType].min || tileTypeCounts[tType] > tileGenDict[tType].max)
                {
                    failed = true;
                    break;
                }
            }
            if (!failed) break;
        }
        if (iter >= 20)
        {
            Debug.LogError("Failed to generate grid that satisfies conditions");
        }

    }

    public void AddBoundaryTile(BoundaryTile bTile, Vector2Int bPos)
    {
        Debug.Assert(IsOnBoundary(bPos));
        boundaryTiles.Add(bPos, bTile);
    }

    // does not do any checks
    public void SetUnit(Unit unit, Vector2Int pos, bool moveVisual = true)
    {
        units[pos.x, pos.y] = unit;
        if (unit != null)
        {
            unit.SetPos(pos);
            GridTile tile = GetTile(pos);
            unit.transform.SetParent(tile.transform, true);
            if (moveVisual) unit.transform.localPosition = Vector3.zero;
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
    public BoundaryTile GetBoundaryTile(Vector2Int pos)
    {
        return boundaryTiles[pos];
    }

    public static bool IsValidPos(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    public bool IsPassable(Vector2Int pos)
    {
        return (IsValidPos(pos) && GetTile(pos).Type != TileType.Mountain) || (IsLethalBoundaryTile(pos));
    }

    public static bool IsOnBoundary(Vector2Int pos)
    {
        return ((pos.x == -1 || pos.x == gridWidth) && (pos.y >= -1 && pos.y <= gridHeight))||
            ((pos.y == -1 || pos.y == gridHeight) && (pos.x >= -1 && pos.x <= gridWidth)) ; 
    }

    // move from one tile to another, must be a valid empty tile
    public void MoveUnit(Unit unit, Vector2Int pos)
    {
        if (!IsLethalBoundaryTile(pos) && (!IsValidPos(pos) || GetUnit(pos) != null)) {
            Debug.LogError("Invalid Move");
        }
        var oldPos = unit.Pos;
        if (!IsLethalBoundaryTile(pos))
        {
            SetUnit(unit, pos, false);
        }
        else
        {
            unit.SetPos(pos);
        }
        SetUnit(null, oldPos);
        if (IsLethalBoundaryTile(pos))
        {
            unit.transform.DOMove(GetBoundaryTile(pos).transform.position, 0.3f).OnComplete(() => FinalizeMove(unit, pos));
        }
        else
        {
            unit.transform.DOMove(GetTile(pos).transform.position, 0.3f).OnComplete(() => FinalizeMove(unit, pos));
        }
        MovingUnits = true;
    }

    private void FinalizeMove(Unit unit, Vector2Int pos)
    {
        if (IsValidPos(pos))
        {
            unit.transform.localPosition = Vector3.zero;
        }
        if (IsLethalBoundaryTile(pos) || (unit.unitType != UnitType.Aerial && GetTile(pos).Type == TileType.Chasm))
        {
            unit.Death();
        }
        MovingUnits = false;
    }

    public bool IsLethalBoundaryTile(Vector2Int pos)
    {
        return (IsOnBoundary(pos) && GetBoundaryTile(pos).IsLethal);
    }

}