using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image attackIcon;
    Attack attack;

    public bool IsOwned = true;

    public void SetAttack(Attack attack)
    {
        attackIcon.sprite = attack.icon;
        this.attack = attack;
    } 

    public void Select()
    {
        if (IsOwned)
        {
            PlayerControl.Instance.SelectAttack(attack);
        }
        else
        {
            AttackManager.Instance.DrawAttack(attack);
            AttackSelector.Instance.Close();
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AttackPreview.Instance.SetAttack(attack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AttackPreview.Instance.SetAttack(PlayerControl.Instance.SelectedAttack);
    }
}
