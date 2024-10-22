using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem_HiVisShirt : MonoBehaviour
{

    public GameObject Shirt;
    public GameObject HiVisShirt;
    public GameObject Pant;
    public GameObject WorkerPant;
    public GameObject InteractionText;
    public AudioSource hiVisShirtAudioSource;

    private bool playerInsideTrigger = false;

    private void Start()
    {
        // Attempt to get the AudioSource component
        if (HiVisShirt != null)
        {
            hiVisShirtAudioSource = HiVisShirt.GetComponent<AudioSource>();

            if (hiVisShirtAudioSource != null)
            {
                Debug.Log("AudioSource found on HiVisShirt GameObject.");
            }
            else
            {
                Debug.LogError("AudioSource component not found on HiVisShirt GameObject.");
            }
        }
        else
        {
            Debug.LogError("HiVisShirt GameObject not assigned.");
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
        if (Shirt != null)
        {
            Shirt.SetActive(!Shirt.activeSelf);
            Debug.Log("Hide Shirt");
        }

        if (HiVisShirt != null)
        {
            HiVisShirt.SetActive(!HiVisShirt.activeSelf);

            if (hiVisShirtAudioSource != null)
            {
                hiVisShirtAudioSource.Play(); // Play the audio
            }

            if (InteractionText != null)
            {
                InteractionText.SetActive(false); // Hide Interaction Text
                Debug.Log("Show HiVisShirt");
            }
        }

        if (Pant != null)
        {
            Pant.SetActive(!Pant.activeSelf);
            Debug.Log("Hide Pant");
        }

        if (WorkerPant != null)
        {
            WorkerPant.SetActive(!WorkerPant.activeSelf);

            if (InteractionText != null)
            {
                InteractionText.SetActive(false); // Hide Interaction Text
                Debug.Log("Show HiVisShirt");
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
    //             if (Shirt != null)
    //             {
    //                 Shirt.SetActive(!Shirt.activeSelf);
    //                 Debug.Log("Hide Shirt");
    //             }
                
    //             if (HiVisShirt != null)
    //             {
    //                 HiVisShirt.SetActive(!HiVisShirt.activeSelf);
    //                 InteractionText.SetActive(false); // Hide Interaction Text
    //                 Debug.Log("Show HiVisShirt");
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
