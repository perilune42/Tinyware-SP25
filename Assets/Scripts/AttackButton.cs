using TMPro;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    Attack attack;

    public void SetAttack(Attack attack)
    {
        nameText.text = attack.attackName;
        this.attack = attack;
    } 

    public void Select()
    {
        PlayerControl.Instance.SelectAttack(attack);
    }
}
