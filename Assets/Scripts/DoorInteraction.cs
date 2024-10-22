using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    public enum InteractionRequirement { None, PPE, Permits, Hazards }

    [Header("Door Interaction Settings")]
    public InteractionRequirement interactionRequirement = InteractionRequirement.None;

    [Header("Custom Texts")]
    public string interactionText = "Press [E] to interact";
    public string doorLockedText = "The door is locked. Complete required tasks.";

    [Header("UI Displays")]
    public LogDisplay logDisplay;
    public Animator animator;

    public AudioSource doorOpenAudio;
    public AudioSource doorLockedAudio;
    public AudioSource doorCloseAudio;

    private bool playerInsideTrigger = false;
    private bool doorOpen = false;

    private PlayerObjectInteractionListManager listManager;

    private void Awake()
    {
        listManager = FindObjectOfType<PlayerObjectInteractionListManager>();
    }

    private void Update()
    {
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
            logDisplay.ShowLog(interactionText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            logDisplay.HideLog();
        }
    }

    private void OnEKeyPressed()
    {
        bool areRequiredInteractionsComplete = CheckRequiredInteractionsComplete();

        logDisplay.HideLog();

        if (areRequiredInteractionsComplete)
        {
            if (doorOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
        else
        {
            logDisplay.ShowLog(doorLockedText);
            PlayDoorAudio(doorLockedAudio);
        }
    }

    private bool CheckRequiredInteractionsComplete()
    {
        if (interactionRequirement == InteractionRequirement.None)
        {
            return listManager.ListsAreEmptyCheck();
        }

        if (interactionRequirement == InteractionRequirement.PPE)
        {
            return listManager.PPEListFullCheck();
        }
        else if (interactionRequirement == InteractionRequirement.Permits)
        {
            return listManager.PermitsListFullCheck();
        }
        else if (interactionRequirement == InteractionRequirement.Hazards)
        {
            return listManager.HazardsListFullCheck();
        }

        return false;
    }

    private void OpenDoor()
    {
        animator.SetTrigger("doorOpen");
        logDisplay.HideLog();
        PlayDoorAudio(doorOpenAudio);
        doorOpen = true;
    }

    private void CloseDoor()
    {
        animator.SetTrigger("doorClose");
        PlayDoorAudio(doorCloseAudio);
        doorOpen = false;
    }

    private void PlayDoorAudio(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource is null.");
        }
    }
}
