using UnityEngine;

public class EquipmentPickup : MonoBehaviour
{
    [SerializeField] EquipmentData equipment;

    private void OnTriggerEnter(Collider other)
    {
        IEquipmentPickup pickup = other.GetComponentInParent<IEquipmentPickup>();

        if (pickup != null)
        {
            EquipmentData oldEquipment = pickup.GetEquipment(equipment);

            if (oldEquipment != null)
            {
                equipment = oldEquipment;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

  
}
