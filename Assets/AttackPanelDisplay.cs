using TMPro;
using UnityEngine;

public class AttackPanelDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;

    private void Awake()
    {
        PlayerControl.Instance.OnAttackChanged += ShowAttackInfo;
        Clear();
    }

    public void ShowAttackInfo(Attack attack)
    {
        nameText.text = attack.attackName;
    }

    public void Clear()
    {
        nameText.text = "";
    }
}
