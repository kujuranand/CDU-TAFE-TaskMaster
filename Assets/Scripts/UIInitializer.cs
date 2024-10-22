using UnityEngine;
using UnityEngine.UI;

public class UIInitializer : MonoBehaviour
{
    public RectTransform[] layoutGroups; // Assign the layout groups in the Inspector (e.g., the parent objects of your lists)

    void Start()
    {
        // Force layout rebuild for each layout group
        foreach (RectTransform layoutGroup in layoutGroups)
        {
            if (layoutGroup != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
            }
        }
    }
}
