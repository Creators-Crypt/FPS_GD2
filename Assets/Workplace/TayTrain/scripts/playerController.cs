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

    [SerializeField] private float dodgeSpeed;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
        controller = GetComponent<CharacterController>();
       
        healthSystem = GetComponent<HealthSystem>();

        staminaController = GetComponent<StaminaController>();

        dodgeSpeed = stats.dodgeSpeedMultiplier;
       
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
       
    }

    void movement()
    {
        if(controller.isGrounded)
        {
            jumpCount = 0;
            playerVel.y = 0;
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
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rot = Quaternion.LookRotation(worldPosition);
        armAim.rotation = Quaternion.Lerp(armAim.rotation, rot, 100);
        
    }

}
