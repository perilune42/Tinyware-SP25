using UnityEngine;

public class AttackPreview : MonoBehaviour
{
    private AttackPreviewCell[] linCells;
    private void Awake()
    {
        PlayerControl.Instance.OnAttackChanged += SetAttack;
        linCells = GetComponentsInChildren<AttackPreviewCell>();
    }

    public void SetAttack(Attack atk)
    {
        if (atk == null)
        {
            foreach (var cell in linCells)
            {
                cell.SetTileAttack(null);
            }
            return;
        }
        var linTileAttacks = atk.tileAttacks.LinearizeArray();
        for (int i = 0; i < linTileAttacks.Length; i++) {
            linCells[i].SetTileAttack(linTileAttacks[i]);
        }
    }
    


}
