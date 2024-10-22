using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerObjectInteractionListManager : MonoBehaviour {
    [Header("List Parents")]
    public Transform ppeListParent;
    public Transform permitListParent;
    public Transform hazardListParent;
    public VerticalLayoutGroup masterListLayoutGroup;

    [Header("List Item Prefab")]
    public GameObject listItemPrefab;

    [Header("PPE List Settings")]
    public TMP_Text ppeListHeadingText;
    public int totalPPE = 5;

    [Header("Permit List Settings")]
    public TMP_Text permitListHeadingText;
    public int totalPermits = 2;

    [Header("Hazard List Settings")]
    public TMP_Text hazardListHeadingText;
    public int totalHazards = 6;

    [Header("Spacing Settings")]
    public float defaultSpacing = 10f;

    private void Awake() {
    }

    public void AddItemToList(PlayerObjectInteractionManager.InteractionType type, bool isCorrect, string addToListText) {
        Transform targetParent = null;

        switch (type) {
            case PlayerObjectInteractionManager.InteractionType.PPEDisplay:
                targetParent = ppeListParent;
                break;
            case PlayerObjectInteractionManager.InteractionType.PermitDisplay:
                targetParent = permitListParent;
                break;
            case PlayerObjectInteractionManager.InteractionType.HazardDisplay:
                targetParent = hazardListParent;
                break;
        }

        if (targetParent != null && listItemPrefab != null) {
            GameObject newItem = Instantiate(listItemPrefab, targetParent);
            TMP_Text itemText = newItem.GetComponentInChildren<TMP_Text>();
            if (itemText != null) {
                itemText.text = addToListText;

                if (!isCorrect && type == PlayerObjectInteractionManager.InteractionType.HazardDisplay) {
                    itemText.fontStyle |= FontStyles.Strikethrough;
                }
            }

            switch (type) {
                case PlayerObjectInteractionManager.InteractionType.PPEDisplay:
                    UpdatePPEHeading();
                    break;
                case PlayerObjectInteractionManager.InteractionType.PermitDisplay:
                    UpdatePermitHeading();
                    break;
                case PlayerObjectInteractionManager.InteractionType.HazardDisplay:
                    UpdateHazardHeading();
                    break;
            }

            ResetMasterListSpacing();
        } else {
            Debug.LogError("Target parent or ListItemPrefab is not assigned or invalid.");
        }
    }

    private void UpdatePPEHeading() {
        int ppeCompleted = ppeListParent.childCount - 1;

        if (ppeCompleted > totalPPE) {
            ppeCompleted = totalPPE;
        }

        if (ppeListHeadingText != null) {
            ppeListHeadingText.text = $"PPE List ({ppeCompleted}/{totalPPE})";
        } else {
            Debug.LogError("PPE list heading text is not assigned.");
        }
    }

    private void UpdatePermitHeading() {
        int permitsCompleted = permitListParent.childCount - 1;

        if (permitsCompleted > totalPermits) {
            permitsCompleted = totalPermits;
        }

        if (permitListHeadingText != null) {
            permitListHeadingText.text = $"Permit List ({permitsCompleted}/{totalPermits})";
        } else {
            Debug.LogError("Permit list heading text is not assigned.");
        }
    }

    private void UpdateHazardHeading() {
        int hazardsCompleted = hazardListParent.childCount - 1;

        if (hazardsCompleted > totalHazards) {
            hazardsCompleted = totalHazards;
        }

        if (hazardListHeadingText != null) {
            hazardListHeadingText.text = $"Hazards List ({hazardsCompleted}/{totalHazards})";
        } else {
            Debug.LogError("Hazard list heading text is not assigned.");
        }
    }

    private void ResetMasterListSpacing() {
        if (masterListLayoutGroup != null) {
            masterListLayoutGroup.spacing = defaultSpacing;
            LayoutRebuilder.ForceRebuildLayoutImmediate(masterListLayoutGroup.GetComponent<RectTransform>());
        } else {
            Debug.LogError("Master List VerticalLayoutGroup is not assigned.");
        }
    }

    public bool ListsAreEmptyCheck() {
        return ppeListParent.childCount <= 1 && permitListParent.childCount <= 1 && hazardListParent.childCount <= 1;
    }

    public bool PPEListFullCheck() {
        return ppeListParent.childCount - 1 >= totalPPE;
    }

    public bool PermitsListFullCheck() {
        return permitListParent.childCount - 1 >= totalPermits;
    }

    public bool HazardsListFullCheck() {
        return hazardListParent.childCount - 1 >= totalHazards;
    }
}
