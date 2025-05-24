using UnityEngine;
using UnityEngine.Android;

public class Attack : MonoBehaviour
{
    public string attackName;

    public const int attackWidth = 3;
    public const int attackHeight = 3;

    public TileAttack[,] tileAttacks = new TileAttack[attackWidth, attackHeight];

    public void Init()
    {
        var editor = GetComponentInChildren<AttackEditor>(true);
        tileAttacks = editor.GetTileAttacks();
        Destroy(editor.gameObject);
    }

    public void Execute(Vector2Int clickPos)
    {
        // bottom left
        Vector2Int rootPos = clickPos + new Vector2Int(-1, -1);
        tileAttacks.Iterate2D((tAtk, x, y) =>
        {
            if (tAtk.IsNullAttack()) return;
            Vector2Int pos = rootPos + new Vector2Int(x, y);
            if (!GameGrid.IsValidPos(pos)) return;

            Unit unit = GameGrid.Instance.GetUnit(pos);
            if (unit == null) return;

            unit.Damage(tAtk.damage, tAtk.damageType);
            if (tAtk.knockbackDirection != Directions.None)
            {
                unit.Knockback(tAtk.knockbackDirection);
            }
            
        }, true);
        // iterates top left -> bottom right (shouldn't matter in most cases)


    }

}
