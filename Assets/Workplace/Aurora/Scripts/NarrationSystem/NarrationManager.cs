using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NarrationManager : MonoBehaviour {

    [System.Serializable]
    public struct StageDialogueMapping {

        public GameStage stage;
        public DialogueData dialogue;
    }

    [SerializeField] private List<StageDialogueMapping> stageNarrations;
    [SerializeField] private AudioSource audioSource;

    private Dictionary<GameStage, DialogueData> narrationLookup;

    private void OnEnable() {

        GameManager.OnStageChanged += HandleStageChanged;
        GameManager.OnPlayerAction += HandleDynamicAction;
    }
    private void OnDisable() {

        GameManager.OnStageChanged -= HandleStageChanged;
        GameManager.OnPlayerAction -= HandleDynamicAction;
    }
    private void Awake() {

        narrationLookup = new Dictionary<GameStage, DialogueData>();

        foreach (var mapping in stageNarrations) {
            narrationLookup[mapping.stage] = mapping.dialogue;
        }
    }
    private void HandleStageChanged(GameStage newStage) {
        
        if (narrationLookup.TryGetValue(newStage, out DialogueData data)) {

            PlayNarration(data);
        }
    }
    private void HandleDynamicAction(string obj) {

        Debug.Log("I has killed a slime!!!!!");
    }
    private void PlayNarration(DialogueData data) {

        if (data == null) return;

        if (data.voiceAudio != null) {

            audioSource.Stop();
            audioSource.PlayOneShot(data.voiceAudio);
        }

        //Send Dialogbox data only if we want Example; UIController.Instance.ShowSubtitle(data.speakerName, data.subtitleText, data.displayDuration);
        //Splash screen only is we want for future implementation.
    }

}