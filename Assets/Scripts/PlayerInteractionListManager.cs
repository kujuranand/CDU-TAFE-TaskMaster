using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInteractionListManager : MonoBehaviour
{
    [Header("List Parents")]
    public Transform ppeListParent;     // Direct reference to PPE List parent (where items will be added)
    public Transform permitListParent;  // Direct reference to Permit List parent
    public Transform hazardListParent;  // Direct reference to Hazard List parent
    public VerticalLayoutGroup masterListLayoutGroup; // Reference to the Vertical Layout Group of the Master List

    [Header("List Item Prefab")]
    public GameObject listItemPrefab;   // Prefab for the list item (e.g., the Panel with text)

    [Header("Custom List Item Text")]
    public string ListItemText = "New Item"; // Text that will be added to the panel

    [Header("PPE List Settings")]
    public TMP_Text ppeListHeadingText;   // Reference to the PPE List heading text
    public int totalPPE = 5;              // Total number of PPE items in the scene

    [Header("Permit List Settings")]
    public TMP_Text permitListHeadingText;   // Reference to the Permit List heading text
    public int totalPermits = 4;             // Total number of Permits in the scene

    [Header("Hazard List Settings")]
    public TMP_Text hazardListHeadingText;   // Reference to the Hazards List heading text
    public int totalHazards = 6;             // Total number of hazards in the scene

    [Header("Spacing Settings")]
    public float defaultSpacing = 10f;  // Default spacing between items in the Master List

    // Method to add item to the correct list based on the interaction type and whether the answer was correct
    public void AddItemToList(PlayerInteraction.InteractionType type, bool isCorrect)
    {
        Transform targetParent = null;

        // Determine the correct parent based on the interaction type
        switch (type)
        {
            case PlayerInteraction.InteractionType.PPEDisplay:
                targetParent = ppeListParent;
                break;
            case PlayerInteraction.InteractionType.PermitDisplay:
                targetParent = permitListParent;
                break;
            case PlayerInteraction.InteractionType.HazardDisplay:
                targetParent = hazardListParent;
                break;
        }

        if (targetParent != null && listItemPrefab != null)
        {
            // Instantiate a new list item directly under the target parent (PPE, Permits, or Hazards List)
            GameObject newItem = Instantiate(listItemPrefab, targetParent);

            // Set the text of the list item
            TMP_Text itemText = newItem.GetComponentInChildren<TMP_Text>();
            if (itemText != null)
            {
                itemText.text = ListItemText;

                // If the answer was incorrect, apply a strike-through
                if (!isCorrect)
                {
                    itemText.fontStyle |= FontStyles.Strikethrough; // Apply strike-through effect
                }
            }

            // After adding an item, update the heading based on the current count of items
            switch (type)
            {
                case PlayerInteraction.InteractionType.PPEDisplay:
                    UpdatePPEHeading();
                    break;
                case PlayerInteraction.InteractionType.PermitDisplay:
                    UpdatePermitHeading();
                    break;
                case PlayerInteraction.InteractionType.HazardDisplay:
                    UpdateHazardHeading();
                    break;
            }

            // Reset the spacing of the Master List layout group to default
            ResetMasterListSpacing();
        }
        else
        {
            Debug.LogError("Target parent or ListItemPrefab is not assigned or invalid.");
        }
    }

    // Method to update the PPE list heading text and count of items
    private void UpdatePPEHeading()
    {
        int ppeCompleted = ppeListParent.childCount - 1; // Exclude the heading from the count

        if (ppeCompleted > totalPPE)
        {
            ppeCompleted = totalPPE;
        }

        if (ppeListHeadingText != null)
        {
            ppeListHeadingText.text = $"PPE List ({ppeCompleted}/{totalPPE})";
        }
        else
        {
            Debug.LogError("PPE list heading text is not assigned.");
        }
    }

    // Method to update the Permit list heading text and count of items
    private void UpdatePermitHeading()
    {
        int permitsCompleted = permitListParent.childCount - 1; // Exclude the heading from the count

        if (permitsCompleted > totalPermits)
        {
            permitsCompleted = totalPermits;
        }

        if (permitListHeadingText != null)
        {
            permitListHeadingText.text = $"Permit List ({permitsCompleted}/{totalPermits})";
        }
        else
        {
            Debug.LogError("Permit list heading text is not assigned.");
        }
    }

    // Method to update the Hazard list heading text and count of hazards
    private void UpdateHazardHeading()
    {
        int hazardsCompleted = hazardListParent.childCount - 1; // Exclude the heading from the count

        if (hazardsCompleted > totalHazards)
        {
            hazardsCompleted = totalHazards;
        }

        if (hazardListHeadingText != null)
        {
            hazardListHeadingText.text = $"Hazards List ({hazardsCompleted}/{totalHazards})";
        }
        else
        {
            Debug.LogError("Hazard list heading text is not assigned.");
        }
    }

    // Method to reset the spacing of the Master List to the default value
    private void ResetMasterListSpacing()
    {
        if (masterListLayoutGroup != null)
        {
            masterListLayoutGroup.spacing = defaultSpacing;

            // Force the layout group to rebuild to ensure the spacing takes effect immediately
            LayoutRebuilder.ForceRebuildLayoutImmediate(masterListLayoutGroup.GetComponent<RectTransform>());
        }
        else
        {
            Debug.LogError("Master List VerticalLayoutGroup is not assigned.");
        }
    }

    // Check if all lists are empty
    public bool ListsAreEmptyCheck()
    {
        return ppeListParent.childCount == 1 && permitListParent.childCount == 1 && hazardListParent.childCount == 1;
    }

    // Check if PPE list is full
    public bool PPEListFullCheck()
    {
        return ppeListParent.childCount - 1 >= totalPPE;
    }

    // Check if Permit list is full
    public bool PermitsListFullCheck()
    {
        return permitListParent.childCount - 1 >= totalPermits;
    }

    // Check if Hazard list is full
    public bool HazardsListFullCheck()
    {
        return hazardListParent.childCount - 1 >= totalHazards;
    }
}
