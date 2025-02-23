using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Manages subtitles synchronized with AI narration.
/// Displays text in sync with different voice clips.
/// </summary>
public class SubtitleManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI subtitleText; // Subtitle UI text element

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.5f; // Duration for text fading effect

    private void Start()
    {
        subtitleText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Starts displaying subtitles for the given AI narration type.
    /// </summary>
    /// <param name="narrationType">The type of narration (e.g., "Welcome", "Success", "JudgesMessage").</param>
    public void StartSubtitles(string narrationType)
    {
        StartCoroutine(ShowSubtitles(narrationType));
    }

    /// <summary>
    /// Displays subtitles based on the narration type.
    /// </summary>
    private IEnumerator ShowSubtitles(string narrationType)
    {
        subtitleText.gameObject.SetActive(true);

        // Define subtitle lines and their corresponding start times
        string[,] subtitles = GetSubtitlesForType(narrationType);
        if (subtitles == null)
        {
            Debug.LogError("Unknown narration type: " + narrationType);
            yield break;
        }

        for (int i = 0; i < subtitles.GetLength(0); i++)
        {
            subtitleText.text = subtitles[i, 0]; // Set subtitle text
            float delay = float.Parse(subtitles[i, 1]); // Get subtitle timing
            yield return new WaitForSeconds(delay);
            yield return StartCoroutine(FadeTextInAndOut());
        }

        subtitleText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns subtitle lines for the given narration type.
    /// </summary>
    private string[,] GetSubtitlesForType(string narrationType)
    {
        if (narrationType == "Welcome")
        {
            return new string[,]
            {
                {"Ah... so another lost soul has wandered into the abyss...", "0"},
                {"The void is silent, waiting... but you hold the power to change it.", "3"},
                {"Before you lies an ancient terminal... your key to awakening this world.", "7"},
                {"Type the words: print(\"Hello, World!\");", "11"},
                {"Ignite the first spark of life!", "15"}
            };
        }
        else if (narrationType == "Success")
        {
            return new string[,]
            {
                {"YES! The words have been spoken!", "0"},
                {"The ground beneath you awakens, life surges forth!", "2"},
                {"The darkness fades, the sun rises...", "6"},
                {"You have reshaped this world!", "9"}
            };
        }
        else if (narrationType == "JudgesMessage")
        {
            return new string[,]
            {
                {"Hello to the judges and everyone reviewing this project.", "0"},
                {"Due to time constraints, I had to rebuild this game from scratch.", "3"},
                {"I couldn’t complete everything, but I hope this level gives a good impression.", "7"},
                {"Thank you for your time and understanding!", "11"}
            };
        }

        return null;
    }

    /// <summary>
    /// Fades the subtitle text in and out.
    /// </summary>
    private IEnumerator FadeTextInAndOut()
    {
        float elapsedTime = 0f;

        // Fade in
        while (elapsedTime < fadeDuration)
        {
            subtitleText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f); // Keep text visible

        elapsedTime = 0f;
        // Fade out
        while (elapsedTime < fadeDuration)
        {
            subtitleText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
