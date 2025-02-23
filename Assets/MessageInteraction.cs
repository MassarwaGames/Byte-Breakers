using UnityEngine;

/// <summary>
/// Allows the player to interact with the radio to play a special message.
/// </summary>
public class MessageInteraction : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource messageAudio; // Assign the judges' message audio

    [Header("Dependencies")]
    [SerializeField] private SubtitleManager subtitleManager; // Assign the SubtitleManager for synced subtitles

    private bool isNearItem = false;

    private void Update()
    {
        if (isNearItem && Input.GetKeyDown(KeyCode.F))
        {
            PlayMessage();
        }
    }

    /// <summary>
    /// Plays the judges' message when interacted with.
    /// </summary>
    private void PlayMessage()
    {
        if (messageAudio != null && !messageAudio.isPlaying)
        {
            messageAudio.Play();
            subtitleManager?.StartSubtitles("JudgesMessage");
            Debug.Log("Playing message for the competition judges.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearItem = true;
            Debug.Log("Press 'F' to play the message.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearItem = false;
        }
    }
}
