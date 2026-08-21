using System;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("Player")) {

            GameManager.Instance.SetStage(GameStage.Intro_TutorialTrapped);
            TrapPlayerInArena();
            SpawnSlimes();
        }
    }

    private void TrapPlayerInArena() {
        throw new NotImplementedException();
    }

    private void SpawnSlimes() {
        throw new NotImplementedException();
    }
}