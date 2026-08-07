using UnityEngine;

/// <summary>
/// 
/// Manages the health bar shader effect based on the health of a target GameObject.
/// 
/// </summary>
/// 
/// <remarks>
/// 
/// Updates the shader property to reflect the current health percentage, smoothing the
/// transition.
/// 
/// </remarks>
public class HealthBarShader : MonoBehaviour {

    [Header("Reference")]
    [SerializeField] private GameObject target;

    [Header("Material Settings")]
    [SerializeField] private Renderer renderer;
    [SerializeField] private string shaderPropertyName = "_HealthPercent";
    [SerializeField] private float smoothSpeed = 5f;

    private IHealth healthSystem;
    private MaterialPropertyBlock propertyBlock;
    private int propertyID;
    private float targetPercent = 1f;
    private float currentPercent = 1f;

    private void Awake() {
        
        propertyID = Shader.PropertyToID(shaderPropertyName);
        propertyBlock = new MaterialPropertyBlock();

        if (target.TryGetComponent<IHealth>(out var health)) healthSystem = health;
    }
    private void OnEnable() {
        healthSystem.OnHealthChanged += UpdateHealthBar;

        targetPercent = healthSystem.CurrentHealth / healthSystem.MaxHealth;
        currentPercent = targetPercent;
        ApplyShaderChange(currentPercent);
    }


    private void OnDisable() {
        if (healthSystem != null) healthSystem.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth) {
        targetPercent = currentHealth / maxHealth;
    }
    private void Update() {

        if (renderer != null) return;

        currentPercent = Mathf.MoveTowards(currentPercent, targetPercent, smoothSpeed);
        ApplyShaderChange(currentPercent);
    }
    private void ApplyShaderChange(float value) {

        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(propertyID, value);
        renderer.SetPropertyBlock(propertyBlock);
    }
}