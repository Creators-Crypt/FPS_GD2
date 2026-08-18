using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [Header("Equipment Slots")]
    [SerializeField] GameObject helmet;
    [SerializeField] GameObject amulet;
    [SerializeField] GameObject armor;
    [SerializeField] GameObject boots;

    //for testing
    [SerializeField] GameObject testItem;

    [Header("Player")]
    [SerializeField] EquipStatsMods stats;

    //Equipment types
    public enum EquipmentSlot
    {
        Helmet,
        Amulet,
        Armor,
        Boots
    }

    //Used for testing can change when we decide for UI and Inventory
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            equipItem(testItem);
        }
       
        if(Input.GetKeyDown(KeyCode.G))
        {
            unequipItem(testItem);
        }
    }

    public void equipItem(GameObject item)
    {
        IEquipment equipment = item.GetComponent<IEquipment>();

        if (equipment == null)
            return;

        GameObject currentItem = null;

      switch (equipment.Slot)
        {
            case EquipmentSlot.Helmet:
                currentItem = helmet;
                break;

            case EquipmentSlot.Amulet:
                currentItem = amulet;
                break;

            case EquipmentSlot.Armor:
                currentItem = armor;
                break;

            case EquipmentSlot.Boots:
                currentItem = boots;
                break;
        }

        if (currentItem == item)
            return;

        if (currentItem != null)
        {
            IEquipment oldEquipment = currentItem.GetComponent<IEquipment>();

            if (oldEquipment != null)
            {
                oldEquipment.unequip(stats);
            }
        }

        switch (equipment.Slot)
        {
            case EquipmentSlot.Helmet:
                helmet = item;
                break;

            case EquipmentSlot.Amulet:
                amulet = item;
                break;

            case EquipmentSlot.Armor:
                armor = item;
                break;

            case EquipmentSlot.Boots:
                boots = item;
                break;
        }

        equipment.equip(stats);
    }

    public void unequipItem(GameObject item)
    {
        IEquipment equipment = item.GetComponent<IEquipment>();

        if (equipment == null)
            return;

        switch (equipment.Slot)
        {
            case EquipmentSlot.Helmet:
                if (helmet != item)
                    return;

                helmet = null;
                break;

            case EquipmentSlot.Amulet:
                if (amulet != item)
                    return;

                amulet = null;
                break;

            case EquipmentSlot.Armor:
                if (armor != item)
                    return;

                armor = null;
                break;

            case EquipmentSlot.Boots:
                if (boots != item)
                    return;

                boots = null;
                break;
        }

        equipment.unequip(stats);
    }
}
