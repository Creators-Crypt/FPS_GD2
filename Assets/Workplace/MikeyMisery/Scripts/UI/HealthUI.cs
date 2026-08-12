using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;

    private float maxHealth = 100f;
    private float currentHealth = 25f;

    private void Start() {
        UpdateHealthUI();
    }

    private void UpdateHealthUI() {
        healthFill.fillAmount = currentHealth / maxHealth;
    }
}