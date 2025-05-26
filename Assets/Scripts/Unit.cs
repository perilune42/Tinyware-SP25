using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public enum UnitType
{
    Ground, Aerial, Static
}

public class Unit : MonoBehaviour
{
    public string UnitName;
    public Sprite Sprite;
    [SerializeField] Sprite[] spriteSheet;

    public Vector2Int Pos;
    public int currHealth;
    public int maxHealth;
    public DamageType immuneDamage;
    public DamageType vulnerableDamage;
    public UnitType unitType;

    
    [SerializeField] HealthBar healthBar;
    [SerializeField] Image unitImage;
    


    public void SetPos(Vector2Int pos)
    {
        Pos = pos;
    }

    private void Awake()
    {
        currHealth = maxHealth;
        healthBar.Initialize(maxHealth, vulnerableDamage, immuneDamage);
        unitImage.sprite = Sprite;
        Animations.Instance.OnIdleAnimation += SwitchToFrame;
    }

    public DamagePreviewInfo SimulateAttack(TileAttack tAtk)
    {
        if (tAtk == null || tAtk.IsNullAttack())
        {
            return null;
        }
        DamagePreviewInfo info = new DamagePreviewInfo();
        var dType = tAtk.damageType;
        info.dType = tAtk.damageType;
        if (dType == immuneDamage)
        {
            info.damage = 0;
            info.immune = true;
        }
        else if (dType == vulnerableDamage && tAtk.damage > 0)
        {
            info.damage = tAtk.damage * 2;
            info.boosted = true;
        }
        else
        {
            info.damage = tAtk.damage;
        }
        info.knockback = tAtk.knockbackDirection;
        return info;
    }

    public void Damage(DamagePreviewInfo info)
    {
        currHealth -= info.damage;
        healthBar.ShowHealth(currHealth, maxHealth);
        if (currHealth <= 0) 
        {
            Death();
        }
    }

    public void PreviewHealth(DamagePreviewInfo info)
    {
        if (info == null)
        {
            healthBar.ShowHealth(currHealth, maxHealth);
            return;
        }
        healthBar.PreviewHealth(currHealth, maxHealth, info.damage);
    }

    public void Knockback(Vector2Int dir)
    {
        if (unitType == UnitType.Static)
        {
            return;
        }
        if (dir.magnitude != 1)
        {
            Debug.LogError($"Invalid knockback value: {dir}");
            return;
        }

        List<Unit> pushChain = new List<Unit>() { this };
        var currPos = Pos;
        bool aerialPushed = false;
        if (unitType == UnitType.Aerial && GameGrid.Instance.IsPassable(Pos + dir) && GameGrid.Instance.GetUnit(Pos + dir) == null)
        {
            currPos += dir;
            aerialPushed = true;
        }
        bool ended = false;
        bool blocked = false;
        int iter = 0;
        while (!ended && iter++ < 20)
        {
            currPos += dir;
            Unit unit = null;
            if (GameGrid.IsValidPos(currPos)) unit = GameGrid.Instance.GetUnit(currPos);
            // blocked tile
            if (!GameGrid.Instance.IsPassable(currPos) || (unit != null && unit.unitType == UnitType.Static))
            {
                ended = true;
                blocked = true;
            }
            // found a free space
            else if (unit == null){
                ended = true;
            }
            else
            {
                pushChain.Add(GameGrid.Instance.GetUnit(currPos));
            }
        }
        if (blocked)
        {
            if (aerialPushed)
            {
                GameGrid.Instance.MoveUnit(this, Pos + dir);
            }
            return;
        }

        for (int i = pushChain.Count - 1; i >= 0; i--)
        {
            Unit toPush = pushChain[i];
            if (toPush == this && aerialPushed)
            {
                GameGrid.Instance.MoveUnit(this, toPush.Pos + dir * 2);
            }
            else
            {
                GameGrid.Instance.MoveUnit(toPush, toPush.Pos + dir);
            }
        }
    }

    public void Death()
    {
        GameGrid.Instance.SetUnit(null, Pos);
        GameManager.Instance.RemoveUnit(this);
        Animations.Instance.OnIdleAnimation -= SwitchToFrame;
        Destroy(gameObject);
    }

    private void SwitchToFrame(int frame)
    {
        if (frame >= spriteSheet.Length) return;
        unitImage.sprite = spriteSheet[frame];
    }

}
