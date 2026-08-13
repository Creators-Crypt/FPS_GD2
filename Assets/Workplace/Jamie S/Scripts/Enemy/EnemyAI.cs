using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static EnemyStatsSO;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("Data from Scriptiabl object")]
    [SerializeField] public EnemyStatsSO stats;
    

    [SerializeField] private Transform firePoint;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Transform playerTarget;
    [SerializeField] public Renderer model;

    public Vector3 spawnPostion;   
    public float currentHealth;
    public float faceTargetRotSpeed = 10;
    public float timeSinceLastSawPlayer;
    public float lastAttackTime;
    public Color origColor;


    public EnemyStateMachine stateMachine;
    public EnemyIdleState idleState;
    public EnemyPatrolState patrolState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj != null )
        {
            playerTarget = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Be sure the player is TAGED AS PLAYER");
        }

        origColor = model.material.color;

        if (firePoint == null) firePoint = transform;
        spawnPostion = transform.position;
        currentHealth = stats.maxHealth;

        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this);
        patrolState = new EnemyPatrolState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateMachine.Initialize(patrolState);

    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Tick();
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

    public void AddSightLossTime()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
    }

    public bool IsPlayerInAttackRange()
    {
        if(playerTarget == null) return false;

        return Vector3.Distance(transform.position, playerTarget.position) <= stats.attackRange;
    }

    public void FacePlayer()
    {
        if(playerTarget ==null) return;

        Vector3 dir = playerTarget.position - transform.position;

        Quaternion targetRot = Quaternion.LookRotation(dir); 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, faceTargetRotSpeed * Time.deltaTime);
    }

    public bool CanAttack()
    {
        return Time.time - lastAttackTime >= stats.attackCooldown;
    }

    public void PreformAttack()
    {
        lastAttackTime = Time.time;

        switch(stats.enemyType)
        {
            case EnemyType.Melee:
                PerformMeleeAttack();
                break;
            case EnemyType.Ranged:
                PreformRangedAttack();
                break;
            case EnemyType.Bomber:
                PreforeBomberAttack();
                break;
            case EnemyType.Boss:
                break;
        }
    }

    private void PreformRangedAttack()
    {
        if(playerTarget == null || stats.projectilePrefab == null) return;

        Vector3 dir = playerTarget.position - firePoint.position;
        dir.Normalize();

        GameObject projectileObj = Instantiate(stats.projectilePrefab,firePoint.position, Quaternion.LookRotation(dir));

        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if(projectile != null)
        {
            projectile.Fire(dir, stats.projectileSpeed, stats.attackDamage);
        }
        else
        {
            Debug.LogWarning("PLEASE CHECK THE PROJECTILE PREFAB IN THE ENEMY SCRIPTED OBJECT IS NOT EMPTY");
        }
       
    }
    private void PreforeBomberAttack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, stats.explosionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.OnDamage(stats.attackDamage);
                }
            }


        }

        Die();

    }
    private void PerformMeleeAttack()
    {
        if (playerTarget == null) return;
        IDamageable damageable = playerTarget.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.OnDamage(stats.attackDamage);
        }
    }

    public void OnDamage(float amount)
    {

        currentHealth -= amount;
        StartCoroutine(FlashRed());
        if(stats.projectilePrefab != null) {Destroy(stats.projectilePrefab, .01f); }
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = origColor;
    }
    public void Die()
    {
        Destroy(gameObject, .01f);
    }

}
