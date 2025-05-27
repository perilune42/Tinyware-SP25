using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;
    public Attack SelectedAttack;

    public Action<Attack> OnAttackChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnAttackChanged?.Invoke(null);
    }
    public void SelectAttack(Attack attack)
    {
        SelectedAttack = attack;
        OnAttackChanged?.Invoke(attack);
    }

    public void ClickTile(Vector2Int pos)
    {
        if (GameGrid.Instance.MovingUnits || SelectedAttack == null) return;
        SelectedAttack.Execute(pos);
        OverlayManager.Instance.ClearOverlay();
        SelectAttack(null);
    }

    public void HoverTile(Vector2Int pos)
    {
        if (GameGrid.IsValidPos(pos))
        {
            Unit unit = GameGrid.Instance.GetUnit(pos);
            UnitInfoUI.Instance.ViewUnitInfo(unit);
            if (unit == null)
            {
                TerrainInfoUI.Instance.ShowTile(GameGrid.Instance.GetTile(pos));
            }
            else
            {
                TerrainInfoUI.Instance.Hide();
            }
        }
        else
        {
            TerrainInfoUI.Instance.ShowBoundaryTile(GameGrid.Instance.GetBoundaryTile(pos));
        }
        if (SelectedAttack == null) return;
        OverlayManager.Instance.UpdateOverlay(SelectedAttack, pos);
    }

    public void CancelAttack()
    {
        SelectedAttack = null;
        OverlayManager.Instance.ClearOverlay();
        OnAttackChanged?.Invoke(null);
    }
}