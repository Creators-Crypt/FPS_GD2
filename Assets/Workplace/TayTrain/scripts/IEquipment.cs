using UnityEngine;

public interface IEquipment
{
    EquipmentManager.EquipmentSlot Slot { get; }
    void equip(PlayerController player);

    void unequip(PlayerController player);
}
