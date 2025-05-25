using TMPro;
using UnityEngine;

public class AttackPreview : MonoBehaviour
{
    private AttackPreviewCell[] linCells;
    public static AttackPreview Instance;
    [SerializeField] TMP_Text nameText;


    private void Awake()
    {
        Instance = this;
        PlayerControl.Instance.OnAttackChanged += SetAttack;
        linCells = GetComponentsInChildren<AttackPreviewCell>();
        nameText.text = "";
    }

    public void SetAttack(Attack atk)
    {
        if (atk == null)
        {
            nameText.text = "";
            foreach (var cell in linCells)
            {
                cell.SetTileAttack(null);
            }
            return;
        }
        nameText.text = atk.attackName;
        var linTileAttacks = atk.tileAttacks.LinearizeArray();
        for (int i = 0; i < linTileAttacks.Length; i++) {
            linCells[i].SetTileAttack(linTileAttacks[i]);
        }
    }

    public void SetText(string str)
    {
        nameText.text = str;
    }
    


}
