using Unity.VisualScripting;
using UnityEngine;

public class EquipStatsMods : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerController playerController;
    [SerializeField] StaminaController staminaController;

    [Header("Equipment Bonuses")]
    //Amulets
    [SerializeField] float staminaRegenMult = 1f;
    [SerializeField] float concentrationSpeedMult = 1f;
    [SerializeField] float healthRegenMult = 1f;
   
    //Boots
    [SerializeField] int bonusJumps;
    [SerializeField] float speedMult = 1f;
    [SerializeField] float gravityMult = 1f;

    public float StaminaRegenMult
    {
        get { return staminaRegenMult; }
    }
    public float ConcentrationSpeedMult
    {
        get { return concentrationSpeedMult; }
    }
    public float HealthRegenMult
    {
        get { return healthRegenMult; }
    }
    public void increaseStaminaRegen(float amount)
    {
        staminaRegenMult += amount;
        staminaController.setRegenMult(staminaRegenMult);
    }
    public void increaseConcentrationSpeedMult(float amount)
    {
        concentrationSpeedMult += amount;
        playerController.setConcentrationSpeedMult(concentrationSpeedMult);
    }
    public void increaseHealthRegen(float amount)
    {
        healthRegenMult += amount;
        playerController.setHealthRegenMult(healthRegenMult);
    }
    public void normalStaminaRegen(float amount)
    {
        staminaRegenMult -= amount;
        if(staminaRegenMult < 1f)
        {
            staminaRegenMult = 1f;
        }

        staminaController.setRegenMult(staminaRegenMult);
    }
    public void normalConcentrationSpeedMult(float amount)
    {
        concentrationSpeedMult -= amount;
        if (concentrationSpeedMult < 1f)
        {
            concentrationSpeedMult = 1f;
        }
        playerController.setConcentrationSpeedMult(concentrationSpeedMult) ;
    }
    public void normalHealthRegen(float amount)
    {
        healthRegenMult -= amount;
        if (healthRegenMult < 1f)
        {
            healthRegenMult = 1f;
        }
        playerController.setHealthRegenMult(healthRegenMult);
    }
    public int BonusJumps
    {
        get { return bonusJumps;}
    }
    public float SpeedMult
    {
        get { return speedMult; }
    }
    public float GravityMult
    {
        get { return gravityMult; }
    }
    public void addJumps(int amount)
    {
        bonusJumps += amount;
        playerController.setBonusJumps(bonusJumps);
    }
    public void removeJumps(int amount)
    {
        bonusJumps -= amount;
        if(bonusJumps < 0)
        {
            bonusJumps = 0;
        }
        playerController.setBonusJumps(bonusJumps);
    }
    public void addSpeed(float amount)
    {
        speedMult += amount;
        playerController.setSpeedMult(speedMult);
    }
    public void removeSpeed(float amount)
    {
        speedMult -= amount;
        if(speedMult < 1f)
        {
            speedMult = 1f;
        }
        playerController.setSpeedMult(speedMult);
    }
    public void lowerGravity(float amount)
    {
        gravityMult -= amount;
        gravityMult = Mathf.Max(0.1f, gravityMult);
        playerController.setGravityMult(gravityMult);
    }
    public void restoreGravity(float amount)
    {
        gravityMult += amount;
        
        if(gravityMult > 1f)
        {
            gravityMult = 1f;
        }
        playerController.setGravityMult(gravityMult);
        
    }
}
