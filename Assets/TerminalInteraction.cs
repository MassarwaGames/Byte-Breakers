using UnityEngine;
using TMPro;

/// <summary>
/// Allows the player to interact with the terminal by pressing 'F'.
/// </summary>
public class TerminalInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject inputUI; // The terminal UI panel
    [SerializeField] private TMP_InputField inputField; // The input field for code submission

    private bool isNearTerminal = false;

    private void Start()
    {
        // Hide the terminal UI at game start
        inputUI.SetActive(false);
    }

    private void Update()
    {
        // Only allow terminal interaction when the player is close and presses F
        if (isNearTerminal && Input.GetKeyDown(KeyCode.F))
        {
            ToggleTerminal(true);
        }

        // Allow closing the terminal with Escape key
        if (inputUI.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleTerminal(false);
        }
    }

    /// <summary>
    /// Opens or closes the terminal UI.
    /// </summary>
    /// <param name="state">True to open, false to close.</param>
    private void ToggleTerminal(bool state)
    {
        inputUI.SetActive(state);

        if (state)
        {
            inputField.text = ""; // Clear previous input
            inputField.ActivateInputField(); // Auto-focus on input field
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearTerminal = true;
            Debug.Log("Press 'F' to interact with the terminal.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearTerminal = false;
        }
    }
}
