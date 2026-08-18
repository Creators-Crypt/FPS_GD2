using UnityEngine;

public class Amulets : MonoBehaviour, IEquipment
{
    [Header("Amulet Info")]
    [SerializeField] string amuletName;
    [SerializeField] string amuletDescriptoin;
    [SerializeField] Sprite icon;

    [Header("Boots Bonuses")]
    [SerializeField] float staminaRegenMult;
    [SerializeField] float concentrationRegenMult;
    [SerializeField] float healthRegenMult;

    //Parts to show in a UI
    public string ItemName
    {
        get { return amuletName; }
    }
    public string ItemDescription
    {
        get { return amuletDescriptoin; }
    }
    public Sprite Icon
    {
        get { return icon; }
    }

    public EquipmentManager.EquipmentSlot Slot
    {
        get { return EquipmentManager.EquipmentSlot.Amulet; }
    }

    public void equip(EquipStatsMods stats)
    {
        //stats.addJumps(extraJumps);
        stats.increaseStaminaRegen(staminaRegenMult);
        stats.increaseConcentrationSpeedMult(concentrationRegenMult);
        stats.increaseHealthRegen(healthRegenMult);
        
        //Debug.Log(bootsName + " Equipped");
    }

    public void unequip(EquipStatsMods stats)
    {
        stats.normalStaminaRegen(staminaRegenMult);
        stats.normalConcentrationSpeedMult(concentrationRegenMult);
        stats.normalHealthRegen(healthRegenMult);
        //Debug.Log(bootsName + " Unequipped");
    }
}
