
using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TileAttack
{
    

    public int damage;
    public DamageType damageType;
    public Vector2Int knockbackDirection;

    public bool IsNullAttack()
    {
        return (damage == 0 && knockbackDirection == Directions.None);
    }
}