using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Boots : MonoBehaviour, IEquipment
{
    [Header("Boot Info")]
    [SerializeField] string bootsName;
    [SerializeField] string bootsDescriptoin;
    [SerializeField] Sprite icon;

    [Header("Boots Bonuses")]
    [SerializeField] int extraJumps;
    [SerializeField] float speedBonus;
    [SerializeField] float gravityMult;

    //Parts to show in a UI
    public string ItemName
    {
        get { return bootsName; }
    }
    public string ItemDescription
    {
        get { return bootsDescriptoin; }
    }
    public Sprite Icon
    {
        get { return icon; }
    }

    public EquipmentManager.EquipmentSlot Slot
    {
        get { return EquipmentManager.EquipmentSlot.Boots; }
    }

    public void equip(EquipStatsMods stats)
    {
        stats.addJumps(extraJumps);
        stats.addSpeed(speedBonus);
        stats.lowerGravity(gravityMult);
        //Debug.Log(bootsName + " Equipped");
    }

    public void unequip(EquipStatsMods stats)
    {
        stats.removeJumps(extraJumps);
        stats.removeSpeed(speedBonus);
        stats.restoreGravity(gravityMult);
        //Debug.Log(bootsName + " Unequipped");
    }
}
