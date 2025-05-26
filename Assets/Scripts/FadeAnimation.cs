using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class FadeAnimation : MonoBehaviour
{
    [SerializeField] Image image;

    public static FadeAnimation Instance;
    private const float duration = 1f;

    private void Awake()
    {
        Instance = this;
    }

    public void Fade(Action onEnd)
    {
        var seq = DOTween.Sequence();
        seq.Append(image.DOColor(Color.black, duration).OnComplete(() => onEnd?.Invoke()));
        seq.Append(image.DOColor(Color.clear, duration));
        seq.Play();
    }

}
