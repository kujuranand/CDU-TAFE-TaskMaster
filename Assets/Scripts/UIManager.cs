using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {

    [Header("UI")]
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject userDataUI;
    public GameObject unitUI;
    public GameObject scoreboardUI;

    public void ClearScreen() {
        if (loginUI != null) loginUI.SetActive(false);
        if (registerUI != null) registerUI.SetActive(false);
        if (userDataUI != null) userDataUI.SetActive(false);
        if (unitUI != null) unitUI.SetActive(false);
        if (scoreboardUI != null) scoreboardUI.SetActive(false);
    }

    public void LoginScreen() {
        ClearScreen();
        if (loginUI != null) loginUI.SetActive(true);
    }

    public void RegisterScreen() {
        ClearScreen();
        if (registerUI != null) registerUI.SetActive(true);
    }

    public void UserDataScreen() {
        ClearScreen();
        if (userDataUI != null) userDataUI.SetActive(true);
    }

    public void UnitUIScreen() {
        ClearScreen();
        if (unitUI != null) unitUI.SetActive(true);
    }

    public void ScoreboardUIScreen() {
        ClearScreen();
        if (scoreboardUI != null) scoreboardUI.SetActive(true);
    }
}