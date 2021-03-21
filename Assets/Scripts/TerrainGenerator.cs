using System.Collections;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("World Config:")]
    public GameObject terrainBlock;
    public int blockSize;
    public int worldSizeX;
    public int worldSizeZ;

    [Header("Height Noise Config:")]
    public int noiseHeightModifier;
    public float detailScale;

    [Header("Player Config:")]
    public GameObject player;
    private Vector3 startPosition;

    private Hashtable blockContainer = new Hashtable();


    // Start is called before the first frame update
    private void Start()
    {
        for (int x = -worldSizeX; x < worldSizeX; x++)
        {
            for (int z = -worldSizeZ; z < worldSizeZ; z++)
            {
                // Calulate position of new block
                Vector3 position = new Vector3(x * blockSize + startPosition.x, generateHeightNoise(x, z), z * blockSize + startPosition.z);

                // Create new block
                GameObject block = Instantiate(terrainBlock, position, Quaternion.identity, transform);

                // Add the new block to our hash table
                blockContainer.Add(position, block);
            }
        }
    }

    private void Update()
    {
        // Is distance player has moved greater than chunkContainer?
        if (Mathf.Abs(playerMoveOffsetX) >= blockSize || Mathf.Abs(playerMoveOffsetZ) >= blockSize)
        {
            for (int x = -worldSizeX; x < worldSizeX; x++)
            {
                for (int z = -worldSizeZ; z < worldSizeZ; z++)
                {
                    // Calulate position of new block
                    Vector3 position = new Vector3(
                        x * blockSize + playerLocationX,
                        generateHeightNoise(x + playerLocationX, z + playerLocationZ),
                        z * blockSize + playerLocationZ
                    );

                    // Check that block doesn't already exist
                    if (!blockContainer.ContainsKey(position))
                    {
                        // Create new block
                        GameObject block = Instantiate(terrainBlock, position, Quaternion.identity, transform);

                        // Add the new block to our hash table
                        blockContainer.Add(position, block);
                    }
                }
            }
        }
    }

    private int playerMoveOffsetX
    {
        get
        {
            return (int)(player.transform.position.x - startPosition.x);
        }
    }
    private int playerMoveOffsetZ
    {
        get
        {
            return (int)(player.transform.position.z - startPosition.z);
        }
    }

    private int playerLocationX
    {
        get
        {
            return (int)Mathf.Floor(player.transform.position.x);
        }
    }

    private int playerLocationZ
    {
        get
        {
            return (int)Mathf.Floor(player.transform.position.z);
        }
    }


    private float generateHeightNoise(int x, int z)
    {
        float noiseX = (x + this.transform.position.x) / detailScale;
        float noiseZ = (z + this.transform.position.z) / detailScale;

        return Mathf.PerlinNoise(noiseX, noiseZ) * noiseHeightModifier;
    }

}
