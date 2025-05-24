
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public List<Attack> Attacks;

    private void Start()
    {
        Attacks = GetComponentsInChildren<Attack>().ToList();
        foreach (var attack in Attacks)
        {
            attack.gameObject.SetActive(true);
            attack.Init();
        }
        AttackListUI.Instance.SetAttacks(Attacks);
    }
}