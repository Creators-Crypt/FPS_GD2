using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IHealth, IDamageable {

    public event Action OnDeath;
    public event Action<float, float> OnHealthChanged;

    [Header("Health Data")]
    [SerializeField] private PlayerStats stats;

    private readonly float maxHP = 100f;

    private IInvulnerable invulnerable;

    public float CurrentHealth {  get; private set; }
    public float MaxHealth => stats != null ? stats.MaxHealth : maxHP;
    public bool IsDead { get; private set; }

    private void Awake() {

        invulnerable = GetComponent<IInvulnerable>();
        InitializeHealth();
    }
    private void InitializeHealth() {

        CurrentHealth = MaxHealth;
        IsDead = false;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
    public void HealMax() {
        CurrentHealth = MaxHealth;
    }
    public IEnumerator HealOverTime(float duration) {
        throw new NotImplementedException();
    }
    public void OnHeal(float healAmount) {

        if (IsDead || healAmount <= 0) return;

        CurrentHealth = Mathf.Max(MaxHealth, CurrentHealth + healAmount);

        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
    public void OnDamage(float damage) {
        
        if (IsDead || damage <= 0) return;

        if (invulnerable != null && invulnerable.IsInvulnerable) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0) Death();
    }
    private void Death() {

        IsDead = true;
        OnDeath?.Invoke();
    }
}