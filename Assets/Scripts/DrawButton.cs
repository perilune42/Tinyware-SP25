using UnityEngine;

public class DrawButton : MonoBehaviour
{
    public void Draw()
    {
        AttackManager.Instance.DrawNewAttack();
    }
}