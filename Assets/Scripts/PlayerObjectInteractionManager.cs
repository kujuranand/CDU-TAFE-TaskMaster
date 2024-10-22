using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;

public class PlayerObjectInteractionManager : MonoBehaviour {
    public enum InteractionType { PPEDisplay, PermitDisplay, HazardDisplay, Bonus }

    public string InteractionStartText = "Press [E]";
    public string QuestionText = "???";
    public string YesActionText = "Correct!";
    public string NoActionText = "Incorrect!";
    public string AddToListText = "Add to List";
    public string YesNoOptionsText = "Press [Y] for Yes, [N] for No";
    public string InteractionCompleteText = "Interaction Completed!";
    
    public LogDisplay logDisplay;
    public ScoreDisplay scoreDisplay;
    public InteractionType Type = InteractionType.PPEDisplay;
    
    public int CorrectScore = 10;
    public int IncorrectScore = 5;
    public string CorrectAnswer = "Yes";

    public AudioManager buttonAudioManager;
    public AudioManager correctAnswerAudioManager;
    public AudioManager incorrectAnswerAudioManager;

    public Animator animator;
    
    private PlayerObjectInteractionScoreManager scoreManager;
    private PlayerObjectInteractionListManager listManager;
    private PlayerInteractionEffects interactionEffects;
    private PlayerInteractionHideShow hideShow;

    private ThirdPersonController thirdPersonController;

    private int interactionStage = 0;
    private bool playerInsideTrigger = false;
    private bool interactionFinalized = false;

    private void Awake() {
        interactionEffects = GetComponent<PlayerInteractionEffects>();
        hideShow = GetComponent<PlayerInteractionHideShow>();
        scoreManager = FindObjectOfType<PlayerObjectInteractionScoreManager>();
        listManager = FindObjectOfType<PlayerObjectInteractionListManager>();
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
    }

    private void Update() {        
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame && interactionStage == 0 && !interactionFinalized) {
            if (buttonAudioManager != null) {
                buttonAudioManager.PlayAudio();
            }
            CompleteInteraction0();
        }

        if (interactionStage == 1) {
            if (Type == InteractionType.HazardDisplay) {
                if (Keyboard.current.yKey.wasPressedThisFrame) {
                    YesAction();
                } else if (Keyboard.current.nKey.wasPressedThisFrame) {
                    NoAction();
                }
            } else { 
                if (Keyboard.current.yKey.wasPressedThisFrame) {
                    YesAction();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInsideTrigger = true;

            if (interactionFinalized) {
                logDisplay.ShowLog(InteractionCompleteText);
            } else if (interactionStage < 2) {
                interactionStage = 0;
                logDisplay.ShowLog(InteractionStartText);
                interactionEffects.StartHighlight();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInsideTrigger = false;
            interactionEffects.StopHighlight();
            logDisplay.HideLog();

            if (!interactionFinalized) {
                interactionStage = 0;
            }
        }
    }

    private void CompleteInteraction0() {
        logDisplay.HideLog();
        string optionsText = Type == InteractionType.HazardDisplay ? YesNoOptionsText : "Press [Y] to proceed";
        logDisplay.ShowLog(QuestionText + "\n" + optionsText);
        interactionStage = 1;
    }

    private void YesAction() {
        logDisplay.HideLog();
        bool isCorrect = false;

        DisablePlayerMovement();

        if (Type == InteractionType.PPEDisplay || Type == InteractionType.PermitDisplay) {
            PlayPickObjectAnimation();
        } else if (Type == InteractionType.HazardDisplay && CorrectAnswer == "Yes") {
            PlayInspectAnimation();
        } else if (Type == InteractionType.HazardDisplay && CorrectAnswer == "No") {
            PlayAngryAnimation();
        }

        if (CorrectAnswer == "Yes") {
            AddScore(CorrectScore);
            logDisplay.ShowLog(YesActionText);
            interactionEffects.ShowTickMark();
            isCorrect = true;
        } else {
            AddScore(-IncorrectScore);
            logDisplay.ShowLog(NoActionText);
            interactionEffects.ShowCrossMark();
        }

        if (isCorrect) {
            if (correctAnswerAudioManager != null) {
                correctAnswerAudioManager.PlayAudio();
            }
        } else {
            if (incorrectAnswerAudioManager != null) {
                incorrectAnswerAudioManager.PlayAudio();
            }
        }

        FinalizeInteraction(isCorrect);
    }

    private void NoAction() {
        if (Type != InteractionType.HazardDisplay) return;
        
        logDisplay.HideLog();
        bool isCorrect = false;

        DisablePlayerMovement();

        if (CorrectAnswer == "No") {
            PlayPickObjectAnimation();
        } else {
            PlayAngryAnimation();
        }

        if (CorrectAnswer == "No") {
            AddScore(CorrectScore);
            logDisplay.ShowLog(YesActionText);
            interactionEffects.ShowTickMark();
            isCorrect = true;
        } else {
            AddScore(-IncorrectScore);
            logDisplay.ShowLog(NoActionText);
            interactionEffects.ShowCrossMark();
        }

        if (isCorrect) {
            if (correctAnswerAudioManager != null) {
                correctAnswerAudioManager.PlayAudio();
            }
        } else {
            if (incorrectAnswerAudioManager != null) {
                incorrectAnswerAudioManager.PlayAudio();
            }
        }

        FinalizeInteraction(isCorrect);
    }

    private void FinalizeInteraction(bool isCorrect) {
        interactionStage = 2;
        interactionFinalized = true;
        interactionEffects.StopHighlight();
        UpdateScoreText();

        if (Type == InteractionType.HazardDisplay && isCorrect) {
            scoreManager.UpdateHazards(1, 0);
        } else if (Type == InteractionType.PPEDisplay) {
            scoreManager.UpdatePPE(1);
            hideShow.HandleObjectAppearance();
        } else if (Type == InteractionType.PermitDisplay) {
            scoreManager.UpdatePermits(1);
        } else if (Type == InteractionType.Bonus) {
            AddScore(CorrectScore);
        }

        if (Type != InteractionType.Bonus && listManager != null) {
            listManager.AddItemToList(Type, isCorrect, AddToListText);
        } else if (Type != InteractionType.Bonus) {
            Debug.LogError("PlayerObjectInteractionListManager is not found.");
        }

        scoreManager.SaveDataToFirebase();
        logDisplay.ShowLog(InteractionCompleteText);
    }

    private void AddScore(int points) {
        scoreManager.UpdateScore(points);
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        if (scoreDisplay != null) {
            scoreDisplay.UpdateScore(scoreManager.GetTotalScore());
        } else {
            Debug.LogError("ScoreDisplay UI element is not assigned!", gameObject);
        }
    }

    private void PlayPickObjectAnimation() {
        animator.SetTrigger("PickObject");
        StartCoroutine(WaitForAnimationToFinish("PickObject"));
    }

    private void PlayInspectAnimation() {
        animator.SetTrigger("Inspect");
        StartCoroutine(WaitForAnimationToFinish("Inspect"));
    }

    private void PlayAngryAnimation() {
        animator.SetTrigger("Angry");
        StartCoroutine(WaitForAnimationToFinish("Angry"));
    }

    private IEnumerator WaitForAnimationToFinish(string animationName) {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));

        EnablePlayerMovement();
    }

    private void DisablePlayerMovement() {
        if (thirdPersonController != null) {
            thirdPersonController.DisableMovement();
        }
    }

    private void EnablePlayerMovement() {
        if (thirdPersonController != null) {
            thirdPersonController.EnableMovement();
        }
    }
}
