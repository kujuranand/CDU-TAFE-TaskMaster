using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TableInteraction : MonoBehaviour
{
    public GameObject InteractionText;
    public GameObject HazardText;
    public string customHazardText = "Custom Hazard Text"; // Set your custom text here
    public LogDisplay logDisplay; // Reference to the LogDisplay script

    private bool playerInsideTrigger = false;
    private static bool scoreAdded = false; // Static variable to track if the score has been added

    private void Update()
    {
        // Check for key press in the Update method
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame && !scoreAdded)
        {
            OnEKeyPressed();
            scoreAdded = true; // Set the flag to true after adding the score
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            InteractionText.SetActive(true);
            HazardText.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            InteractionText.SetActive(false);
            HazardText.SetActive(false);

            // Hide the log message on trigger exit
            logDisplay.HideLog();
        }
    }

    private void OnEKeyPressed()
    {
        InteractionText.SetActive(false); // Hide Interaction Text
        HazardText.SetActive(true);

        // Show log message on the display
        logDisplay.ShowLog(customHazardText);
        Debug.Log(customHazardText);

        ScoreManager.scoreCount += 10;
    }

}
