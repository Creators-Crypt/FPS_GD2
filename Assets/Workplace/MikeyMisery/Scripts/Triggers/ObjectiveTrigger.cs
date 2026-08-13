using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private string newObjective;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectiveManager.SetObjective(newObjective);
            Destroy(gameObject); //Destroy the trigger after it's activated
        }

    }
}
