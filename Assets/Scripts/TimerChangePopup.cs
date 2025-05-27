using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerChangePopup : MonoBehaviour
{
    [SerializeField] TMP_Text descriptionText, timeText;
    [SerializeField] float upShift = 3f;
    [SerializeField] bool isPositive;

    public void Display(float time, string desc)
    {
        if (isPositive) timeText.text = $"+ {Timer.FormatTime(time)}";
        else timeText.text = $"- {Timer.FormatTime(time)}";
        descriptionText.text = desc;
        transform.DOMoveY(transform.position.y + upShift, 7f);
        descriptionText.DOColor(Color.clear, 7f);
        timeText.DOColor(Color.clear, 7f).OnComplete(() => { if (gameObject != null) Destroy(gameObject); });
    }
}
