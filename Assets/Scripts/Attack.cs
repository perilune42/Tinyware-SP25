using UnityEngine;
using UnityEngine.Android;

public class Attack : MonoBehaviour
{
    public string attackName;
    public Sprite icon;

    public const int attackWidth = 3;
    public const int attackHeight = 3;

    public TileAttack[,] tileAttacks = new TileAttack[attackWidth, attackHeight];

    [HideInInspector] public bool Initialized = false;

    public AudioClip sfx;

    public void Init()
    {
        var editor = GetComponentInChildren<AttackEditor>(true);
        tileAttacks = editor.GetTileAttacks();
        Destroy(editor.gameObject);
        Initialized = true;
    }

    public void Execute(Vector2Int clickPos)
    {
        // bottom left
        Vector2Int rootPos = clickPos + new Vector2Int(-1, -1);
        tileAttacks.Iterate2D((tAtk, x, y) =>
        {
            if (tAtk == null || tAtk.IsNullAttack()) return;
            Vector2Int pos = rootPos + new Vector2Int(x, y);
            if (!GameGrid.IsValidPos(pos)) return;

            Unit unit = GameGrid.Instance.GetUnit(pos);

            if (tAtk.knockbackDirection != Directions.None)
            {
                Animations.Instance.PlayKnockbackVFX(tAtk.knockbackDirection, pos);
            }
            else
            {
                Animations.Instance.PlayAttackVFX(tAtk.damageType, pos);
            }

            if (unit == null) return;

            DamagePreviewInfo info = unit.SimulateAttack(tAtk);
            unit.Damage(info);
            if (tAtk.knockbackDirection != Directions.None)
            {
                unit.Knockback(tAtk.knockbackDirection);
            }
            
        }, true);
        // iterates top left -> bottom right (shouldn't matter in most cases)

        SoundPlayer.PlayAtkSound(this);
        AttackManager.Instance.RemoveAttack(this);
        
    }

}
