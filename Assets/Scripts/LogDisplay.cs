using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogDisplay : MonoBehaviour
{
    private TextMeshProUGUI logText;
    public RectTransform backgroundRect; // Reference to the background image's RectTransform
    public Vector2 padding = new Vector2(10f, 10f); // Padding around the text

    private void Start()
    {
        // Get the TextMeshPro component
        logText = GetComponentInChildren<TextMeshProUGUI>(); // Adjusted to find the child component

        // Ensure the backgroundRect is set
        if (backgroundRect == null)
        {
            Debug.LogError("Background RectTransform is not assigned!");
        }

        // Hide the background by default
        backgroundRect.gameObject.SetActive(false);
    }

    public void ShowLog(string message)
    {
        // Update the text with the log message
        logText.text = message;

        // Force TextMeshPro to update its internal values before calculating the size
        logText.ForceMeshUpdate();

        // Adjust the background size based on the text size
        AdjustBackgroundSize();

        // Show the background
        backgroundRect.gameObject.SetActive(true);
    }

    public void HideLog()
    {
        // Clear the text to hide the log message
        logText.text = "";

        // Hide the background
        backgroundRect.gameObject.SetActive(false);
    }

    private void AdjustBackgroundSize()
    {
        // Adjust the background size based on the text content
        Vector2 textSize = logText.GetRenderedValues(false);
        backgroundRect.sizeDelta = textSize + padding;
    }
}
