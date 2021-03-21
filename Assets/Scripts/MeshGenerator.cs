using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    [Header("World Config:")]
    public int worldX;
    public int worldZ;

    private Mesh mesh;
    private int[] triangles;
    private Vector3[] vertices;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateMesh();
        UpdateMesh();
    }

    private void GenerateMesh()
    {
        // Generate triangle array
        triangles = new int[worldX * worldZ * 6];
        vertices = new Vector3[(worldX + 1) * (worldZ + 1)];

        // Loop through our x/z done in reverse because reverse quads
        for (int i = 0, z = 0; z <= worldZ; z++)
        {
            for (int x = 0; x <= worldX; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }

        }

        int tris = 0;
        int verts = 0;

        for (int z = 0; z < worldZ; z++)
        {
            for (int x = 0; x < worldX; x++)
            {
                // triangle 1
                triangles[tris + 0] = verts + 0;
                triangles[tris + 1] = verts + worldZ + 1;
                triangles[tris + 2] = verts + 1;

                // triangle 2
                triangles[tris + 3] = verts + 1;
                triangles[tris + 4] = verts + worldZ + 1;
                triangles[tris + 5] = verts + worldZ + 2;

                verts++;
                tris += 6;
            }
            verts++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
