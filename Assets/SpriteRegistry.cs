using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRegistry : MonoBehaviour
{
    [SerializeField] private DamageIcons _dTypeSprites;

    public static Dictionary<DamageType, Sprite> dTypeSprites;
    public static Dictionary<DamageType, Color> dTypeColors;

    private void Awake()
    {
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
    }

    [Serializable]
    public class DamageIcons
    {
        public Sprite ballisticIcon;
        public Color ballisticColor;
        public Sprite energyIcon;
        public Color energyColor;
        public Sprite explosiveIcon;
        public Color explosiveColor;

    }
}
