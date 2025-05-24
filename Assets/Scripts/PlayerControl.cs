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

    public void SelectAttack(Attack attack)
    {
        SelectedAttack = attack;
        OnAttackChanged?.Invoke(attack);
    }

    public void ClickTile(Vector2Int pos)
    {
        if (SelectedAttack == null) return;
        SelectedAttack.Execute(pos);
    }

    public void HoverTile(Vector2Int pos)
    {
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