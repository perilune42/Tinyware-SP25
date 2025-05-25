
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager Instance;

    public List<Attack> Attacks;
    [SerializeField] List<Attack> possibleAttacks;
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

    private void AddAttack(Attack attackPrefab)
    {
        var newAtk = Instantiate(attackPrefab, transform);
        Attacks.Add(newAtk);
        newAtk.Init();
        AttackListUI.Instance.SetAttacks(Attacks);
    }

    public void DrawNewAttack()
    {
        var chosenAttack = possibleAttacks.RandomElement();
        AddAttack(chosenAttack);
        Timer.Instance.RemoveTime(Timer.Instance.DrawPenalty, "");
    }

    public void RemoveAttack(Attack attack)
    {
        Attacks.Remove(attack);
        Destroy(attack.gameObject);
        AttackListUI.Instance.SetAttacks(Attacks);
    }
}