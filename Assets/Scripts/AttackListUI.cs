using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AttackListUI : MonoBehaviour
{
    public static AttackListUI Instance;
    List<AttackButton> attackButtons = new();
    [SerializeField] AttackButton attackButtonPrefab;
    [SerializeField] DrawButton drawButtonPrefab;
    [SerializeField] int maxAttacks;

    private void Awake()
    {
        Instance = this;
        ClearButtons();
    }
    public void SetAttacks(List<Attack> attacks)
    {
        ClearButtons();
        foreach (var atk in attacks)
        {
            AttackButton newButton = Instantiate(attackButtonPrefab, transform);
            newButton.SetAttack(atk);
            attackButtons.Add(newButton);
        }
        if (attackButtons.Count < maxAttacks)
        {
            DrawButton drawButton = Instantiate(drawButtonPrefab, transform);
        }
    }
    private void ClearButtons()
    {
        foreach (var atkButton in GetComponentsInChildren<AttackButton>())
        {
            Destroy(atkButton.gameObject);
        }
        foreach (var drawButton in GetComponentsInChildren<DrawButton>())
        {
            Destroy(drawButton.gameObject);
        }
        attackButtons.Clear();
    }
}
