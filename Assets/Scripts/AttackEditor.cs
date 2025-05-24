using UnityEngine;

public class AttackEditor : MonoBehaviour
{
    public TileAttack[,] GetTileAttacks()
    {
        var cells = GetComponentsInChildren<AttackEditorCell>();
        var tileAttacks = new TileAttack[Attack.attackWidth, Attack.attackHeight];
        for (int y = 0; y <  Attack.attackHeight; y++)
        {
            for (int x = 0; x < Attack.attackWidth; x++)
            {
                tileAttacks[x, y] = cells[y * Attack.attackWidth + x].TileAttack;
            }
        }
        return tileAttacks;
    }
    

}
