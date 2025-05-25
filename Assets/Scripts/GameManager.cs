using Microsoft.Win32.SafeHandles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Unit> AllUnits;

    [SerializeField] List<Unit> possibleEnemies;
    [SerializeField] int numSpawns;

    List<UnitSpawn> prevRoundSpawns;

    public Action<int> OnNewRound;
    int currRound;

    [SerializeField] GameObject endScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var spawns = GetSpawns();
        prevRoundSpawns = spawns;
        GameGrid.Instance.InitBoard(spawns);
        currRound = 1;
        Timer.Instance.StartTimer();
        Timer.Instance.OnTimerEnd += LoseGame;
    }

    public void AddUnit(Unit unit)
    {
        AllUnits.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        AllUnits.Remove(unit);
        if (AllUnits.Count == 0) 
        {
            int remainingAtkCount = AttackManager.Instance.Attacks.Count;
            Timer.Instance.AddTime(remainingAtkCount * Timer.Instance.TimeRefund,
            $"{remainingAtkCount} Attacks Remaining");
            FadeAnimation.Instance.Fade(NextRound);
        }
    }



    public List<UnitSpawn> GetSpawns()
    {
        int count = numSpawns;
        List<UnitSpawn> spawns = new();
        List<GridTile> availableTiles = new();
        foreach (GridTile tile in GameGrid.Instance.tiles)
        {
            if (tile.Type != TileType.Chasm)
            {
                availableTiles.Add(tile);
            }
        }
        if (count > availableTiles.Count)
        {
            count = availableTiles.Count;
        }

        for (int i = 0; i < count; i++)
        {
            var chosenEnemy = possibleEnemies.RandomElement();
            var tile = availableTiles.RandomElement();
            spawns.Add(new(chosenEnemy, tile.Pos));
            availableTiles.Remove(tile);
        }
        return spawns;
    }

    private List<UnitSpawn> GetNextRoundSpawns()
    {
        List<UnitSpawn> spawns = new();
        spawns.AddRange(prevRoundSpawns);
        int addCount = numSpawns - prevRoundSpawns.Count;
        List<GridTile> availableTiles = new();
        foreach (GridTile tile in GameGrid.Instance.tiles)
        {
            if (tile.Type != TileType.Chasm && GameGrid.Instance.GetUnit(tile.Pos) == null)
            {
                availableTiles.Add(tile);
            }
        }

        foreach (var preExistingSpawn in spawns)
        {
            var pos = preExistingSpawn.pos;
            var tile = GameGrid.Instance.GetTile(pos);
            if (availableTiles.Contains(tile)) {
                availableTiles.Remove(tile);
            }
        }

        for (int i = 0; i < addCount; i++)
        {
            var chosenEnemy = possibleEnemies.RandomElement();
            var tile = availableTiles.RandomElement();
            spawns.Add(new(chosenEnemy, tile.Pos));
            availableTiles.Remove(tile);
        }
        prevRoundSpawns = spawns;
        return spawns;
    }

    private void NextRound()
    {
        
        currRound++;
        numSpawns++;
        var spawns = GetNextRoundSpawns();
        GameGrid.Instance.InitBoard(spawns);
        AttackManager.Instance.DrawInitialAttacks(AttackManager.Instance.startingAttackCount);
        OnNewRound?.Invoke(currRound);
    }

    private void LoseGame()
    {
        endScreen.SetActive(true);
    }
}

public class UnitSpawn
{
    public Unit unit;
    public Vector2Int pos;

    public UnitSpawn(Unit unit, Vector2Int pos)
    {
        this.unit = unit;
        this.pos = pos;
    }
}