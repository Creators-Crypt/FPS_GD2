using System;
using UnityEngine;

public class CastleGate : MonoBehaviour, IInteractable {

    [SerializeField] private bool hasKey = false;
    
    public void Interact() {

        if (!hasKey) {

            GameManager.Instance.SetStage(GameStage.Castle_ForgotKey);
            ActivateKeyQuestEnemy();
        } else {

            GameManager.Instance.SetStage(GameStage.Castle_Explore);
            OpenCastleGate();
        }
    }
    private void ActivateKeyQuestEnemy() {
        throw new NotImplementedException();
    }
    private void OpenCastleGate() {
        throw new NotImplementedException();
    }
}