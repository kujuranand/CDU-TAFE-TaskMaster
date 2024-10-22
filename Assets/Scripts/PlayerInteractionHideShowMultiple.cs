using UnityEngine;

public class PlayerInteractionHideShowMultiple : MonoBehaviour
{
    [Header("Object 1")]
    public GameObject objectToAppear1;
    public GameObject objectToHide1;

    [Header("Object 2")]
    public GameObject objectToAppear2;
    public GameObject objectToHide2;

    public void HandleFirstAppearance()
    {
        if (objectToAppear1 != null)
        {
            objectToAppear1.SetActive(true);
        }

        if (objectToHide1 != null)
        {
            objectToHide1.SetActive(false);
        }
    }

    public void HandleSecondAppearance()
    {
        if (objectToAppear2 != null)
        {
            objectToAppear2.SetActive(true);
        }

        if (objectToHide2 != null)
        {
            objectToHide2.SetActive(false);
        }
    }
}