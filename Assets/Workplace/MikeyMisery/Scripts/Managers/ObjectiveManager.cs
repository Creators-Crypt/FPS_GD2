using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private MainObjectiveUI objectiveUI;

    private string currentObjective;

    public void SetObjective(string newObjective)
    {
        currentObjective = newObjective;
        objectiveUI.SetObjective(currentObjective);
    }    
}
