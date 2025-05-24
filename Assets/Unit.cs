using UnityEngine;

public class Unit : MonoBehaviour
{

    public Vector2Int Pos;
    public int currHealth;
    public int maxHealth;

    [SerializeField] HealthBar healthBar;


    public void SetPos(Vector2Int pos)
    {
        Pos = pos;
    }

    private void Awake()
    {
        currHealth = maxHealth;
        healthBar.ShowHealth(currHealth, maxHealth);
    }

    public void Damage(int damage, DamageType dType)
    {
        currHealth -= damage;
        healthBar.ShowHealth(currHealth, maxHealth);
    }

}
