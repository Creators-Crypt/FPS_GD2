using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] EquipmentManager equipmentManager;

    [Header("Helmet")]
    [SerializeField] TMP_Text helmetName;

    [Header("Amulet")]
    [SerializeField] TMP_Text amuletName;

    [Header("Armor")]
    [SerializeField] TMP_Text armorName;

    [Header("Boots")]
    [SerializeField] Image bootsIcon;
    [SerializeField] TMP_Text bootsName;
    [SerializeField] TMP_Text bootsDescription;
    [SerializeField] TMP_Text bootsStats;

    void Update()
    {
        EquipmentData helmet = equipmentManager.GetHelmet();
        EquipmentData amulet = equipmentManager.GetAmulet();
        EquipmentData armor = equipmentManager.GetArmor();
        EquipmentData boots = equipmentManager.GetBoots();

        if (helmet != null)
        {
            helmetName.text = helmet.itemName;
        }
        else
        {
            helmetName.text = "Helmet";
        }

        if (amulet != null)
        {
            amuletName.text = amulet.itemName;
        }
        else
        {
            amuletName.text = "Amulet";
        }

        if (armor != null)
        {
            armorName.text = armor.itemName;
        }
        else
        {
            armorName.text = "Armor";
        }

        if (boots != null )
        {
            bootsName.text = boots.itemName;
        }
        else
        {
            bootsName.text = "Boots";
        }
    }
}
