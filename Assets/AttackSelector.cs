using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSelector : MonoBehaviour
{
    public static AttackSelector Instance;


    [SerializeField] GridLayoutGroup buttonsContainer;
    [SerializeField] AttackButton attackButtonPrefab;

    List<AttackButton> buttons = new();

    bool isShowing = false;
    [SerializeField] int drawCount = 3;

    private void Awake()
    {
        Instance = this;
        foreach (var button in GetComponentsInChildren<AttackButton>())
        {
            Destroy(button.gameObject);
        }
        Close();
    }

    public void ShowNewAttacks()
    {
        if (isShowing) return;
        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
        buttonsContainer.gameObject.SetActive(true);
        isShowing = true;
        List<Attack> chosenAttacks = new();
        int iter = 0;
        while (iter++ < 50 && chosenAttacks.Count < drawCount)
        {
            var atk = AttackManager.Instance.possibleAttacks.RandomElement();
            if (!chosenAttacks.Contains(atk)) chosenAttacks.Add(atk);
        }

        foreach (Attack attackTemplate in chosenAttacks)
        {
            // scuffed
            var newAtk = Instantiate(attackTemplate, transform);
            newAtk.Init();
            AttackButton newButton = Instantiate(attackButtonPrefab, buttonsContainer.transform);
            newButton.SetAttack(newAtk);
            newButton.IsOwned = false;
            newAtk.transform.SetParent(newButton.transform);
            buttons.Add(newButton);
        }
    }

    public void Close()
    {
        isShowing = false;
        buttonsContainer.gameObject.SetActive(false);
    }
}
