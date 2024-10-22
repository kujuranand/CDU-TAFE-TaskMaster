using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    public RectTransform backgroundRect;
    public Vector2 padding = new Vector2(10f, 10f);
    public string initialScoreText = "Score: 0"; // Set an initial score text

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();

        if (scoreText == null)
        {
            Debug.LogError("TextMeshProUGUI component is not found on this GameObject!", gameObject);
        }

        if (backgroundRect == null)
        {
            Debug.LogError("Background RectTransform is not assigned!", gameObject);
        }

        // Ensure the background is visible from the start
        if (backgroundRect != null)
        {
            backgroundRect.gameObject.SetActive(true); // Show background by default
        }

        // Set the initial score text
        UpdateScore(0);
    }

    public void UpdateScore(int score)
    {
        if (scoreText == null) return;

        // Update the score text
        scoreText.text = "Score: " + score;
        scoreText.ForceMeshUpdate();
        AdjustBackgroundSize();
    }

    private void AdjustBackgroundSize()
    {
        if (backgroundRect != null && scoreText != null)
        {
            Vector2 textSize = scoreText.GetRenderedValues(false);
            backgroundRect.sizeDelta = textSize + padding; // Adjust background size based on score text
        }
    }
}
