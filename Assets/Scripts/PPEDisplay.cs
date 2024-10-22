using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PPEDisplay : MonoBehaviour
{
    private TextMeshProUGUI ppeText;
    public RectTransform backgroundRect;
    public Vector2 padding = new Vector2(10f, 10f);
    public string ppeListHeading = "PPE List";

    private string ppeListContent = "";

    private void Awake()
    {
        ppeListContent = ""; // Reset the PPE list content when the game starts
    }

    private void Start()
    {
        ppeText = GetComponent<TextMeshProUGUI>();

        if (ppeText == null)
        {
            Debug.LogError("TextMeshProUGUI component is not found on this GameObject!", gameObject);
        }

        if (backgroundRect == null)
        {
            Debug.LogError("Background RectTransform is not assigned!", gameObject);
        }

        if (backgroundRect != null)
        {
            backgroundRect.gameObject.SetActive(false);
        }

        // Initialize the PPE list display
        UpdatePPEList();
    }

    public void AddPPEItem(string item)
    {
        if (ppeText == null) return;

        ppeListContent += "- " + item + "\n";
        UpdatePPEList();
    }

    private void UpdatePPEList()
    {
        ppeText.text = ppeListHeading + "\n\n" + ppeListContent;
        ppeText.ForceMeshUpdate();
        AdjustBackgroundSize();

        if (backgroundRect != null)
        {
            backgroundRect.gameObject.SetActive(true);
        }
    }

    private void AdjustBackgroundSize()
    {
        if (backgroundRect != null && ppeText != null)
        {
            Vector2 textSize = ppeText.GetRenderedValues(false);
            backgroundRect.sizeDelta = textSize + padding;
        }
    }
}
