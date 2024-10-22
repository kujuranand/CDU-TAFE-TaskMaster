using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour {
    [Header("Managers")]
    public LevelManager levelManager;
    public UIManager uiManager;
    public FirebaseScoreManager firebaseScoreManager;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (levelManager != null) {
                levelManager.MainMenuScreen();
            }

            if (uiManager != null) {
                uiManager.UserDataScreen();
            }

            if (firebaseScoreManager != null) {
                firebaseScoreManager.SaveEndTime();
                firebaseScoreManager.FetchLatestUserData();
            }
        }
    }
}