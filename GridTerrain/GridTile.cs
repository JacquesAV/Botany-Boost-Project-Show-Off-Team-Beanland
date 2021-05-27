using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Automatically add component if essential component is missing
[RequireComponent(typeof(MeshFilter))]
public class GridTile : MonoBehaviour
{
    //Active mesh of the grid tile
    private Mesh tileMesh = null;
    private MeshCollider tileCollider = null;
    private int[] coordinates;

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
    private void Start()
    {
        InstantiateTile();
    }
    public void InstantiateTile(int[] givenCoordinates)
    {
        //Update the coordinates
        coordinates = givenCoordinates;

        //Instantiate normally
        InstantiateTile();
    }

    public void InstantiateTile()
    {
        //Create a new mesh filter to construct the tile with
        tileMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = tileMesh;

        //Get the mesh collider
        tileCollider = this.GetComponent<MeshCollider>();

        //Update the mesh with the correct information
        UpdateMeshGrid();
    }

    public void UpdateMeshGrid()
    {
        //Cleans the mesh
        tileMesh.Clear();

        //Updates to the correctly assosiated information
        tileMesh.vertices = vertices;
        tileMesh.triangles = triangles;

        //Recalculates normals
        tileMesh.RecalculateNormals();

        //Update the mesh collider
        tileCollider.sharedMesh = tileMesh;
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

    //Updates the triangles with a given array of ints
    public void UpdateVerticeHeight(ref Terrain terrain)
    {
        //Draw the vertice for viewing/testing purposes
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = terrain.SampleHeight(this.transform.position + vertices[i]);
        }

        //Update the mesh grid to match the height
        UpdateMeshGrid();
    }

    //Gets the average height of the tile from its original position
    public float GetAverageHeight()
    {
        //Declare temporary variable
        float averageHeight = 0;

        //Add each vertice to the temporary variable
        foreach(Vector3 vertice in vertices)
        {
            averageHeight += vertice.y;
        }

        //Then return the average
        return averageHeight/vertices.Length;
    }
    public int[] GetCoordinates()
    {
        return coordinates;
    }
    public Vector3 GetAverageNormal()
    {
        //Temporary variable
        Vector3 averageNormal = Vector3.zero;

        //Iterate over each
        foreach(Vector3 normal in tileMesh.normals)
        {
            averageNormal += normal;
        }

        //Divide for average
        averageNormal = averageNormal / tileMesh.normals.Length;

        //Return
        return averageNormal;
    }
    private void OnDrawGizmos()
    {
        //Do not continue if vertices are not set yet
        if (tileMesh == null) { return; }

        //Draw the vertice for viewing/testing purposes
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(this.transform.position + tileMesh.vertices[i], new Vector3(0.1f, 0.1f, 0.1f));
        }
    }
}
