using UnityEngine;
using TMPro;

public class PlayerInteractionHazardListDisplay : MonoBehaviour
{
    private TextMeshProUGUI listText;
    public RectTransform backgroundRect;
    public Vector2 padding = new Vector2(10f, 10f);
    public string listHeading = "Hazard List"; // Heading for the Hazard list

    private string listContent = "";

    private void Awake()
    {
        listContent = ""; // Reset the list content when the game starts
    }

    private void Start()
    {
        listText = GetComponent<TextMeshProUGUI>();

        if (listText == null)
        {
            Debug.LogError("TextMeshProUGUI component is not found on this GameObject!", gameObject);
        }

        if (backgroundRect == null)
        {
            Debug.LogError("Background RectTransform is not assigned!", gameObject);
        }

        // Initialize the list display
        UpdateList();
    }

    public void AddHazardItem(string item)
    {
        if (listText == null) return;

        listContent += "- " + item + "\n";
        UpdateList();
    }

    private void UpdateList()
    {
        listText.text = listHeading + "\n\n" + listContent;
        listText.ForceMeshUpdate();
        AdjustBackgroundSize();
    }

    private void AdjustBackgroundSize()
    {
        if (backgroundRect != null && listText != null)
        {
            Vector2 textSize = listText.GetRenderedValues(false);
            backgroundRect.sizeDelta = textSize + padding; // Adjust background size dynamically based on text
        }
    }
}
