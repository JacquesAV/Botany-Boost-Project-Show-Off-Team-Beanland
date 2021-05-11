using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Prevent script from working if essential component is missing
[RequireComponent(typeof(MeshFilter))]
public class GridTile : MonoBehaviour
{
    //Active mesh of the grid tile
    private Mesh tileMesh = null;

    //Arrays are used as they are fixed and should not change

    //Tracks the vertices of the quad grid tile
    private Vector3[] vertices = new Vector3[4]
    {
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(1, 0, 0),
        new Vector3(1, 0, 1)
    };

    //Tracks the triangles of the quad grid tile
    private int[] triangles = new int[6]
    {
        0, 1, 2,
        1, 3, 2
    };

    //Start is called before the first frame update
    void Start()
    {
        InstantiateTile();
    }

    public void InstantiateTile()
    {
        //Create a new mesh filter to construct the tile with
        tileMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = tileMesh;

        //Update the mesh with the correct information
        UpdateMeshGrid();
    }

    private void UpdateMeshGrid()
    {
        //Cleans the mesh
        tileMesh.Clear();

        //Updates to the correctly assosiated information
        tileMesh.vertices = vertices;
        tileMesh.triangles = triangles;

        //Recalculates normals
        tileMesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        //Do not continue if vertices are not set yet
        if (tileMesh == null) { return; }

        //Draw the vertice for viewing/testing purposes
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(this.transform.position + tileMesh.vertices[i], new Vector3(0.1f, 0.1f, 0.1f));
        }
    }
     
    //Updates the vertices with a given array of vertices
    public void UpdateVertices(Vector3[] NewVertices)
    {
        vertices = NewVertices;
    }
    //Updates the triangles with a given array of ints
    public void UpdateTriangles(int[] newTriangles)
    {
        triangles = newTriangles;
    }
 
}
