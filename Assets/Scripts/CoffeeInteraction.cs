using UnityEngine;
using UnityEngine.InputSystem;

public class CoffeeInteraction : MonoBehaviour
{
    [Header("Interaction Texts")]
    public string InteractionStartText = "Press [E] to grab the coffee";   // Text for interaction start prompt
    public string InteractionText1 = "You found Glenn's coffee!"; // Text for the first stage of interaction
    public string YesOptionText = "Press [Y] to take the coffee"; // Customizable Yes option text
    public string InteractionCompleteText = "Bonus found: Coffee collected!"; // Custom field for completion message

    [Header("UI Displays")]
    public LogDisplay logDisplay;
    public ScoreDisplay scoreDisplay;

    [Header("Interaction Score")]
    public int BonusScore = 50; // Bonus score for finding the coffee

    private PlayerInteractionEffects interactionEffects;
    private PlayerInteractionHideShow hideShow;

    public int InteractionStage { get; private set; } = 0;  // Public getter for the interaction stage, starts at 0
    private static int totalScore = 0;  // Keep track of total score across interactions
    private bool playerInsideTrigger = false; // Keep track of player presence for this specific gameobject

    private void Awake()
    {
        interactionEffects = GetComponent<PlayerInteractionEffects>();
        hideShow = GetComponent<PlayerInteractionHideShow>();
    }

    private void Update()
    {
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame && InteractionStage == 0)
        {
            CompleteInteraction0(); // Move to Interaction 1 automatically
        }

        if (InteractionStage == 1)
        {
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                YesAction(); // Handle Yes action
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;

            // Only start interaction if it's not already completed
            if (InteractionStage < 2)
            {
                InteractionStage = 0; // Start from interaction 0 if not completed
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
        }
    }

    private void CompleteInteraction0()
    {
        logDisplay.HideLog();
        logDisplay.ShowLog(InteractionText1 + "\n" + YesOptionText); // Show Interaction text 1 and Yes option
        InteractionStage = 1; // Move directly to Interaction Stage 1
    }

    private void YesAction()
    {
        logDisplay.HideLog();
        AddScore(BonusScore); // Increase score for finding the coffee
        interactionEffects.ShowTickMark(); // Show tick mark

        FinalizeInteraction();
    }

    private void FinalizeInteraction()
    {
        InteractionStage = 2; // Mark Interaction as complete for this specific object
        interactionEffects.StopHighlight(); // Stop the highlight effect
        UpdateScoreText();

        if (hideShow != null)
        {
            hideShow.HandleObjectAppearance();
        }

        // Display the custom interaction complete message
        logDisplay.ShowLog(InteractionCompleteText);
    }

    private void AddScore(int points)
    {
        totalScore += points;

        // Ensure the score does not go below 0
        if (totalScore < 0)
        {
            totalScore = 0;
        }

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreDisplay != null)
        {
            scoreDisplay.UpdateScore(totalScore);
        }
        else
        {
            Debug.LogError("ScoreDisplay UI element is not assigned!", gameObject);
        }
    }
}
