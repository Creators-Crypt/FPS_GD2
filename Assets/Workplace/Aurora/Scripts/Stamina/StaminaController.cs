using UnityEngine;

public class StaminaController : MonoBehaviour, IStamina {
    
    [SerializeField] private PlayerStats stats;

    public float Current { get; private set; }
    public float Ratio => stats ? Current / stats.maxStamina : 0f;
    public bool TrySpend(float cost) {
        
        if (stats == null) return false;
        
        if (Current >= cost) {
            Current -= cost;
            return true;
        }
        return false;
    }
    private void Update() {
        
        if (stats == null) return;
        
        Current = Mathf.Clamp(Current + stats.staminaRegenRate * Time.deltaTime, 0f, stats.maxStamina);
    }

    /// <summary>
    /// 
    /// Alternative regen for sprint (slower, can be called from a locomotion script).
    /// 
    /// </summary>
    public void Regen(float amount) {
        
        if (stats == null) return;
        
        Current = Mathf.Clamp(Current + amount * Time.deltaTime, 0f, stats.maxStamina);
    }
}