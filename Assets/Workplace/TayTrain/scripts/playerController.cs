using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.LowLevel;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerStats stats;
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] StaminaController staminaController;

    [Header("Jump")]
    [Range(1, 10)][SerializeField] int jumpSpeed = 5;
    [Range(1, 10)][SerializeField] int jumpMax = 1;
    [SerializeField, Range(1, 10)] int gravity = 10;

    [Header("Arm Control")]
    [SerializeField] Transform armAim;
    [SerializeField] private float armRotateSpeed = 30f;

    [Header("Player State")]
    [SerializeField] private PlayerState currentState;

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

    // PlayerState
    public enum PlayerState
    {
        Idle,
        Walk,
        Sprint,
        Jump,
        Dodge,
        Dead
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
        controller = GetComponent<CharacterController>();
       
        healthSystem = GetComponent<HealthSystem>();

        staminaController = GetComponent<StaminaController>();
       
        currentSpeed = stats.walkSpeed;

    }
    // Update is called once per frame
    void Update()
    {
        rotateArm();
        stamina = staminaController.Current;
        isPlayerSprinting = Input.GetKey(KeyCode.LeftShift) && (staminaController.Current > stats.sprintStaminaCost);
        staminaController.IsConsuming = isPlayerSprinting;
        currentSpeed = (isPlayerSprinting) ? stats.sprintSpeed : stats.walkSpeed;
        if(isPlayerSprinting)
        {
            staminaController.ContinousSpent(stats.sprintStaminaCost);
        }

            if (dodgeCooldownTimer > 0)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }
        
        dodge();
        movement();
        updateState();
       
    }

    void movement()
    {
        if(controller.isGrounded && playerVel.y <0)
        {
            jumpCount = 0;
            playerVel.y = -2f;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right +
            Input.GetAxis("Vertical") * transform.forward;

        controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

        jump();

        controller.Move(playerVel * Time.deltaTime);

        playerVel.y -= gravity * Time.deltaTime;

    }

    void dodge()
    {
       

       if(Input.GetButtonDown("Dodge") && dodgeCooldownTimer <= 0 && staminaController.TrySpend(stats.dodgeStaminaCost))
        {
            isDodging = true;

            dodgeTimer = stats.dodgeDuration;
            dodgeCooldownTimer = stats.dodgeCooldown;

            dodgeDirection = moveDir.normalized;

            if(dodgeDirection == Vector3.zero)
            {
                dodgeDirection = transform.forward;
            }
        }

       if(isDodging)
        {
            controller.Move(dodgeDirection *
                stats.walkSpeed *
                stats.dodgeSpeedMultiplier *
                Time.deltaTime);
        }

        dodgeTimer -= Time.deltaTime;

        if(dodgeTimer <= 0)
        {
            isDodging = false;
        }
    }

    void jump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void rotateArm()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 10f);

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);
        Vector3 direction = (worldPosition - armAim.position).normalized;
        Quaternion rot = Quaternion.FromToRotation(armAim.right, direction) * armAim.rotation;
        armAim.rotation = Quaternion.Lerp(armAim.rotation, rot, armRotateSpeed * Time.deltaTime);
        
    }

    void updateState()
    {
        if (healthSystem.IsDead)
        {
            currentState = PlayerState.Dead;
            return;
        }

        if(isDodging)
        {
            currentState = PlayerState.Dodge;
            return;
        }

        if(!controller.isGrounded)
        {
            currentState = PlayerState.Jump;
            return;
        }
        if(isPlayerSprinting && moveDir.sqrMagnitude > 0.01f)
        {
            currentState = PlayerState.Sprint;
            return;
        }
        if(moveDir.sqrMagnitude > 0.01f)
        {
            currentState = PlayerState.Walk;
            return;
        }
        currentState = PlayerState.Idle;
    }

}
