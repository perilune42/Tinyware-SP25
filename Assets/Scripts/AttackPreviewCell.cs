using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackPreviewCell : MonoBehaviour
{
    [SerializeField] TMP_Text damageNumber;
    [SerializeField] Image dTypeImage;

    public void SetTileAttack(TileAttack tAtk)
    {
        if (tAtk == null || tAtk.IsNullAttack())
        {
            damageNumber.text = "";
            dTypeImage.enabled = false;
            return;
        }
        dTypeImage.enabled = true;
        if (tAtk.damage == 0)
        {
            Debug.Assert(tAtk.knockbackDirection != Directions.None);
            damageNumber.text = "";
            dTypeImage.sprite = SpriteRegistry.knockbackIcons[tAtk.knockbackDirection];
        }
        else
        {
            damageNumber.text = tAtk.damage.ToString();
            damageNumber.color = SpriteRegistry.dTypeColors[tAtk.damageType];
            dTypeImage.sprite = SpriteRegistry.dTypeSprites[tAtk.damageType];
        }
    }

}
