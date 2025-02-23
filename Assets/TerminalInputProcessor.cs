using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

/// <summary>
/// Processes the player's input in the terminal, validates using regex,
/// plays success/failure sounds, and triggers the world transformation.
/// </summary>
public class TerminalInputProcessor : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField inputField; // Input field where the player types code
    [SerializeField] private GameObject inputUI; // Terminal UI panel

    [Header("Audio")]
    [SerializeField] private AudioSource successSound; // Sound for correct input
    [SerializeField] private AudioSource failSound; // Sound for incorrect input
    [SerializeField] private AudioSource aiVoice; // AI congratulatory voice

    [Header("Dependencies")]
    [SerializeField] private TerrainFadeEffect terrainFadeEffect; // Handles world transformation
    [SerializeField] private SubtitleManager subtitleManager; // Displays subtitles in sync with AI voice

    private readonly string validCommandPattern = @"^\s*print\s*\(\s*""Hello,\s*World!""\s*\)\s*;\s*$";

    private void Start()
    {
        // Ensure Terminal UI is hidden at the start
        inputUI.SetActive(false);
    }

    private void Update()
    {
        // Allow player to close the terminal with Escape key
        if (inputUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTerminal();
        }

        // Allow submitting input using Enter key
        if (inputUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            OnSubmit();
        }
    }

    /// <summary>
    /// Called when the player submits code in the terminal.
    /// </summary>
    public void OnSubmit()
    {
        string input = inputField.text.Trim();

        if (Regex.IsMatch(input, validCommandPattern, RegexOptions.IgnoreCase))
        {
            HandleSuccess();
        }
        else
        {
            HandleFailure();
        }
    }

    /// <summary>
    /// Handles correct command input, triggering world transformation.
    /// </summary>
    private void HandleSuccess()
    {
        Debug.Log("Correct command entered! Transitioning world...");
        successSound?.Play();
        aiVoice?.Play();
        subtitleManager?.StartSubtitles("Success");
        terrainFadeEffect?.StartTerrainTransition();
        CloseTerminal();
    }

    /// <summary>
    /// Handles incorrect command input, playing failure sound.
    /// </summary>
    private void HandleFailure()
    {
        Debug.Log("Incorrect command. Try again.");
        failSound?.Play();
    }

    /// <summary>
    /// Closes the terminal UI.
    /// </summary>
    private void CloseTerminal()
    {
        inputUI.SetActive(false);
    }
}
