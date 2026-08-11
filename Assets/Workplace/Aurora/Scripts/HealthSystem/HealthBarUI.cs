using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Represents a UI component that displays and updates the player's health bar based on changes in health.
/// 
/// </summary>
/// <remarks>
/// Requires a GameObject with an IHealth component assigned to function correctly.
/// </remarks>
public class HealthBarUI : MonoBehaviour {

    [Header("Target & Components")]
    [Tooltip("Health target is the Player.")]
    [SerializeField] private GameObject healthTarget;
    [SerializeField] private Slider healthSlider;

    private IHealth healthSystem;

    private void Awake() {
        
        if (healthTarget != null) {
            Debug.LogError($"Health Target is missing on {gameObject.name} UI!", this);
            return;
        }

        if (healthTarget.TryGetComponent<IHealth>(out var health)) healthSystem = health;
    }
    private void OnEnable() {

        if (healthSystem == null) return;

        healthSystem.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar(healthSystem.CurrentHealth, healthSystem.MaxHealth);
    }
    private void OnDisable() {

        if (healthSystem == null) return;

        healthSystem.OnHealthChanged -= UpdateHealthBar;
    }
    private void UpdateHealthBar(float currentHealth, float maxHealth) {
        
        if (healthSlider == null) return;

        float calculatedHealth = currentHealth / maxHealth;
        healthSlider.value = calculatedHealth;
    }
}