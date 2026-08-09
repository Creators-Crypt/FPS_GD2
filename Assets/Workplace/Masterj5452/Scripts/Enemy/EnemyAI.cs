using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Data from Scriptiabl object")]
    [SerializeField] public EnemyStatsSO stats;


    [SerializeField] private Transform firePoint;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Transform playerTarget;
    public Vector3 spawnPostion;
    public float currentHealth;
    public float timeSinceLastSawPlayer;
    public float lastAttackTime;

    public EnemyStateMachine stateMachine;
    public EnemyIdleState idleState;
    public
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanSeePlayer()
    {
        if(playerTarget == null) return false;

        Vector3 playerDist = playerTarget.position - firePoint.position;
        float distance = playerDist.magnitude;

        if(distance > stats.detectionRadius) return false;

        float angle = Vector3.Angle(transform.forward, playerDist.normalized);
        if(angle > stats.detectionAngle * .5f) return false;
        if(Physics.Raycast(firePoint.position, playerDist.normalized,distance,stats.obstacleMask)) return false;

        timeSinceLastSawPlayer = 0f;
        return true;
    }
}
