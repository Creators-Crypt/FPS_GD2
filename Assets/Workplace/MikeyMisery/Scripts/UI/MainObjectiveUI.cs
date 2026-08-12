using UnityEngine;
using TMPro;

public class MainObjectiveUI : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;

    public void SetObjective(string newObjective)
    {
        objectiveText.text = newObjective;
    }


}
