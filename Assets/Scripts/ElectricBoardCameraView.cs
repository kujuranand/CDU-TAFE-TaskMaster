using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElectricBoardCameraView : MonoBehaviour
{
    public GameObject ElectricBoardCamera;
    public GameObject InteractionText;
    public GameObject Player;

    private bool playerInsideTrigger = false;
    private bool cameraViewActive = false;

    // Start is called before the first frame update
    void Start()
    {
        ElectricBoardCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Camera Switching on E Key press
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (cameraViewActive)
            {
                ExitCamera();
            }
            else
            {
                EnterCamera();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            InteractionText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            InteractionText.SetActive(false);
        }
    }

    private void EnterCamera()
    {
        Debug.Log("Switching Camera Views");
        InteractionText.SetActive(false);
        ElectricBoardCamera.SetActive(true);
        // Used for getting the user out of the electric board camera
        cameraViewActive = true;
        // Disable player movement by disabling the player controller
        Player.GetComponent<ThirdPersonController>().enabled = false;
    }

    private void ExitCamera()
    {
        Debug.Log("Switching Camera Views");
        // The interaction text is there but hidden becuase of the FOV of the camera.
        InteractionText.SetActive(true);
        ElectricBoardCamera.SetActive(false);
        cameraViewActive = false;
        // Reset the player controller
        Player.GetComponent<ThirdPersonController>().enabled = true;
    }
}
