using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Boots : MonoBehaviour, IEquipment
{
    [Header("Boot Info")]
    [SerializeField] string bootsName;

    [Header("Boots Bonuses")]
    [SerializeField] int extraJumps;
    [SerializeField] float staminaBonus;
    [SerializeField] float speedBonus;

    public EquipmentManager.EquipmentSlot Slot
    {
        get { return EquipmentManager.EquipmentSlot.Boots; }
    }

  
    public void equip(PlayerController player)
    {
        player.addJumps(extraJumps);
        Debug.Log(bootsName + " Equipped");
    }

    public void unequip(PlayerController player)
    {
        player.removeJumps(extraJumps);
        Debug.Log(bootsName + " Unequipped");
    }
}
