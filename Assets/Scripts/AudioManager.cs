using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        // Get the AudioSource component on the button's GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // Method to be called on button click
    public void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource component is missing on the button GameObject.");
        }
    }

    public void PlayCorrectAnswerAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource component is missing on the button GameObject.");
        }
    }

    public void PlayIncorrectAnswerAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource component is missing on the button GameObject.");
        }
    }
}