using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Image staminaFill;
    [SerializeField] private Image manaFill;

    [SerializeField] private StaminaController staminaController;

    private float maxMana = 100f;
    private float currentMana = 50f;

    private void Start() {
        staminaFill.fillAmount = staminaController.Ratio * 0.5f; // Start with half stamina
        manaFill.fillAmount = (currentMana / maxMana) * 0.5f; // Start with half mana
    }

    private void Update()
    {
        healthFill.fillAmount = healthSystem.CurrentHealth / healthSystem.MaxHealth;
        staminaFill.fillAmount = staminaController.Ratio * 0.5f; // Update stamina fill amount
    }
}