using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image immuneIcon;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] HorizontalLayoutGroup barSegmentsContainer;
    [SerializeField] Image barSegmentPrefab;

    Color barBaseColor;
    [SerializeField] Color damagePreviewAdditive;
    [SerializeField] Color emptyColor;

    List<Image> segments = new();

    public void ShowHealth(int curr, int max)
    {
        for (int i = 0; i < max; i++)
        {
            var segment = segments[i];
            if (i < curr)
            {
                segment.color = barBaseColor;
            }
            else
            {
                segment.color = emptyColor;
            }
        }
    }

    public void PreviewHealth(int curr, int max, int dmg)
    {
        for (int i = 0; i < max; i++)
        {
            var segment = segments[i];
            if (i < curr - dmg)
            {
                segment.color = barBaseColor;
            }
            else if (i < curr)
            {
                segment.color = barBaseColor + damagePreviewAdditive;
            }
            else
            {
                segment.color = emptyColor;
            }
        }
    }


    public void Initialize(int maxHealth, DamageType vType, DamageType iType)
    {
        foreach (var item in barSegmentsContainer.transform.GetComponentsInChildren<Image>()) {
            Destroy(item.gameObject);
        }
        barBaseColor = SpriteRegistry.dTypeColors[vType];
        for (int i = 0; i < maxHealth; i++)
        {
            var newSegment = Instantiate(barSegmentPrefab, barSegmentsContainer.transform);
            segments.Add(newSegment);
            newSegment.color = barBaseColor;
        }
        ShowImmune(iType);
    }

    private void ShowImmune(DamageType dType)
    {
        immuneIcon.sprite = SpriteRegistry.immuneSprites[dType];
    }

}
