using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the smooth transition from DarkTerrain to BrightTerrain.
/// </summary>
public class TerrainFadeEffect : MonoBehaviour
{
    [Header("Terrain Objects")]
    [SerializeField] private GameObject darkTerrain; // Assign DarkTerrain
    [SerializeField] private GameObject brightTerrain; // Assign BrightTerrain

    [Header("Audio Settings")]
    [SerializeField] private AudioSource worldMusic; // Assign bright world theme music
    [SerializeField] private AudioSource darkAmbience; // Assign dark world ambient sound

    [Header("Transition Settings")]
    [SerializeField] private float transitionTime = 3f; // Duration of the fade effect

    private Material darkTerrainMaterial; // Material used for fading effect

    private void Start()
    {
        // Ensure DarkTerrain is active and BrightTerrain is hidden at the start
        darkTerrain.SetActive(true);
        brightTerrain.SetActive(false);

        // Retrieve DarkTerrain's material
        Terrain terrainComponent = darkTerrain.GetComponent<Terrain>();

        if (terrainComponent != null)
        {
            darkTerrainMaterial = new Material(Shader.Find("Standard"));
            terrainComponent.materialTemplate = darkTerrainMaterial;
            darkTerrainMaterial.color = new Color(0, 0, 0, 1); // Fully black at start
        }
    }

    /// <summary>
    /// Begins the fading transition from DarkTerrain to BrightTerrain.
    /// </summary>
    public void StartTerrainTransition()
    {
        StartCoroutine(FadeOutDarkTerrain());
    }

    /// <summary>
    /// Smoothly fades out the DarkTerrain and reveals the BrightTerrain.
    /// </summary>
    private IEnumerator FadeOutDarkTerrain()
    {
        float elapsedTime = 0f;

        // Stop dark world ambience
        if (darkAmbience != null)
        {
            darkAmbience.Stop();
        }

        // Play bright world music
        if (worldMusic != null)
        {
            worldMusic.Play();
        }

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionTime);

            // Adjust material transparency
            if (darkTerrainMaterial != null)
            {
                darkTerrainMaterial.color = new Color(0, 0, 0, alpha);
            }

            yield return null;
        }

        // Ensure DarkTerrain is fully disabled and BrightTerrain is visible
        darkTerrain.SetActive(false);
        brightTerrain.SetActive(true);

        Debug.Log("World transformation complete.");
    }
}
