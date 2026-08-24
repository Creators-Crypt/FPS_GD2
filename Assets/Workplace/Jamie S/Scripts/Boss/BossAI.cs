using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public enum BossPhase
{
    Phase1,
    Transition,
    Phase2,
    Stunned,
    Phase3,
    Dead
}


public class BossAI : EnemyAI
{
    public Transform mortarFirePoint;

    [Header("Layers")]
    public LayerMask groundMask =~0;
    public LayerMask damageableMask =~0; //Player Layer
    public LayerMask otherEnemyMask =0;

    [Header("Friendly Fire - hurt other slimes")]
    public bool mortarFriendlyFire = true;
    public bool landingShockFriendlyFire = false;
    public bool meleeFriendlyFire = false;
    public bool aoeWaveFriendlyFire = false;
    public bool detonationFriendlyFire = true;

    [Header("Info/Stats")]
    public BossStatsSO bossStats;
    public BossPhase currentPhase = BossPhase.Phase1;
    public bool isInvulnerable = false;
    public bool isStunned = false;
    public float stunTimeleft = 0f;
    public float detonationTimeLeft = 0f;
    public int eyeHitCount = 0;

    public BossPhase1State phase1State;
    public BossPhase2State phase2State;
    public BossPhase3State phase3State;
    public BossStunState stunState;
    public BossTransitionState transitionState;



    public Vector3 playerVelocity;

    private Vector3 lastPlayerPostion;

    private Color currentColor;
    private Color targetColor;
   [SerializeField] private float colorBlendSpeed;
    private bool isBlendingColor = false;

    private float eyeHitExpireTime = 0f;
    private float stunLockout = 0f;


    public override void Awake()
    {
        if (bossStats == null)
        {
            Debug.LogError("The boss needs his Scripted Object dropped into the field");
            enabled = false;
            return;
        }

        stats = bossStats;

        base.Awake();

        if (mortarFirePoint == null) mortarFirePoint = firePoint;

        SetPhaseColor(bossStats.phase1Material);
        phase1State = new BossPhase1State(this);
        phase2State = new BossPhase2State(this);
        phase3State = new BossPhase3State(this);
        

        
    }


    public void SetPhaseColor(Material _phaseMaterial)
    {
       if(_phaseMaterial  == null)
        {
            Debug.LogWarning("Check the Phase Material Slots and make sure they are filled");
            return;
        }

       model.material = _phaseMaterial;

        currentColor = model.material.color;
    }
    public void SetMovementEnabled(bool _canMove)
    {
        if (agent == null) return;
        if (agent.enabled == false) return;
        if (agent.isOnNavMesh == false) return;

        agent.isStopped = !_canMove;

        if (_canMove == false)
        {
            agent.velocity = Vector3.zero;
            agent.ResetPath(); 
        }

    }
    public Color GetCurrentColor() { return currentColor; }
    public LayerMask GetAttackMask(bool _friendlyFire)
    {
        int mask = damageableMask;
        if (_friendlyFire)
        {
            mask = mask | otherEnemyMask;
        }
        return mask;
    }
}

