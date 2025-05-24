using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public TMP_Text healthText;

    public void ShowHealth(int curr, int max)
    {
        healthText.SetText($"{curr}/{max}");
    }
}
