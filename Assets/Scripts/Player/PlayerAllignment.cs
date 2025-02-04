using UnityEngine;

public class PlayerAlignment : MonoBehaviour
{
    void Start()
    {
        AlignToTerrain();
    }

    private void AlignToTerrain()
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            Vector3 playerPosition = transform.position;
            float terrainHeight = terrain.SampleHeight(playerPosition) + terrain.GetPosition().y;
            transform.position = new Vector3(playerPosition.x, terrainHeight, playerPosition.z);
        }
    }
}
