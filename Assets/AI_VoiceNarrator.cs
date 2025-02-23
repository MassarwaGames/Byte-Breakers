using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the AI voice narration at the start of the game.
/// Triggers synchronized subtitles when narration starts.
/// </summary>
public class AI_VoiceNarrator : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource voiceNarration; // Assign the AI voice narration audio source

    [Header("Dependencies")]
    [SerializeField] private SubtitleManager subtitleManager; // Assign the SubtitleManager to display synchronized subtitles

    private void Start()
    {
        StartCoroutine(PlayIntroVoice());
    }

    /// <summary>
    /// Delays slightly before playing the AI voice and triggering subtitles.
    /// </summary>
    private IEnumerator PlayIntroVoice()
    {
        yield return new WaitForSeconds(1f); // Delay for dramatic effect
        voiceNarration?.Play();
        subtitleManager?.StartSubtitles("Welcome"); // Trigger subtitles
    }
}
