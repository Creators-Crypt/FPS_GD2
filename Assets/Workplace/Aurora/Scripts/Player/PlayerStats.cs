using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Player Stats")]
public class PlayerStats : ScriptableObject {

    [Header("Player Movement")]
    [Tooltip("The maximum speed the player can reach while moving.")]
    public float walkSpeed = 6f;
    [Tooltip("The maximum speed the player can reach while sprinting.")]
    public float sprintSpeed = 10f;
    [Tooltip("Multiplier applied when dodging (speed * multiplier).")]
    public float dodgeSpeedMultiplier = 1.5f;
    [Tooltip("The amount of time the player can sprint before needing to rest.")]
    public float dodgeDuration = 0.5f;
    [Tooltip("The amount of time the player must wait before using the dodge ability again.")]
    public float dodgeCooldown = 1f;

    [Header("Grounded Stats")]
    public float slopeCheckDistance = 0.5f;
    public float maxSlopeAngle = 45f;

    public LayerMask enemyLayer;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaRegenRate = 18f;
    public float dodgeStaminaCost = 20f;
    public float sprintStaminaCost = 12f;
    public float sprintStaminaRegen = 4f;
}