using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public Vector2Int Pos;
    public int currHealth;
    public int maxHealth;

    [SerializeField] HealthBar healthBar;


    public void SetPos(Vector2Int pos)
    {
        Pos = pos;
    }

    private void Awake()
    {
        currHealth = maxHealth;
        healthBar.ShowHealth(currHealth, maxHealth);
    }

    public void Damage(int damage, DamageType dType)
    {
        currHealth -= damage;
        healthBar.ShowHealth(currHealth, maxHealth);
    }

    public void Knockback(Vector2Int dir)
    {
        if (dir.magnitude != 1)
        {
            Debug.LogError($"Invalid knockback value: {dir}");
            return;
        }
        List<Unit> pushChain = new List<Unit>() { this };
        var currPos = Pos;
        bool ended = false;
        bool blocked = false;
        int iter = 0;
        while (!ended && iter++ < 20)
        {
            currPos += dir;
            // off board
            if (!GameGrid.IsValidPos(currPos))
            {
                ended = true;
                blocked = true;
            }
            // found a free space
            else if (GameGrid.Instance.GetUnit(currPos) == null){
                ended = true;
            }
            else
            {
                pushChain.Add(GameGrid.Instance.GetUnit(currPos));
            }
        }
        if (blocked)
        {
            return;
        }

        for (int i = pushChain.Count - 1; i >= 0; i--)
        {
            Unit toPush = pushChain[i];
            GameGrid.Instance.MoveUnit(toPush, toPush.Pos + dir);
        }
    }

}
