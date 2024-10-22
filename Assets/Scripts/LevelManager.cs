using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [Header("Level")]
    public GameObject mainMenu;
    public GameObject level1;

    [Header("Managers")]
    public FirebaseScoreManager firebaseScoreManager;

    public void ClearScreen() {
        if (mainMenu != null) mainMenu.SetActive(false);
        if (level1 != null) level1.SetActive(false);
    }

    public void MainMenuScreen() {
        ClearScreen();
        if (mainMenu != null) mainMenu.SetActive(true);
    }

    public void Level1Screen() {
        ClearScreen();
        if (level1 != null) level1.SetActive(true);

        if (firebaseScoreManager != null) {
            firebaseScoreManager.SaveStartTime();
        }
    }
}