using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public enum InteractionType { PPEDisplay, PermitDisplay, HazardDisplay }

    [Header("Interaction Texts")]
    public string InteractionStartText = "Press [E]";
    public string QuestionText = "???";
    public string YesActionText = "Correct!";
    public string NoActionText = "Incorrect!";
    public string AddToListText = "Add to List"; // Generic for all lists
    public string YesNoOptionsText = "Press [Y] for Yes, [N] for No"; // Customizable Yes/No options text
    public string InteractionCompleteText = "Interaction Completed!"; // Custom field for completion message

    [Header("UI Displays")]
    public LogDisplay logDisplay;
    public ScoreDisplay scoreDisplay;
    public InteractionType Type = InteractionType.PPEDisplay; // Field to select between PPE, Permit, or Hazard list

    [Header("Interaction Score")]
    public int CorrectScore = 10; // Score for correct answers
    public int IncorrectScore = 5; // Score to subtract for incorrect answers
    public string CorrectAnswer = "Yes"; // Custom field to specify the correct answer (Yes/No)

    private PlayerInteractionEffects interactionEffects;
    private PlayerInteractionHideShow hideShow;
    private PlayerInteractionListManager listManager; // Reference to the List Manager

    private int interactionStage = 0; // Track interaction stages for each object
    
    private bool playerInsideTrigger = false; // Track if the player is inside the trigger for this object

    private void Awake()
    {
        interactionEffects = GetComponent<PlayerInteractionEffects>();
        hideShow = GetComponent<PlayerInteractionHideShow>();
        listManager = GetComponent<PlayerInteractionListManager>(); // Get reference to the List Manager
    }

    private void Update()
    {
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame && interactionStage == 0)
        {
            CompleteInteraction0(); // Move to Interaction 1 automatically
        }

        if (interactionStage == 1)
        {
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                YesAction(); // Handle Yes action
            }
            else if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                NoAction(); // Handle No action
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;

            // Only start interaction if it's not already completed
            if (interactionStage < 2)
            {
                interactionStage = 0; // Start from interaction 0 if not completed
                logDisplay.ShowLog(InteractionStartText);
                interactionEffects.StartHighlight();
            }
            else
            {
                // Display the custom interaction complete message
                logDisplay.ShowLog(InteractionCompleteText);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            interactionEffects.StopHighlight();
            logDisplay.HideLog();
            interactionStage = 0; // Reset the interaction stage when exiting trigger
        }
    }

    private void CompleteInteraction0()
    {
        logDisplay.HideLog();
        logDisplay.ShowLog(QuestionText + "\n" + YesNoOptionsText);
        interactionStage = 1; // Move directly to Interaction Stage 1
    }

    private void YesAction()
    {
        logDisplay.HideLog();
        bool isCorrect = false; // To track if the answer was correct

        if (CorrectAnswer == "Yes")
        {
            AddScore(CorrectScore); // Add correct score
            logDisplay.ShowLog(YesActionText);
            interactionEffects.ShowTickMark();
            isCorrect = true;
        }
        else
        {
            AddScore(-IncorrectScore); // Subtract incorrect score
            logDisplay.ShowLog(NoActionText);
            interactionEffects.ShowCrossMark();
            isCorrect = false;
        }

        FinalizeInteraction(isCorrect);
    }

    private void NoAction()
    {
        logDisplay.HideLog();
        bool isCorrect = false; // To track if the answer was correct

        if (CorrectAnswer == "No")
        {
            AddScore(CorrectScore); // Add correct score
            logDisplay.ShowLog(YesActionText);
            interactionEffects.ShowTickMark();
            isCorrect = true;
        }
        else
        {
            AddScore(-IncorrectScore); // Subtract incorrect score
            logDisplay.ShowLog(NoActionText);
            interactionEffects.ShowCrossMark();
            isCorrect = false;
        }

        FinalizeInteraction(isCorrect);
    }

    private void FinalizeInteraction(bool isCorrect)
    {
        interactionStage = 2; // Mark Interaction as complete for this specific object
        interactionEffects.StopHighlight(); // Stop the highlight effect
        UpdateScoreText();

        // Use the List Manager to add the item to the correct panel based on the InteractionType, and mark if correct or incorrect
        if (listManager != null)
        {
            listManager.AddItemToList(Type, isCorrect);
        }

        if (hideShow != null)
        {
            hideShow.HandleObjectAppearance();
        }

        // Display the custom interaction complete message
        logDisplay.ShowLog(InteractionCompleteText);
    }


    private void AddScore(int points)
    {
        PlayerInteractionScoreManager.AddScore(points);  // Use PlayerInteractionScoreManager to update the total score
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreDisplay != null)
        {
            scoreDisplay.UpdateScore(PlayerInteractionScoreManager.GetTotalScore()); // Display the total score
        }
        else
        {
            Debug.LogError("ScoreDisplay UI element is not assigned!", gameObject);
        }
    }
}