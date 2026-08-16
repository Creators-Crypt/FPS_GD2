using UnityEngine;

public class EquipStatsMods : MonoBehaviour
{

    [Header("Equipment Bonuses")]
    //Boots
    [SerializeField] int bonusJumps;
    [SerializeField] float speedMult = 1f;
    [SerializeField] float gravityMult = 1f;

    //Boots
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
