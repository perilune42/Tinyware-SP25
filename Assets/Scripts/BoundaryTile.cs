using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// used to target tiles beyond the grid border (so an outer edge of an attack can hit edge enemies)
public class BoundaryTile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public Vector2Int Pos;

    [SerializeField] Image dTypeImage;
    [SerializeField] TMP_Text damageNumber;

    public Sprite TileSprite;
    public Color SpriteColor;

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
