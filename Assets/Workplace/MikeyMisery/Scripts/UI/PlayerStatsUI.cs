using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private Image staminaFill;
    [SerializeField] private Image manaFill;

    [SerializeField] private StaminaController staminaController;

    private float maxMana = 100f;
    private float currentMana = 50f;

    private void OnEnable() {
        HealthSystem.OnHealthChangedUI += UpdateHealthBar;
    }

    private void Start() {

        staminaController = GameObject.FindGameObjectWithTag("Player").GetComponent<StaminaController>();
        

        staminaFill.fillAmount = staminaController.Ratio * 0.5f; // Start with half stamina
        manaFill.fillAmount = (currentMana / maxMana) * 0.5f; // Start with half mana
    }

    private void Update()
    {
        
        staminaFill.fillAmount = staminaController.Ratio * 0.5f; // Update stamina fill amount
    }
    private void UpdateHealthBar(float currentHealth, float maxHealth) {

        healthFill.fillAmount = currentHealth / maxHealth;
    }
}