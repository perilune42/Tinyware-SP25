using TMPro;
using UnityEngine;

public class AttackEditorCell : MonoBehaviour
{
    [SerializeField] TMP_Text damageNumber, damageType;
    public TileAttack TileAttack;

    private void OnValidate()
    {
        if (TileAttack.damage == 0)
        {
            damageType.text = "";
            Vector2Int dir = TileAttack.knockbackDirection;
            if (dir == Directions.None)
            {
                damageNumber.text = "";
            }
            else if (dir == Directions.Up)
            {
                damageNumber.text = "↑";
            }
            else if (dir == Directions.Down)
            {
                damageNumber.text = "↓";
            }
            else if (dir == Directions.Left)
            {
                damageNumber.text = "←";
            }
            else if (dir == Directions.Right)
            {
                damageNumber.text = "→";
            }
            else
            {
                damageNumber.text = "ERR";
            }

        }
        else
        {
            damageNumber.text = TileAttack.damage.ToString();
            damageType.text = TileAttack.damageType.ToString();
        }

    }

}
