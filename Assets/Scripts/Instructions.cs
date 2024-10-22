using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    [Header("Instructions Settings")]
    public LogDisplay logDisplay;         // Reference to the LogDisplay component
    public string customInstructionsText; // Customizable instructions text

    private bool instructionsDisplayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !instructionsDisplayed)
        {
            // Show custom instructions using LogDisplay
            if (logDisplay != null)
            {
                logDisplay.ShowLog(customInstructionsText); // Show the custom instructions
                Debug.Log("Show Instructions.");
                instructionsDisplayed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the instructions and disable the Instructions GameObject on exit
        if (logDisplay != null)
        {
            logDisplay.HideLog(); // Hide the instructions text
            Debug.Log("Hide Instructions.");
        }

        gameObject.SetActive(false); // Hide the Instructions GameObject
    }
}
