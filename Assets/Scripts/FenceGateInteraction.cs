using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FenceGateInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string interactionText = "Press [E] to interact with the gate"; // Custom text to display
    public LogDisplay logDisplay; // Reference to the LogDisplay for showing text

    [Header("Animator and Audio")]
    public Animator animator;
    public AudioSource fenceGateOpenAudio;
    public AudioSource fenceGateCloseAudio;

    private bool playerInsideTrigger = false;
    private bool gateOpen = false;

    private void Update()
    {
        // Check for key press in the Update method
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame)
        {
            OnEKeyPressed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            ShowInteractionText(); // Show the custom interaction text when the player enters
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            logDisplay.HideLog(); // Hide the interaction text when the player exits
        }
    }

    private void OnEKeyPressed()
    {
        logDisplay.HideLog(); // Hide the interaction text after the player presses E

        if (gateOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        animator.SetTrigger("doorOpen"); // Set door open trigger
        PlayDoorAudio(fenceGateOpenAudio); // Play door opening audio
        gateOpen = true;
    }

    private void CloseDoor()
    {
        animator.SetTrigger("doorClose"); // Set door close trigger
        PlayDoorAudio(fenceGateCloseAudio); // Play door closing audio
        gateOpen = false;
    }

    // Play the corresponding audio clip based on the provided AudioSource
    private void PlayDoorAudio(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void ShowInteractionText()
    {
        if (logDisplay != null)
        {
            logDisplay.ShowLog(interactionText); // Display the custom text using the LogDisplay
        }
    }
}
