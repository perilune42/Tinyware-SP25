using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRegistry : MonoBehaviour
{
    [SerializeField] private Icons _dTypeSprites;

    public static Dictionary<DamageType, Sprite> dTypeSprites;
    public static Dictionary<DamageType, Color> dTypeColors;
    public static Dictionary<DamageType, Sprite> immuneSprites;

    public static Dictionary<UnitType, Sprite> unitTypeSprites;

    public static Dictionary<Vector2Int, Sprite> knockbackIcons;

    public static Colors colors;
    [SerializeField] private Colors _colors;

    private void Awake()
    {
        colors = _colors;
        dTypeSprites = new Dictionary<DamageType, Sprite>()
        {
            {DamageType.Ballistic,  _dTypeSprites.ballisticIcon},
            {DamageType.Energy, _dTypeSprites.energyIcon},
            {DamageType.Explosive, _dTypeSprites.explosiveIcon},
            {DamageType.Friendly,  _dTypeSprites.friendlyIcon}
        };

        dTypeColors = new Dictionary<DamageType, Color>()
        {
            {DamageType.Ballistic,  _dTypeSprites.ballisticColor},
            {DamageType.Energy, _dTypeSprites.energyColor},
            {DamageType.Explosive, _dTypeSprites.explosiveColor },
            {DamageType.Friendly,  _dTypeSprites.friendlyColor}
        };

        knockbackIcons = new Dictionary<Vector2Int, Sprite>()
        {
            {Directions.Up, _dTypeSprites.knockbackIcons[0] },
            {Directions.Down, _dTypeSprites.knockbackIcons[1] },
            {Directions.Left, _dTypeSprites.knockbackIcons[2] },
            {Directions.Right, _dTypeSprites.knockbackIcons[3] },
        };

        immuneSprites = new Dictionary<DamageType, Sprite>()
        {
            {DamageType.Ballistic,  _dTypeSprites.ballisticImmuneIcon},
            {DamageType.Energy, _dTypeSprites.energyImmuneIcon},
            {DamageType.Explosive, _dTypeSprites.explosiveImmuneIcon},
        };

        unitTypeSprites = new Dictionary<UnitType, Sprite>()
        {
            { UnitType.Ground, _dTypeSprites.groundIcon },
            { UnitType.Aerial, _dTypeSprites.aerialIcon },
            { UnitType.Static, _dTypeSprites.staticIcon },
        };

    }

    [Serializable]
    public class Icons
    {
        public Sprite ballisticIcon;
        public Sprite ballisticImmuneIcon;
        public Color ballisticColor;
        public Sprite energyIcon;
        public Sprite energyImmuneIcon;
        public Color energyColor;
        public Sprite explosiveIcon;
        public Sprite explosiveImmuneIcon;
        public Color explosiveColor;

        public Sprite friendlyIcon;
        public Color friendlyColor;


        public Sprite groundIcon;
        public Sprite aerialIcon;
        public Sprite staticIcon;

        // U, D, L, R
        public Sprite[] knockbackIcons = new Sprite[4];

    }

    [Serializable]

    public class Colors
    {
        public Color damageBoostedAdditive, damageImmuneAdditive, damageAppliedAdditive, friendly, enemy;
    }
}
