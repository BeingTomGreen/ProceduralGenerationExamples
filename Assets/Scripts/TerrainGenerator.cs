using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("World Config:")]
    public GameObject terrainBlock;
    public int worldSizeX = 40;
    public int worldSizeZ = 40;

    [Header("Height Noise Config:")]
    public int noiseHeightModifier = 5;
    public float detailScale = 8f;

    private List<Vector3> blockPositions = new List<Vector3>();
    private float gridOffset = 1f;

    [Header("Object Config:")]
    public GameObject objectPrefab;
    public int objectCount;
    private float objectSpawnHeightOffset = .5f;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < worldSizeX; x++)
        {
            for (int z = 0; z < worldSizeZ; z++)
            {
                // Calulate position of new block
                Vector3 position = new Vector3(x * gridOffset, generateHeightNoise(x, z) , z * gridOffset);

                // Create new block
                _ = Instantiate(terrainBlock, position, Quaternion.identity, transform);

                // Add the new blocks position our list
                blockPositions.Add(position);
            }
        }

        SpawnObjects();
    }

    private float generateHeightNoise(int x, int z)
    {
        float noiseX = (x + this.transform.position.x) / detailScale;
        float noiseZ = (z + this.transform.position.z) / detailScale;

        return Mathf.PerlinNoise(noiseX, noiseZ) * noiseHeightModifier;
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 position = GetObjectSpawnLocation();
            _ = Instantiate(objectPrefab, position, Quaternion.identity, transform);
        }
    }

    private Vector3 GetObjectSpawnLocation()
    {
        // Grab a random index
        int rndIndex = Random.Range(0, blockPositions.Count);

        // Generate a new spawn position
        Vector3 spawnLocation = new Vector3(blockPositions[rndIndex].x, blockPositions[rndIndex].y + objectSpawnHeightOffset, blockPositions[rndIndex].z);

        // Remove this block from the list to prevent dupes
        blockPositions.RemoveAt(rndIndex);

        return spawnLocation;
    }
}
