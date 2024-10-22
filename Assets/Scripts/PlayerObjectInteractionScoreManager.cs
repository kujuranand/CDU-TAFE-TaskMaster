using System.Collections;
using System.Collections.Generic;
using TMPro;
using Firebase;
using Firebase.Database;
using UnityEngine;

public class PlayerObjectInteractionScoreManager : MonoBehaviour {

    [Header("Firebase")]
    public FirebaseScoreManager firebaseScoreManager;

    private int totalPPE = 0;
    private int totalPermits = 0;
    private int totalHazards = 0;
    private int totalCorrectHazards = 0;
    private int totalIncorrectHazards = 0;
    private int totalScore = 0;

    private void Start() {
        if (firebaseScoreManager == null) {
            firebaseScoreManager = FindObjectOfType<FirebaseScoreManager>();

            if (firebaseScoreManager == null) {
                Debug.LogError("FirebaseScoreManager not found in the scene. Make sure it is assigned.");
            }
        }

        StartCoroutine(WaitForFirebaseInitialization());
    }

    private IEnumerator WaitForFirebaseInitialization() {
        while (firebaseScoreManager == null || firebaseScoreManager.User == null) {
            yield return null;
        }

        Debug.Log("FirebaseScoreManager and User successfully initialized.");
    }

    public void SaveDataToFirebase() {
        if (firebaseScoreManager != null && firebaseScoreManager.User != null) {
            string username = firebaseScoreManager.User.DisplayName ?? "Unknown User";
            
            firebaseScoreManager.SaveUserData(username, totalPPE, totalPermits, totalHazards, totalScore);
            Debug.Log("Data saved to Firebase successfully.");
        } else {
            Debug.LogError("Cannot save data to Firebase. FirebaseScoreManager or User not found. Make sure you're logged in.");
        }
    }

    public void UpdateCounts(int ppe, int permits, int correctHazards, int incorrectHazards, int score) {
        totalPPE += ppe;
        totalPermits += permits;
        totalCorrectHazards += correctHazards;
        totalIncorrectHazards += incorrectHazards;
        totalHazards = totalCorrectHazards + totalIncorrectHazards;
        totalScore += score;

        Debug.Log($"Updated Counts - PPE: {totalPPE}, Permits: {totalPermits}, Hazards: {totalHazards}, Score: {totalScore}");
    }

    public void UpdatePPE(int ppe) {
        totalPPE += ppe;
        Debug.Log($"Updated PPE: {totalPPE}");
    }

    public void UpdatePermits(int permits) {
        totalPermits += permits;
        Debug.Log($"Updated Permits: {totalPermits}");
    }

    public void UpdateHazards(int correctHazards, int incorrectHazards) {
        totalCorrectHazards += correctHazards;
        totalIncorrectHazards += incorrectHazards;
        totalHazards = totalCorrectHazards + totalIncorrectHazards;
        Debug.Log($"Updated Hazards - Correct: {totalCorrectHazards}, Incorrect: {totalIncorrectHazards}, Total: {totalHazards}");
    }

    public void UpdateScore(int score) {
        totalScore += score;
        Debug.Log($"Updated Score: {totalScore}");
    }

    public int GetTotalPPE() {
        return totalPPE;
    }

    public int GetTotalPermits() {
        return totalPermits;
    }

    public int GetTotalHazards() {
        return totalHazards;
    }

    public int GetTotalScore() {
        return totalScore;
    }
}