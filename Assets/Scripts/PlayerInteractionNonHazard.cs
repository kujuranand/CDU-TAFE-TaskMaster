using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionNonHazard : MonoBehaviour
{
    public PlayerInteraction.InteractionType Type = PlayerInteraction.InteractionType.PPEDisplay; // Field to select between PPE or Permit list

    [Header("Interaction Texts")]
    public string InteractionStartText = "Press [E]";   // Text for interaction start prompt
    public string InteractionText1 = "Proceed with interaction"; // Text for the first stage of interaction
    public string YesOptionText = "Press [Y] to proceed"; // Customizable Yes option text
    public string InteractionCompleteText = "Interaction Completed!"; // Custom field for completion message

    [Header("UI Displays")]
    public LogDisplay logDisplay;
    public ScoreDisplay scoreDisplay;

    [Header("Interaction Score")]
    public int CorrectScore = 5; // Score for correct actions in non-hazard interactions

    private PlayerInteractionEffects interactionEffects;
    private PlayerInteractionHideShow hideShow;
    private PlayerInteractionListManager listManager; // Reference to the List Manager

    public int InteractionStage { get; private set; } = 0;  // Public getter for the interaction stage, starts at 0
    
    private bool playerInsideTrigger = false; // Keep track of player presence for this specific gameobject
    private bool scoreAdded = false; // To prevent double counting of the score

    private void Awake()
    {
        interactionEffects = GetComponent<PlayerInteractionEffects>();
        hideShow = GetComponent<PlayerInteractionHideShow>();
        listManager = GetComponent<PlayerInteractionListManager>(); // Get reference to the List Manager
    }

    private void Start()
    {
        UpdateScoreText();
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
            InteractionStage = 0; // Reset interaction stage when exiting the trigger
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
        interactionEffects.ShowTickMark(); // Show tick mark

        // Add score only once to the global total score
        if (!scoreAdded)
        {
            AddScore(CorrectScore); // Increase score
            scoreAdded = true; // Mark score as added to prevent multiple additions
        }

        FinalizeInteraction();
    }

    private void FinalizeInteraction()
    {
        InteractionStage = 2; // Mark Interaction as complete for this specific object
        interactionEffects.StopHighlight(); // Stop the highlight effect
        UpdateScoreText();

        // Use the List Manager to add the item to the correct panel based on the InteractionType
        if (listManager != null)
        {
            bool isCorrect = true; // Non-hazard interactions are always correct
            listManager.AddItemToList(Type, isCorrect); // Pass 'isCorrect' as true for non-hazard interactions
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
        PlayerInteractionScoreManager.AddScore(points); // Use PlayerInteractionScoreManager to update the total score
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