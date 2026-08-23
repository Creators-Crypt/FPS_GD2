using UnityEngine;

public class TutorialProgress : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private GameObject tutorialCage;
    [SerializeField] private GameObject tutorialSpawner;

    [Header("Tutorial Progress")]
    private bool tutorialActive;

    private bool jumped;
    private bool dodged;
    private bool teleported;
    private bool concentrated;

    private int slimeKills;
    private int slimeKillsNeeded = 9;

    private void OnEnable()
    {
        GameManager.OnPlayerAction += HandlePlayerAction;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerAction -= HandlePlayerAction;
    }

    public void StartTutorial()
    {
        tutorialActive = true;
        UpdateTutorialUI();
    }
    
    private void HandlePlayerAction(string actionKey)
    {
        if (!tutorialActive)
            return;

        switch(actionKey)
        {
            case "Jump":
                jumped = true;
                break;

            case "Dodge":
                dodged = true;
                break;

            case "Teleport":
                teleported = true;
                break;

            case "Concentrate":
                concentrated = true;
                break;

            case "SlimeKilled":
                if(slimeKills < slimeKillsNeeded)
                {
                    slimeKills++;
                }
                break;

        }

        UpdateTutorialUI();
        CheckTutorialComplete();
    }

    private void UpdateTutorialUI()
    {
        string tutorialText = "TUTORIAL\n";

        tutorialText += jumped ? " X Jump [SPACE] " : " Jump [SPACE] ";
        tutorialText += dodged ? " X Dodge [L ALT] " : " Dodge [L ALT] ";
        tutorialText += teleported ? " X Teleport [E]\n" : " Teleport [E]\n";
        tutorialText += concentrated ? " X Concentrate [C] " : " Concentrate [C] ";

        tutorialText += "Kill Slimes (" + slimeKills + "/" + slimeKillsNeeded + ")";

        objectiveManager.SetObjective(tutorialText);

    }

    private void CheckTutorialComplete()
    {
        if(jumped && dodged && teleported && concentrated && slimeKills >= slimeKillsNeeded)
        {
            CompleteTutorial();
        }
    }

    private void CompleteTutorial()
    {
        tutorialActive = false;

        tutorialCage.SetActive(false);
        tutorialSpawner.SetActive(false);

        GameManager.Instance.SetStage(GameStage.Intro_SlimeShowcase);

        objectiveManager.SetObjective("Continue through the forest");
    }
}
