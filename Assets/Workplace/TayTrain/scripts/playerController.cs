using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    //Player stats
    [Range(1,10)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] int speed;
    [Range(1, 10)][SerializeField] int sprintMod;
    [Range(1, 10)][SerializeField] int dodgeMod;
    [Range(1, 10)][SerializeField] int dodgeMax;
    [Range(1, 10)][SerializeField] int jumpSpeed;
    [Range(1, 10)][SerializeField] int jumpMax;
    [Range(1, 10)][SerializeField] int gravity;

    //Spell stats
    [Range(1, 10)][SerializeField] int spellDamage;
    [Range(1, 10)][SerializeField] int spellDist;
    [Range(1, 10)][SerializeField] int spellFireRate;

    int jumpCount;
    int HPOrig;
    int dodgeCount;

    float spellTimer;

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
       
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        dodge();

    }

    void movement()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * spellDist, Color.green);

        spellTimer += Time.deltaTime; 

        if(controller.isGrounded)
        {
            jumpCount = 0;
            playerVel.y = 0;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(moveDir.normalized * speed * Time.deltaTime);

        jump();
        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if(Input.GetButton("Fire1") && spellTimer > spellFireRate)
        {
            castSpell();
        }

    }

    void sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void dodge()
    {
        if(Input.GetButtonDown("Dodge") && dodgeCount < dodgeMax)
        {
            dodgeCount++;
            speed *= dodgeMod;
            //Lerp from one position or translate player very fast during specific time
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

    void castSpell()
    {
        spellTimer = 0;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
            out hit, spellDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(spellDamage);
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
    }
}

 
       
       
        

        
   
