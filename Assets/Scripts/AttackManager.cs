
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager Instance;

    public List<Attack> Attacks;
    public List<Attack> possibleAttacks;
    [SerializeField] public int startingAttackCount = 3;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DrawInitialAttacks(startingAttackCount);
    }

    public void DrawInitialAttacks(int count)
    {
        Attacks.Clear();
        foreach (var atk in GetComponentsInChildren<Attack>())
        {
            Destroy(atk.gameObject);
        }
        for (int i = 0; i < count; i++)
        {
            var chosenAttack = possibleAttacks.RandomElement();
            AddAttack(chosenAttack);
        }
    }

    private void AddAttack(Attack attack)
    {
        Attack newAtk;
        if (attack.Initialized)
        {
            newAtk = attack;
            newAtk.transform.SetParent(transform);
        }
        else
        {
            newAtk = Instantiate(attack, transform);
            newAtk.Init();
        }
        Attacks.Add(newAtk);
        AttackListUI.Instance.SetAttacks(Attacks);
    }

    public void DrawAttack(Attack attack)
    {
        AddAttack(attack);
        Timer.Instance.ApplyDrawPenalty();
    }

    public void RemoveAttack(Attack attack)
    {
        Attacks.Remove(attack);
        Destroy(attack.gameObject);
        AttackListUI.Instance.SetAttacks(Attacks);
    }
}