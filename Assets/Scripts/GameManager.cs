using Microsoft.Win32.SafeHandles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Unit> AllUnits;
    public List<Unit> FriendlyUnits;

    [SerializeField] List<Unit> possibleEnemies;
    [SerializeField] List<Unit> possibleFriendlies;
    [SerializeField] int numSpawns;

    List<UnitSpawn> prevRoundSpawns;
    [SerializeField] int[] addFriendlyRounds;

    private int currSeed;
    private static int prevSeed = 0;

    public Action<int> OnNewRound;
    int currRound;

    bool startingNextRound;

    [SerializeField] GameObject endScreen;
    [SerializeField] TMP_Text endScreenText;

    private void Awake()
    {
        Instance = this;
        if (prevSeed == 0)
        {
            currSeed = Random.Range(int.MinValue, int.MaxValue);
            Random.InitState(currSeed);
        }
        else
        {
            currSeed = prevSeed;
            Random.InitState(prevSeed);
        }
        Debug.Log(currSeed);
        
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
        if (unit.vulnerableDamage == DamageType.Friendly)
        {
            FriendlyUnits.Add(unit);
        }
    }

    public void RemoveUnit(Unit unit)
    {
        AllUnits.Remove(unit);
        if (FriendlyUnits.Contains(unit))
        {
            FriendlyUnits.Remove(unit);
            if (!startingNextRound)
            {
                Timer.Instance.RemoveTime(30f, "Friendly Fire!");
            }
        }
        if (!startingNextRound && AllUnits.Count == FriendlyUnits.Count) 
        {
            StartCoroutine(Util.DelayedCall(0.1f, StartRoundTransition));
        }
    }

    private void StartRoundTransition()
    {
        SoundPlayer.PlayWin();
        if (currRound == 15)
        {
            WinGame();
            return;
        }
        int remainingAtkCount = AttackManager.Instance.Attacks.Count;
        float timeReward = Timer.Instance.BaseTimeReward
            + Timer.Instance.TimeRewardPerRound * (currRound - 1)
            + remainingAtkCount * Timer.Instance.TimeRefund;
        Timer.Instance.AddTime(timeReward, $"Enemies Defeated! \n{remainingAtkCount} Attacks Remaining");
        FadeAnimation.Instance.Fade(NextRound);
    }

    private UnitSpawn ChooseUnit(List<Unit> pool, List<GridTile> availableTiles)
    {
        var chosenEnemy = pool.RandomElement();
        var tile = availableTiles.RandomElement();
        availableTiles.Remove(tile);
        return new(chosenEnemy, tile.Pos);
    }

    public List<UnitSpawn> GetSpawns()
    {
        int count = numSpawns;
        List<UnitSpawn> spawns = new();
        List<GridTile> availableTiles = new();
        foreach (GridTile tile in GameGrid.Instance.tiles)
        {
            if (tile.Type != TileType.Chasm && tile.Type != TileType.Mountain)
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
            spawns.Add(ChooseUnit(possibleEnemies, availableTiles));
        }

        spawns.Add(ChooseUnit(possibleFriendlies, availableTiles));

        return spawns;
    }

    private List<UnitSpawn> GetNextRoundSpawns()
    {
        List<UnitSpawn> spawns = new();
        spawns.AddRange(prevRoundSpawns);
        int addCount = numSpawns - prevRoundSpawns.Where((spawn) => spawn.unit.vulnerableDamage != DamageType.Friendly).Count();
        List<GridTile> availableTiles = new();
        foreach (GridTile tile in GameGrid.Instance.tiles)
        {
            if (tile.Type != TileType.Chasm && tile.Type != TileType.Mountain && GameGrid.Instance.GetUnit(tile.Pos) == null)
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
            spawns.Add(ChooseUnit(possibleEnemies, availableTiles));
        }

        if (addFriendlyRounds.Contains(currRound))
        {
            spawns.Add(ChooseUnit(possibleFriendlies, availableTiles));
        }

        prevRoundSpawns = spawns;
        return spawns;
    }

    private void NextRound()
    {
        startingNextRound = true;
        if (AllUnits.Count != FriendlyUnits.Count)
        {
            Debug.LogWarning("Ending round with enemies remaining");
        }

        for(int i = AllUnits.Count - 1; i >= 0; i--)
        {
            AllUnits[i].Death();
        }
        currRound++;
        numSpawns++;
        var spawns = GetNextRoundSpawns();
        GameGrid.Instance.InitBoard(spawns);
        AttackManager.Instance.DrawInitialAttacks(AttackManager.Instance.startingAttackCount);
        OnNewRound?.Invoke(currRound);
        startingNextRound = false;
    }

    private void LoseGame()
    {
        endScreen.SetActive(true);
        SoundPlayer.PlayLose();
    }

    private void WinGame()
    {
        endScreen.SetActive(true);
        Timer.Instance.StopCounting();
        endScreenText.text = "VICTORY";
        endScreenText.color = SpriteRegistry.colors.friendly;
        
    }

    public void Restart()
    {
        prevSeed = 0;
        SceneManager.LoadScene("Main");
    }

    public void Repeat()
    {
        prevSeed = currSeed;
        SceneManager.LoadScene("Main");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
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