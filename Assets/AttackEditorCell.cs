using TMPro;
using UnityEngine;

public class AttackEditorCell : MonoBehaviour
{
    [SerializeField] TMP_Text damageNumber, damageType;
    public TileAttack TileAttack;

    private void OnValidate()
    {
        damageNumber.text = TileAttack.damage.ToString();
        damageType.text = TileAttack.damageType.ToString();
    }

}
