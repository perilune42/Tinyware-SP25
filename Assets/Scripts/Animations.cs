using System;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    [Serializable]
    private class AttackVFX
    {
        public DamageType damageType;
        public GameObject effect;
    }

    [Serializable]
    private class KnockbackVFX
    {
        public Vector2Int knockbackDir;
        public GameObject effect;
    }

    public static Animations Instance;

    public Action<int> OnIdleAnimation;

    [SerializeField] AttackVFX[] atkVfx;
    [SerializeField] KnockbackVFX[] knockbackVfx;

    private Dictionary<DamageType, AttackVFX> atkVfxDict = new();
    private Dictionary<Vector2Int, KnockbackVFX> knockbackVfxDict = new();


    private void Awake()
    {
        Instance = this;
        foreach (var atk in atkVfx)
        {
            atkVfxDict.Add(atk.damageType, atk);
        }
        foreach (var kb in knockbackVfx)
        {
            knockbackVfxDict.Add(kb.knockbackDir, kb);
        }
    }

    private void Start()
    {
        Timer.Instance.OnTimerTick += BroadcastIdleAnims;
    }

    private void BroadcastIdleAnims(float time)
    {
        int frameNum = (int)(time * 2) % 2;
        OnIdleAnimation?.Invoke(frameNum);
    }

    public void PlayAttackVFX(DamageType damageType, Vector2Int pos)
    {
        var newEffect = Instantiate(atkVfxDict[damageType].effect);
        newEffect.transform.SetParent(GameGrid.Instance.GetTile(pos).transform, false);
        Destroy(newEffect, 1f);
    }

    public void PlayKnockbackVFX(Vector2Int knockbackDir, Vector2Int pos)
    {
        var newEffect = Instantiate(knockbackVfxDict[knockbackDir].effect);
        newEffect.transform.SetParent(GameGrid.Instance.GetTile(pos).transform, false);
        newEffect.transform.SetParent(transform, true);
        Destroy(newEffect, 1f);
    }


}
