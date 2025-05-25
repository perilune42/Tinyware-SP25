using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    public static UnitInfoUI Instance;
    [SerializeField] TMP_Text unitName;
    [SerializeField] Image unitPortrait;

    [SerializeField] Image immuneIcon, vulnerableIcon;
    [SerializeField] TMP_Text immuneText, vulnerableText;

    private void Awake()
    {
        Instance = this;
        ViewUnitInfo(null);
    }

    public void ViewUnitInfo(Unit unit)
    {
        if (unit == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        unitName.text = unit.UnitName;
        unitPortrait.sprite = unit.Sprite;
        immuneIcon.sprite = SpriteRegistry.dTypeSprites[unit.immuneDamage];
        immuneText.text = unit.immuneDamage.ToString();
        immuneText.color = SpriteRegistry.dTypeColors[unit.immuneDamage];
        vulnerableIcon.sprite = SpriteRegistry.dTypeSprites[unit.vulnerableDamage];
        vulnerableText.text = unit.vulnerableDamage.ToString();
        vulnerableText.color = SpriteRegistry.dTypeColors[unit.vulnerableDamage];
    }
}