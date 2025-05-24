using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// used to target tiles beyond the grid border (so an outer edge of an attack can hit edge enemies)
public class BoundaryTile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public Vector2Int Pos;

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

}
