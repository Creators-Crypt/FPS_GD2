using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class BossPhase1State : IEnemyState
{
    private BossAI boss;
    private BossStatsSO stats;
    private Coroutine attackRoutine;

    public BossPhase1State(BossAI _boss)
    {
        boss = _boss;
    }

    public void Enter()
    {
        stats = boss.bossStats;

        boss.isInvulnerable = false;
        boss.SetMovementEnabled(false);
        boss.SetPhaseColor(stats.phase1Material);

        attackRoutine = boss.StartCoroutine(AttackLoop());
    }

    public void Tick()
    {
        if(boss.playerTarget != null)
        {
            boss.FacePlayer();
        }
    }
    public void Exit()
    {
        if(attackRoutine != null)
        {
            boss.StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
        boss.SetMovementEnabled(true);
    }

    private IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(stats.p1StartDelay);

        float nextMortarTime = Time.time + stats.mortarCooldown;

        while (true)
        {
            if (boss.playerTarget == null)
            {
                yield return null;
                continue;
            }

            if (Time.time >= nextMortarTime)
            {
                yield return MortarAttack();
                nextMortarTime = Time.time * stats.mortarCooldown;
            }
            else
            {
                yield return NormalAttack();
                yield return new WaitForSeconds(stats.p1TimeBetweenVolley);
            }
        }

    }

    private IEnumerator NormalAttack()
    {
        if (stats.projectilePrefab == null)
        {
            Debug.LogWarning("Please put a projectile prefab on the bossSO");
            yield return new WaitForSeconds(.2f);
        }
        else
        {
            Transform muzzle = boss.firePoint;
            if (muzzle == null) muzzle = boss.transform;

            for (int i = 0; i < stats.p1ShotsPerVolley; i++)
            {
                if (boss.playerTarget == null) break;

                Vector3 aimPoint = boss.playerTarget.position;

                float travelTime = Vector3.Distance(muzzle.position, aimPoint) / stats.p1ProjectileSpeed;
                aimPoint = aimPoint + boss.playerVelocity * (travelTime * stats.bulletAimAheadOfPlayer);

                Vector3 dir = (aimPoint - muzzle.position).normalized;
                dir = AddSpread(dir,stats.p1SpreadDegrees);

                GameObject shot = Object.Instantiate(stats.projectilePrefab, muzzle.position, Quaternion.LookRotation(dir));
                Projectile bullet = shot.GetComponent<Projectile>();

                if(bullet != null)
                {
                    bullet.Fire(dir, stats.p1ProjectileSpeed, stats.p1ProjectileDamage);
                }

                boss.lastAttackTime = Time.time;

                yield return new WaitForSeconds(stats.p1TimeBetweenShots);


            }
        }
    }

    private Vector3 AddSpread(Vector3 _dir, float _degrees)
    {
        if (_degrees <= 0) return _dir;

        float randomX = Random.Range(-_degrees, _degrees);
        float randomY = Random.Range(-_degrees, _degrees);

        return Quaternion.Euler(randomX, randomY,0)*_dir;
    }

    private IEnumerator MortarAttack()
    {
        Transform muzzle = boss.mortarFirePoint;
        if (muzzle == null) muzzle = boss.transform;

        for(int i = 0;i<stats.mortarShellsPreSalvo; i++)
        {
            if (boss.playerTarget == null) break;

            Vector3 impactPoint = PickImpactPoint(i);

            Vector3 horizontalOffset = impactPoint - muzzle.position;
            horizontalOffset.y = 0;
            float flightTime = horizontalOffset.magnitude / stats.mortarSpeed;

            SpawnTelegraph(impactPoint, flightTime);

            GameObject mortarShellObj = Object.Instantiate(stats.mortarPrefab,muzzle.position,Quaternion.identity);

            BossMortarProjectile mortarShell = mortarShellObj.GetComponent<BossMortarProjectile>();
            if(mortarShell != null)
            {
                LayerMask splashHits = boss.GetAttackMask(boss.mortarFriendlyFire);

                mortarShell.Launch(impactPoint, stats.mortarSpeed, stats.mortarArcHeight, stats.mortarDamage, stats.mortarSplashRadius, boss.groundMask, splashHits);
            }
            else
            {
                Debug.LogWarning("Check the mortarPrefab anb make sure it has the BossMortarProjectile script :)");
                Object.Destroy(mortarShellObj);
            }
            boss.lastAttackTime = Time.time;

            yield return new WaitForSeconds(stats.mortarTimeBetweenShells);
        }
    }

    private Vector3 PickImpactPoint(int _shellNumber)
    {
        Vector3 aimPoint = boss.playerTarget.position + boss.playerVelocity * stats.mortarAimAheadOfPlayer;

        if(_shellNumber > 0)
        {
            Vector2 randomCircle = Random.insideUnitCircle * stats.mortarScatter;
            aimPoint = aimPoint + new Vector3(randomCircle.x, 0f, randomCircle.y);
        }

        Vector3 groundPoint = GetGroundPoint(aimPoint);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(groundPoint, out hit, 3f, NavMesh.AllAreas))
        {
            groundPoint = hit.position
        }
        return groundPoint;
    }

    private Vector3 GetGroundPoint(Vector3 _point)
    {
        Vector3 start = _point + Vector3.up * 5f;

        RaycastHit hit;
        if(Physics.Raycast(start, Vector3.down, out hit, 35f, boss.groundMask, QueryTriggerInteraction.Ignore))
        {
            return hit.point;
        }
        return _point;
    }

    private void SpawnTelegraph(Vector3 _impactPoint, float _flightTime)
    {
        if (stats.mortarHitPosDisplayPrefab == null) return;

        Vector3 spawnPoint = _impactPoint + Vector3.up * .05f;

        GameObject marker = Object.Instantiate(stats.mortarHitPosDisplayPrefab, spawnPoint, Quaternion.Euler(90f,0f,0f));

        BossTelegraph telegraph = marker.GetComponent<BossTelegraph>();
        if(telegraph != null)
        {
            telegraph.Play(stats.mortarSplashRadius,_flightTime);
        }
        else
        {
            Object.Destroy(marker, _flightTime +.5f);
        }
    }
}

public class BossTransitionState : IEnemyState
{
    private BossAI boss;
    private BossStatsSO stats;
    private Coroutine routine;

    public BossPhase targetPhase = BossPhase.Phase2;

    public BossTransitionState(BossAI _boss)
    {
        boss = _boss;
    }

    public void Enter()
    {
        stats = boss.bossStats;

        boss.isInvulnerable = true;
        boss.SetMovementEnabled(false);

        Material nextMaterial = stats.phase2Material;

        if(targetPhase == BossPhase.Phase3 )
        {
            nextMaterial = stats.phase3Material;
        }

        boss.SetPhaseColor(nextMaterial);

        if(targetPhase == BossPhase.Phase2 )
        {
            routine = boss.StartCoroutine(LeapDownToPlayer());
        }

    }
}
    
