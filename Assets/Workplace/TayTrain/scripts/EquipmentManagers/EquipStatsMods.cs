using UnityEngine;

public class EquipStatsMods : MonoBehaviour
{

    [Header("Equipment Bonuses")]
    //Amulets
    [SerializeField] float staminaRegenMult = 1f;
    [SerializeField] float concentrationRegenMult = 1f;
    [SerializeField] float healthRegenMult = 1f;
    public float StaminaRegenMult
    {
        get { return staminaRegenMult; }
    }
    public float ConcentrationRegenMult
    {
        get { return concentrationRegenMult; }
    }
    public float HealthRegenMult
    {
        get { return healthRegenMult; }
    }
    //Boots
    [SerializeField] int bonusJumps;
    [SerializeField] float speedMult = 1f;
    [SerializeField] float gravityMult = 1f;

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
    }
    public void removeJumps(int amount)
    {
        bonusJumps -= amount;
    }
    public void addSpeed(float amount)
    {
        speedMult += amount;
    }
    public void removeSpeed(float amount)
    {
        speedMult -= amount;
    }
    public void lowerGravity(float amount)
    {
        gravityMult -= amount;
        gravityMult = Mathf.Max(0.1f, gravityMult);
    }
    public void restoreGravity(float amount)
    {
        gravityMult += amount;
        
    }
}
