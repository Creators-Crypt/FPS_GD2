using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Image staminaFill;
    [SerializeField] private Image manaFill;

    private float maxStamina = 100f;
    private float currentStamina = 50f;

    private float maxMana = 100f;
    private float currentMana = 50f;

    private void Start() {
        staminaFill.fillAmount = (currentStamina / maxStamina) * 0.5f; // Start with half stamina
        manaFill.fillAmount = (currentMana / maxMana) * 0.5f; // Start with half mana
    }
}