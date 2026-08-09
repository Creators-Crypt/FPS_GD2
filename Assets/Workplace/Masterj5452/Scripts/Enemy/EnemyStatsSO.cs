using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "Scriptable Objects/EnemyStatsSO")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Health")]
    [SerializeField] float maxHealth = 10f;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float patrolRadius = 8f;
    public float patrolWaitTimeMin = 1.5f;
    public float patrolWaitTimeMax = 3.5f;

    [Header("Detection")]
    public float detectionRadius = 10f;
    [Range(1,360)]public float detectionAngle = 90f;
    public float lostSightTime = 3f;
    public LayerMask obstacleMask;

    [Header("Attack")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1.2f;
    public float attackDamage = 5f;    
    
}
