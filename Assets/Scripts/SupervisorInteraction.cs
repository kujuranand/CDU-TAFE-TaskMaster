using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SupervisorInteraction : MonoBehaviour
{
    [Header("Interaction Dialog Arrays")]
    public string[] interaction0Dialogs;
    public string[] interaction1Dialogs;
    public string[] interaction2Dialogs;
    public string[] interaction3Dialogs;

    [Header("UI Displays")]
    public LogDisplay logDisplay;

    [Header("Phone and Animation")]
    public GameObject Phone;
    public Animator animator;

    [Header("Interaction Text")]
    public string interactionText = "Press [E] to continue";

    public AudioManager buttonAudioManager;

    private bool playerInsideTrigger = false;
    private string[] currentDialogs;
    private int currentDialogIndex = 0;

    private PlayerObjectInteractionListManager listManager;

    private Quaternion originalRotation;
    public Transform player;

    private void Start() {
        originalRotation = transform.rotation;

        if (animator == null) {
            Debug.LogError("Animator is not assigned.");
        }

        if (Phone != null) {
            Phone.SetActive(false);
        }
        else {
            Debug.LogError("Phone GameObject is not assigned.");
        }

        listManager = FindObjectOfType<PlayerObjectInteractionListManager>();

        if (listManager == null) {
            Debug.LogError("PlayerObjectInteractionListManager is not found.");
        }
    }

    private void Update() {
        if (playerInsideTrigger && player != null) {
            FacePlayer();
        }
        
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame) {
            OnEKeyPressed();
        }

        if (IsPhoneCallAnimationPlaying()) {
            Phone.SetActive(true);
        } else {
            Phone.SetActive(false);
        }
    }

    private void OnEKeyPressed() {
        Debug.Log("E key pressed.");
        if (currentDialogs != null && currentDialogIndex < currentDialogs.Length) {
            logDisplay.ShowLog(currentDialogs[currentDialogIndex]);

            if (currentDialogIndex < currentDialogs.Length - 1) {
                logDisplay.ShowLog(currentDialogs[currentDialogIndex] + "\n" + interactionText);
            }

            currentDialogIndex++;
            animator.SetTrigger("Talk3");
            buttonAudioManager.PlayAudio();

            if (currentDialogIndex >= currentDialogs.Length) {
                CompleteInteraction();
            }
        }
    }

    private void CompleteInteraction() {
        animator.SetTrigger("Talk2");
    }

    private bool IsPhoneCallAnimationPlaying() {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("PhoneCall");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInsideTrigger = true;
            animator.SetTrigger("Talk1");

            FacePlayer();

            if (listManager.ListsAreEmptyCheck()) {
                currentDialogs = interaction0Dialogs;
            } else if (listManager.PPEListFullCheck() && !listManager.PermitsListFullCheck() && !listManager.HazardsListFullCheck()) {
                currentDialogs = interaction1Dialogs;
            } else if (listManager.PPEListFullCheck() && listManager.PermitsListFullCheck() && !listManager.HazardsListFullCheck()) {
                currentDialogs = interaction2Dialogs;
            } else if (listManager.PPEListFullCheck() && listManager.PermitsListFullCheck() && listManager.HazardsListFullCheck()) {
                currentDialogs = interaction3Dialogs;
            }

            currentDialogIndex = 0;
            if (currentDialogs != null && currentDialogs.Length > 0) {
                logDisplay.ShowLog(currentDialogs[0] + "\n" + interactionText);
                currentDialogIndex++;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInsideTrigger = false;
            logDisplay.HideLog();
            transform.rotation = originalRotation;
        }
    }

    private void FacePlayer() {
        if (player != null) {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; 
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        } else {
            Debug.LogError("Player reference is not assigned.");
        }
    }
}
