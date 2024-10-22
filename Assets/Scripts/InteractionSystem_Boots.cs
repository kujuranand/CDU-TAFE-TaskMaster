using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem_Boots : MonoBehaviour
{
    public GameObject Thongs;
    public GameObject Boots;
    public GameObject InteractionText;
    public AudioSource bootsAudioSource;

    private bool playerInsideTrigger = false;

    private void Start()
    {
        // Attempt to get the AudioSource component
        if (Boots != null)
        {
            bootsAudioSource = Boots.GetComponent<AudioSource>();

            if (bootsAudioSource != null)
            {
                Debug.Log("AudioSource found on Boots GameObject.");
            }
            else
            {
                Debug.LogError("AudioSource component not found on Boots GameObject.");
            }
        }
        else
        {
            Debug.LogError("Boots GameObject not assigned.");
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
        if (Thongs != null)
        {
            Thongs.SetActive(!Thongs.activeSelf);
            Debug.Log("Hide Thongs");
        }

        if (Boots != null)
        {
            Boots.SetActive(!Boots.activeSelf);

            if (bootsAudioSource != null)
            {
                bootsAudioSource.Play(); // Play the audio
            }

            if (InteractionText != null)
            {
                InteractionText.SetActive(false); // Hide Interaction Text
                Debug.Log("Show Boots");
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
    //             if (Thongs != null)
    //             {
    //                 Thongs.SetActive(!Thongs.activeSelf);
    //                 Debug.Log("Hide Thongs");
    //             }
                
    //             if (Boots != null)
    //             {
    //                 Boots.SetActive(!Boots.activeSelf);
    //                 InteractionText.SetActive(false); // Hide Interaction Text
    //                 Debug.Log("Show Boots");
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
