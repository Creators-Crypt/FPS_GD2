using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : IEnemyState
{
    public EnemyAI enemy;
   [SerializeField] private float idleTimer;
   [SerializeField]  private float idleDuration;

    public EnemyIdleState(EnemyAI _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        enemy.agent.isStopped = true;
        idleDuration = Random.Range(enemy.stats.patrolWaitTimeMin, enemy.stats.patrolWaitTimeMax);
        idleTimer = 0f;


    }
    public void Tick()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.stateMachine.ChangeState(enemy.ChaseState);
            return;
        }
        idleTimer = Time.deltaTime;
        if(idleTimer >= idleDuration) { enemy.stateMachine.ChangeState(enemy.PatrolState);
    }


    public void Exit()
    {
        enemy.agent.isStopped = false;
    }


}

public class EnemyPatrolState: IEnemyState
{
    private EnemyAI enemy;
    public EnemyPatrolState(EnemyAI _enemy) {  enemy = _enemy; }

    public void Enter()
    {
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.stats.patrolSpeed;
        SetNewPatrolDestineation();
    }
   
    public void Tick()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.stateMachine.ChangeState(enemy.ChaseState);
            return;

            if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
            }
        }
    }
    private void SetNewPatrolDestineation()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomPoint = enemy.spawnPostion + Random.insideUnitSphere * enemy.stats.patrolRadius;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, enemy.stats.patrolRadius, NavMesh.AllAreas))
            {
                enemy.agent.SetDestination(hit.position);
                return;
            }
        }
        enemy.agent.SetDestination(enemy.transform.position);
    }


    public void Exit()
    {

    }

}

public class EnemyChaseState : IEnemyState
{
    private EnemyAI enemy;

    public EnemyChaseState(EnemyAI _enemy)
    {
        enemy = _enemy;
    }
    
    public void Enter()
    {
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.stats.chaseSpeed;
    }
    public void Tick()
    {
        if(enemy.playerTarget == null)
        {
            enemy.stateMachine.ChangeState(enemy.PatrolState);
            return;
        }

        bool canSee = enemy.CanSeePlayer();

        if (!canSee)
        {
            enemy.addsitt
        }
    }






}
