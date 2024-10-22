using UnityEngine;

public class PlayerInteractionHideShow : MonoBehaviour
{
    [Header("Object Appearance Settings")]
    public GameObject[] objectsToAppear;
    public GameObject[] objectsToHide;

    public void HandleObjectAppearance()
    {
        if (objectsToAppear != null && objectsToAppear.Length > 0)
        {
            foreach (GameObject obj in objectsToAppear)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }

        if (objectsToHide != null && objectsToHide.Length > 0)
        {
            foreach (GameObject obj in objectsToHide)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}