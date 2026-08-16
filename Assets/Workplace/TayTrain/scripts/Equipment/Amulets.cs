using UnityEngine;

public class Amulets : MonoBehaviour, IEquipment
{
    [Header("Amulet Info")]
    [SerializeField] string amuletName;
    [SerializeField] string amuletDescriptoin;
    [SerializeField] Sprite icon;

    [Header("Boots Bonuses")]
    [SerializeField] float staminaRegeMult;
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
        
        //Debug.Log(bootsName + " Equipped");
    }

    public void unequip(EquipStatsMods stats)
    {
       
        //Debug.Log(bootsName + " Unequipped");
    }
}
