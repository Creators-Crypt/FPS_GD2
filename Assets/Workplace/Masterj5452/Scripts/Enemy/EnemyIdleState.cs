using UnityEditor;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private SlimeEnemy enemy;
    private float idleTimer;
    pirvate float idleDuration;

    public EnemyIdleState(SlimeEnemy _enemy)
    {
        enemy = _enemy;
    }

    void public Enter()
    {
       
    }
}
