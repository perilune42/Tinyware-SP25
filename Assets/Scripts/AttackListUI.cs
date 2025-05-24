using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AttackListUI : MonoBehaviour
{
    public static AttackListUI Instance;
    List<AttackButton> attackButtons = new();
    [SerializeField] AttackButton attackButtonPrefab;

    private void Awake()
    {
        Instance = this;
        foreach (var atkButton in GetComponentsInChildren<AttackButton>())
        {
            Destroy(atkButton.gameObject);
        }
    }
    public void SetAttacks(List<Attack> attacks)
    {
        foreach (var button in attackButtons)
        {
            Destroy(button.gameObject);
        }
        attackButtons.Clear();
        foreach (var atk in attacks)
        {
            AttackButton newButton = Instantiate(attackButtonPrefab, transform);
            newButton.SetAttack(atk);
        }
    }
}
