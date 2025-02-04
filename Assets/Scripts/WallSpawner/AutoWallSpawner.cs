using UnityEngine;

/// <summary>
/// Automatically spawns walls around the terrain boundaries at the correct ground level.
/// Uses accurate terrain dimensions to ensure proper alignment.
/// </summary>
public class AutoWallSpawner : MonoBehaviour
{
    [Header("Wall Settings")]
    public GameObject wallPrefab; // Assign your .fbx wall prefab here
    public int segmentCount = 10; // Number of wall segments per side
    public float terrainPadding = 1f; // Small gap from terrain edges

    private Terrain terrain;
    private float terrainMinX, terrainMaxX, terrainMinZ, terrainMaxZ;
    private Vector3 prefabSize;

    void Start()
    {
        terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogError("No Terrain Found! Ensure there's an active terrain in the scene.");
            return;
        }

        // Get terrain boundaries
        Vector3 terrainSize = terrain.terrainData.size;
        Vector3 terrainPosition = terrain.transform.position;

        terrainMinX = terrainPosition.x;
        terrainMaxX = terrainPosition.x + terrainSize.x;
        terrainMinZ = terrainPosition.z;
        terrainMaxZ = terrainPosition.z + terrainSize.z;

        // ✅ Fix: Find the Renderer inside the prefab's children
        if (wallPrefab != null)
        {
            Renderer renderer = wallPrefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                prefabSize = renderer.bounds.size;
            }
            else
            {
                Debug.LogError("No Renderer found in " + wallPrefab.name + ". Ensure the prefab has a Mesh Renderer.");
                return;
            }
        }

        GenerateWalls();
    }

    /// <summary>
    /// Spawns walls around all four edges of the terrain.
    /// </summary>
    void GenerateWalls()
    {
        // Front & Back Walls
        SpawnWall(new Vector3(terrainMinX, 0, terrainMaxZ + terrainPadding), terrainMaxX - terrainMinX, 0);
        SpawnWall(new Vector3(terrainMinX, 0, terrainMinZ - terrainPadding), terrainMaxX - terrainMinX, 0);

        // Left & Right Walls
        SpawnWall(new Vector3(terrainMinX - terrainPadding, 0, terrainMinZ), terrainMaxZ - terrainMinZ, 90);
        SpawnWall(new Vector3(terrainMaxX + terrainPadding, 0, terrainMinZ), terrainMaxZ - terrainMinZ, 90);
    }

    /// <summary>
    /// Spawns a row of wall segments, ensuring alignment with terrain height.
    /// </summary>
    void SpawnWall(Vector3 startPosition, float length, float rotation)
    {
        if (wallPrefab == null) return;

        float segmentSize = prefabSize.x; // Use the original prefab width
        int totalSegments = Mathf.FloorToInt(length / segmentSize); // How many walls fit

        for (int i = 0; i < totalSegments; i++)
        {
            Vector3 spawnPos = startPosition + (rotation == 0 ? Vector3.right : Vector3.forward) * (i * segmentSize);

            // Get the correct ground height
            float terrainHeight = terrain.SampleHeight(spawnPos) + terrain.transform.position.y;

            // ✅ Ensure walls touch the ground exactly
            spawnPos.y = terrainHeight;

            // Spawn the wall segment without scaling
            GameObject wallSegment = Instantiate(wallPrefab, spawnPos, Quaternion.Euler(0, rotation, 0));
        }
    }
}
