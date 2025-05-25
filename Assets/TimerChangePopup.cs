using DG.Tweening;
using TMPro;
using UnityEngine;

public class TimerChangePopup : MonoBehaviour
{
    [SerializeField] TMP_Text descriptionText, timeText;
    [SerializeField] float upShift = 5f;
    [SerializeField] bool isPositive;

    public void Display(float time, string desc)
    {
        if (isPositive) timeText.text = $"+ {Timer.FormatTime(time)}";
        else timeText.text = $"- {Timer.FormatTime(time)}";
        descriptionText.text = desc;
        transform.DOMoveY(transform.position.y + upShift, 2f);
        descriptionText.DOColor(Color.clear, 2f);
        timeText.DOColor(Color.clear, 2f).OnComplete(() => Destroy(gameObject));
    }
}
