using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TileType
{
    Normal, Chasm
}

public enum BiomeType
{
    Stone,
}

public class GridTile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public Vector2Int Pos;
    [SerializeField] Image dTypeImage;
    [SerializeField] TMP_Text damageNumber;

    public TileType Type;

    public void Init(Vector2Int pos)
    {
        this.Pos = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerControl.Instance.ClickTile(Pos);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerControl.Instance.HoverTile(Pos);  
    }

    public void ShowAttackOverlay(TileAttack tAtk)
    {
        damageNumber.fontStyle = FontStyles.Normal;
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

    public void ShowUnitAttackOverlay(DamagePreviewInfo info)
    {
        if (info == null || (info.knockback == Directions.None && info.immune == false && info.damage == 0))
        {
            dTypeImage.enabled = false;
            damageNumber.text = "";
            damageNumber.fontStyle = FontStyles.Normal;
            return;
        }
        else
        {
            dTypeImage.enabled = true;
            if (info.knockback == Directions.None)
            {
                dTypeImage.sprite = SpriteRegistry.dTypeSprites[info.dType];
                damageNumber.text = info.damage.ToString();
                damageNumber.color = SpriteRegistry.dTypeColors[info.dType];
                if (info.boosted)
                {
                    damageNumber.fontStyle = FontStyles.Bold;
                    damageNumber.color = damageNumber.color.AlphaBlend(SpriteRegistry.colors.damageBoostedAdditive);
                }
                else if (info.immune)
                {
                    damageNumber.color = damageNumber.color.AlphaBlend(SpriteRegistry.colors.damageImmuneAdditive);
                }
                else
                {
                    damageNumber.color = damageNumber.color.AlphaBlend(SpriteRegistry.colors.damageAppliedAdditive);
                }
            }
            else
            {
                damageNumber.text = "";
                dTypeImage.sprite = SpriteRegistry.knockbackIcons[info.knockback];
            }
        }

    }
}
