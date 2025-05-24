using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridTile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public Vector2Int Pos;
    [SerializeField] Image dTypeImage;
    [SerializeField] TMP_Text damageNumber;
    

    public void Init(Vector2Int pos)
    {
        this.Pos = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerControl.Instance.ClickTile(Pos);
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
        damageNumber.text = tAtk.damage.ToString();
        damageNumber.color = SpriteRegistry.dTypeColors[tAtk.damageType];
        dTypeImage.sprite = SpriteRegistry.dTypeSprites[tAtk.damageType];

    }
}
