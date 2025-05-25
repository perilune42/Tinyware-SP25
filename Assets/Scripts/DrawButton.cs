using UnityEngine;
using UnityEngine.EventSystems;

public class DrawButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void Draw()
    {
        AttackSelector.Instance.ShowNewAttacks();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AttackPreview.Instance.SetText($"Draw New Attack  <color=#FFAEB2FF>[-{Timer.FormatTime(Timer.Instance.GetDrawPenalty())}]</color>");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AttackPreview.Instance.SetAttack(PlayerControl.Instance.SelectedAttack);
    }
}