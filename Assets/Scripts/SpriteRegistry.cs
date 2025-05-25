using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRegistry : MonoBehaviour
{
    [SerializeField] private DamageIcons _dTypeSprites;

    public static Dictionary<DamageType, Sprite> dTypeSprites;
    public static Dictionary<DamageType, Color> dTypeColors;
    public static Dictionary<DamageType, Sprite> immuneSprites;

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
        };

        dTypeColors = new Dictionary<DamageType, Color>()
        {
            {DamageType.Ballistic,  _dTypeSprites.ballisticColor},
            {DamageType.Energy, _dTypeSprites.energyColor},
            {DamageType.Explosive, _dTypeSprites.explosiveColor }
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


    }

    [Serializable]
    public class DamageIcons
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

        // U, D, L, R
        public Sprite[] knockbackIcons = new Sprite[4];

    }

    [Serializable]

    public class Colors
    {
        public Color damageBoostedAdditive, damageImmuneAdditive, damageAppliedAdditive;
    }
}
