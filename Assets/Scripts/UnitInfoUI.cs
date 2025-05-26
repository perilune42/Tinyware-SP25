using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    public static UnitInfoUI Instance;
    [SerializeField] TMP_Text headerText;

    [SerializeField] TMP_Text unitName;
    [SerializeField] Image unitPortrait;

    [SerializeField] Image immuneIcon, vulnerableIcon;
    [SerializeField] TMP_Text immuneText, vulnerableText;

    [SerializeField] TMP_Text vulnLabel;
    [SerializeField] Color enemyColor,friendlyColor,doNotDamageColor, baseLabelColor;

    [SerializeField] Image typeIcon;
    [SerializeField] TMP_Text typeText, knockbackText;

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

        if (unit.vulnerableDamage == DamageType.Friendly)
        {
            headerText.text = "CIVILIAN";
            unitName.color = SpriteRegistry.colors.friendly;
            vulnLabel.text = "FRIENDLY";
            vulnLabel.color = SpriteRegistry.colors.friendly;
            vulnerableText.text = "DEFEND";
            vulnerableText.color = doNotDamageColor;
        }
        else
        {
            headerText.text = "ENEMY";
            unitName.color = SpriteRegistry.colors.enemy;
            vulnLabel.text = "VULNERABILITY";
            vulnLabel.color = baseLabelColor;
            vulnerableText.text = unit.vulnerableDamage.ToString();
            vulnerableText.color = SpriteRegistry.dTypeColors[unit.vulnerableDamage];
        }

        typeIcon.sprite = SpriteRegistry.unitTypeSprites[unit.unitType];
        typeText.text = unit.unitType.ToString();
        int kbMult = 0;
        if (unit.unitType == UnitType.Ground) kbMult = 1;
        else if (unit.unitType == UnitType.Aerial) kbMult = 2;
        knockbackText.text = $"x{kbMult} Knockback";

    }
}