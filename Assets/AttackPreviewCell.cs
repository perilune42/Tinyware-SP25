using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackPreviewCell : MonoBehaviour
{
    [SerializeField] TMP_Text damageNumber;
    [SerializeField] Image dTypeImage;

    public void SetTileAttack(TileAttack tAtk)
    {
        if (tAtk.IsNullAttack())
        {
            damageNumber.text = "";
            dTypeImage.enabled = false;
            return;
        }
        dTypeImage.enabled = true;
        damageNumber.text = tAtk.damage.ToString();
        damageNumber.color = SpriteRegistry.dTypeColors[tAtk.damageType];
        dTypeImage.sprite = SpriteRegistry.dTypeSprites[tAtk.damageType];

    }

}
