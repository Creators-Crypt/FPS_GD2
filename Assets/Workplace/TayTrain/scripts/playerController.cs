using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour , IInvulnerable
{
    [Header("References")]
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerStats stats;
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] StaminaController staminaController;
    [SerializeField] ConcentrationController concentrationController;
    [SerializeField] SpellCaster spellCaster;
    [SerializeField] DeathHandler deathHandler;
    [SerializeField] private GameObject concentrationLight;

    [Header("Player Visuals")]
    [SerializeField] private TrailRenderer teleportTrail;
    


    [Header("Jump")]
    [Range(1, 30)][SerializeField] int jumpSpeed = 5;
    [Range(1, 10)][SerializeField] int jumpMax = 1;
    [SerializeField, Range(1, 10)] int gravity = 10;


    [Header("Animation")]
    [SerializeField] private Transform armPivot;
    [SerializeField] private float armRotateSpeed = 10f;

    [Header("Player State")]
    [SerializeField] private PlayerState currentState;

    [Header("Teleport")]
    [Range(0.05f, 10f)][SerializeField] float teleportTrailTime = 0.2f;
    [Range(0.1f, 3f)][SerializeField] float teleportCooldown = 1.0f;
    [Range(1f, 100f)][SerializeField] float teleportDistance = 100f;

    //Jumps
    int jumpCount;

    //Movement
    Vector3 moveDir;
    Vector3 playerVel;

    //speed
    [SerializeField] float currentSpeed;  
    [SerializeField] private bool isPlayerSprinting = false;
    [SerializeField] float staminaTimer;
    [SerializeField] float stamina;

    //Dodge
    bool isDodging;
    float dodgeTimer;
    float dodgeCooldownTimer;
    Vector3 dodgeDirection;

    //Bool for dodge and equpment
    public bool IsInvulnerable
    {
        get {  return isDodging; }
    }


    //Teleport
    bool isTeleporting;
    float teleportCooldownTimer;

    //Concentration
    bool isConcentrating;
    float concentrationTimer;

    //Player size
    Vector3 originalScale;

    //LayerMask
    [SerializeField] LayerMask ignoreLayer;

    // Animation for arm
    //Quaternion armBaseRotation;

    // PlayerState
    public enum PlayerState {
        Idle,
        Walk,
        Sprint,
        Jump,
        Dodge,
        Teleport,
        Concentrate,
        Dead
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        controller = GetComponent<CharacterController>();

        healthSystem = GetComponent<HealthSystem>();

        staminaController = GetComponent<StaminaController>();

        concentrationController = GetComponent<ConcentrationController>();

        currentSpeed = stats.walkSpeed;

        deathHandler = GetComponent<DeathHandler>();

        spellCaster = GetComponent<SpellCaster>();

        originalScale = transform.localScale;

        if(teleportTrail != null)
        {
            teleportTrail.emitting = false;
        }

    }
    // Update is called once per frame
    void Update() {

        stamina = staminaController.Current;
        isPlayerSprinting = !isConcentrating && 
            Input.GetKey(KeyCode.LeftShift) &&
            (staminaController.Current > stats.sprintStaminaCost);

        staminaController.IsConsuming = isPlayerSprinting;
        currentSpeed = (isPlayerSprinting) ? stats.sprintSpeed : stats.walkSpeed;
        if (isPlayerSprinting) {
            staminaController.ContinousSpent(stats.sprintStaminaCost);
        }

        if (dodgeCooldownTimer > 0) {
            dodgeCooldownTimer -= Time.deltaTime;
        }

        if (teleportCooldownTimer > 0)
        {
            teleportCooldownTimer -= Time.deltaTime;
        }
        moveDir = Input.GetAxis("Horizontal") * transform.right +
            Input.GetAxis("Vertical") * transform.forward;

        teleport();
        dodge();
        concentrate();
        movement();
        updateState();
        //updateAnimator();
    }
    private void LateUpdate() {
        rotateArm();
    }

    void movement() {
        if (controller.isGrounded && playerVel.y < 0) {
            jumpCount = 0;
            playerVel.y = -2f;
        }

        if (!isTeleporting && !isDodging && !isConcentrating) { 
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

        if(!isConcentrating)
        {
            jump();
        }
       

        controller.Move(playerVel * Time.deltaTime);

        playerVel.y -= gravity * Time.deltaTime;

    }

    void dodge() {


        if (Input.GetButtonDown("Dodge") && dodgeCooldownTimer <= 0 && staminaController.TrySpend(stats.dodgeStaminaCost)) {
            isDodging = true;

            dodgeTimer = stats.dodgeDuration;
            dodgeCooldownTimer = stats.dodgeCooldown;

            dodgeDirection = moveDir.normalized;

            if (dodgeDirection == Vector3.zero) {
                dodgeDirection = transform.forward;
            }
        }

        if (isDodging) {

            transform.localScale = Vector3.Lerp(transform.localScale,
                originalScale * 0.1f, 15f * Time.deltaTime);

            controller.Move(dodgeDirection *
                stats.walkSpeed *
                stats.dodgeSpeedMultiplier *
                Time.deltaTime);

            dodgeTimer -= Time.deltaTime;

            if (dodgeTimer <= 0)
            {
                isDodging = false;
            }
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale,
                originalScale, 15f * Time.deltaTime);
        }
    }
     // only needs a cooldown and will work off of focus
    void teleport ()
    {
        if (Input.GetButtonDown("Teleport") && teleportCooldownTimer <= 0 && !isTeleporting)
        {
            isTeleporting = true;

            teleportCooldownTimer = teleportCooldown;

            RaycastHit hit;

            Vector3 teleportDirection = Camera.main.transform.forward;
            Vector3 teleportPoint;

            if (Physics.Raycast(Camera.main.transform.position,
                teleportDirection, out hit, teleportDistance, ~ignoreLayer))
            {
                teleportPoint = hit.point - teleportDirection * 1.5f;
            }

            else
            {
                teleportPoint = transform.position + teleportDirection * teleportDistance;
            }

            RaycastHit groundHit;

            Vector3 groundCheck = teleportPoint + Vector3.up * 10f;

            if(Physics.Raycast(groundCheck, Vector3.down, out groundHit, 20f, ~ignoreLayer))
            {
                float controllerBottom = controller.center.y - (controller.height / 2f);

                float groundOffset = -controllerBottom;
                if(teleportPoint.y < groundHit.point.y + groundOffset)
                {
                    teleportPoint.y = groundHit.point.y + groundOffset;
                }
            }
           
            if(teleportTrail != null)
            {
                teleportTrail.Clear();
                teleportTrail.emitting = true;
            }

            controller.enabled = false;
            transform.position = teleportPoint;
            controller.enabled = true;

            //playerVel.y = 0f;

            if (teleportTrail != null)
            {
                StartCoroutine(stopTeleportTrail());
            }

            
        } 
    }
    IEnumerator stopTeleportTrail()
    {
        yield return new WaitForSeconds(teleportTrailTime);

        if(teleportTrail!= null)
        {
            teleportTrail.emitting = false;
        }
        isTeleporting = false;
    }

    void jump() {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax) {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }
    void concentrate()
    {
        if (Input.GetButtonDown("Concentrate") && !isConcentrating)
        {
            isConcentrating = true;
            concentrationTimer = stats.refillConcentrationTime;

            concentrationLight.SetActive(true);
        }
        if(isConcentrating)
        {
            concentrationTimer-= Time.deltaTime;

            if(concentrationTimer <= 0f)
            {
                concentrationController.refill();

                isConcentrating = false;
                concentrationTimer = 0f;

                concentrationLight.SetActive(false);
            }
        }
    }

    void rotateArm()
    {
        if (armPivot == null || Camera.main == null)
            return;

        Vector3 screenCenter = new Vector3(
             Screen.width / 2f,
             Screen.height / 2f,
             0f
         );

        Ray aimRay =
            Camera.main.ScreenPointToRay(screenCenter);

        Vector3 aimPoint =
            aimRay.GetPoint(100f);

        Vector3 aimDirection =
            aimPoint - armPivot.position;

        if (aimDirection.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(
                aimDirection.normalized
            );

        armPivot.rotation = Quaternion.Lerp(
            armPivot.rotation,
            targetRotation,
            armRotateSpeed * Time.deltaTime
        );
    }

    //Vector3 getAimPoint() {
    //    Camera cam = Camera.main;

    //    if (cam == null)
    //        return transform.position + transform.forward * 100f;

    //    Ray ray = new Ray(cam.transform.position, cam.transform.forward);

    //    if (Physics.Raycast(ray, out RaycastHit hit, 100f)) {
    //        return hit.point;
    //    }
    //    return ray.GetPoint(100f);
    //}
    //void updateAimTarget() {
    //    if (armAimTarget == null)
    //        return;
    //    armAimTarget.position = getAimPoint();
    //}
    void updateState() {
        if (healthSystem.IsDead) 
        {
            currentState = PlayerState.Dead;
            return;
        }

        if (isDodging) 
        {
            currentState = PlayerState.Dodge;
            return;
        }

        if (isTeleporting)
        {
            currentState = PlayerState.Teleport;
            return;
        }

        if (!controller.isGrounded) 
        {
            currentState = PlayerState.Jump;
            return;
        }
        if (isPlayerSprinting && moveDir.sqrMagnitude > 0.01f) 
        {
            currentState = PlayerState.Sprint;
            return;
        }
        if (moveDir.sqrMagnitude > 0.01f)
        {
            currentState = PlayerState.Walk;
            return;
        }
        if(isConcentrating)
        {
            currentState = PlayerState.Concentrate;
            return;
        }
        currentState = PlayerState.Idle;
    }

    //void updateAnimator() {
    //    animator.SetFloat("Speed", moveDir.magnitude);
    //    animator.SetBool("Sprint", isPlayerSprinting);
    //    animator.SetBool("Grounded", controller.isGrounded);
    //    animator.SetFloat("VerticalSpeed", playerVel.y);
    //}

    //Equipment Stat changes
    public void addJumps(int amount)
    {
        jumpMax += amount;
    }

    public void removeJumps(int amount)
    {
        jumpMax -= amount;
    }
}