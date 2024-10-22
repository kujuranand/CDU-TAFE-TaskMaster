using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem_Helmet : MonoBehaviour
{
    public GameObject Cap;
    public GameObject Helmet;
    public GameObject InteractionText;
    public AudioSource helmetAudioSource;

    private bool playerInsideTrigger = false;

    private void Start()
    {
        // Attempt to get the AudioSource component
        if (Helmet != null)
        {
            helmetAudioSource = Helmet.GetComponent<AudioSource>();

            if (helmetAudioSource != null)
            {
                Debug.Log("AudioSource found on Helmet GameObject.");
            }
            else
            {
                Debug.LogError("AudioSource component not found on Helmet GameObject.");
            }
        }
        else
        {
            Debug.LogError("Helmet GameObject not assigned.");
        }
    }

    private void Update()
    {
        // Check for key press in the Update method
        if (playerInsideTrigger && Keyboard.current.eKey.wasPressedThisFrame)
        {
            OnEKeyPressed();
        }
    }

    private void OnEKeyPressed()
    {
        // Handle the logic when "E" key is pressed
        if (Cap != null)
        {
            Cap.SetActive(!Cap.activeSelf);
            Debug.Log("Hide Cap");
        }

        if (Helmet != null)
        {
            Helmet.SetActive(!Helmet.activeSelf);

            if (helmetAudioSource != null)
            {
                helmetAudioSource.Play(); // Play the audio
            }

            if (InteractionText != null)
            {
                InteractionText.SetActive(false); // Hide Interaction Text
                Debug.Log("Show Helmet");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;

            //Debug.Log("Entered Trigger");
            if (InteractionText != null)
                {
                    InteractionText.SetActive(!InteractionText.activeSelf);
                    Debug.Log("Show Press E");
                }
        }
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         //Debug.Log("Staying in Trigger");
    //         if (Keyboard.current.eKey.wasPressedThisFrame)
    //         {
    //             if (Cap != null)
    //             {
    //                 Cap.SetActive(!Cap.activeSelf);
    //                 Debug.Log("Hide Cap");
    //             }
                
    //             if (Helmet != null)
    //             {
    //                 Helmet.SetActive(!Helmet.activeSelf);
    //                 InteractionText.SetActive(false); // Hide Interaction Text
    //                 Debug.Log("Show Helmet");
    //             }
    //         }
    //     }
    // }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            
            //Debug.Log("Exited Trigger");
            if (InteractionText != null)
                {
                    InteractionText.SetActive(false); // Hide Interaction Text
                    Debug.Log("Hide Press E");
                }
        }
    }

}
