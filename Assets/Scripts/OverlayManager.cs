using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager Instance;
    private Vector2Int prevHovered = new(-1,-1);

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateOverlay(Attack atk, Vector2Int centerPos)
    {
        var prevHoveredRoot = prevHovered + new Vector2Int(-1, -1);

        Util.StdPos3x3.Iterate2D((relPos, x, y) =>
        {
            Vector2Int pos = prevHoveredRoot + new Vector2Int(x, y);
            if (!GameGrid.IsValidPos(pos) && !GameGrid.IsOnBoundary(pos)) return;
            SetTileOverlay(pos, null);
        });


        // bottom left
        Vector2Int rootPos = centerPos + new Vector2Int(-1, -1);
        atk.tileAttacks.Iterate2D((tAtk, x, y) =>
        {
            Vector2Int pos = rootPos + new Vector2Int(x, y);
            if (!GameGrid.IsValidPos(pos) && !GameGrid.IsOnBoundary(pos)) return;
            SetTileOverlay(pos, tAtk);
        });

        prevHovered = centerPos;
    }

    // includes overlaying damage previews on units
    public void SetTileOverlay(Vector2Int pos, TileAttack tAtk)
    {
        if (GameGrid.IsValidPos(pos))
        {
            Unit unit = GameGrid.Instance.GetUnit(pos);
            if (unit != null)
            {
                var previewInfo = unit.SimulateAttack(tAtk);
                unit.PreviewHealth(previewInfo);
                GameGrid.Instance.GetTile(pos).ShowUnitAttackOverlay(previewInfo);
            }
            else
            {
                GameGrid.Instance.GetTile(pos).ShowAttackOverlay(tAtk);
            }
        }
        else if (GameGrid.IsOnBoundary(pos))
        {
            GameGrid.Instance.GetBoundaryTile(pos).ShowAttackOverlay(tAtk);
        }
        else
        {
            Debug.LogError("Tried setting overlay on invalid tile");
        }
    }

    public void ClearOverlay()
    {
        var prevHoveredRoot = prevHovered + new Vector2Int(-1, -1);
        Util.StdPos3x3.Iterate2D((rPos, x, y) =>
        {
            Vector2Int pos = prevHoveredRoot + rPos;
            if (!GameGrid.IsValidPos(pos) && !GameGrid.IsOnBoundary(pos)) return;
            SetTileOverlay(pos, null);
        });
    }
}