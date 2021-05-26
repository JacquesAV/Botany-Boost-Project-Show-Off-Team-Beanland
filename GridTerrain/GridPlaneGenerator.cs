using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridPlaneGenerator : MonoBehaviour
{
    //Prefab of the grid tile
    public GameObject gridTile = null;

    //Decides the number of tiles across the x and z axis
    public int xSize = 0, zSize = 0;

    //List of each grid tile that gets created
    protected private List<GameObject> gridTiles = new List<GameObject>();

    //Array of coordinates, will store the index of the tiles created, allows for easier neighbour fetching/calculating
    protected private int[,] gridCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate separate tile objects
        InstantiateGridTiles();
    }

    //Returns a reference of the grid tiles, allowing for dynamic changes from other classes
    public ref List<GameObject> GetGridTilesReference()
    {
        return ref gridTiles;
    }

    public List<TileBuildingModel> GetGridTileNeighbours(GameObject sourceTile, Vector2 dimensions)
    {
        //Temporary list
        List<TileBuildingModel> tileBuildingNeighbours = new List<TileBuildingModel>();

        //Get the source tile coordinates
        int[] sourceCoordinates = sourceTile.GetComponent<GridTile>().GetCoordinates();

        //Useful in tracking wether dimensions are negative of postive, important for orientation and the forloop
        bool xNeg = false;
        bool zNeg = false;

        //Check if x dimension is negative
        if (dimensions.x < 0)
        {
            dimensions.x *= -1;
            xNeg = true;
        }
        //Check if z dimension is negative
        if (dimensions.y < 0)
        {
            dimensions.y *= -1;
            zNeg = true;
        }

        //Get tiles based on the source tile and dimensions
        //Uses a try catch statement in order to avoid errors when getting a non existant index
        //Returning empty to represent failure
        try 
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                for (int z = 0; z < dimensions.y; z++)
                {
                    //Temporary int array of neighbour coordinates, starting from the source tile
                    int[] neighbourCoordinates = new int[] { sourceCoordinates[0], sourceCoordinates[1] };

                    //If x axis is negative, move backwards instead of forwards
                    if (xNeg) { neighbourCoordinates[0] -= x; }
                    else { neighbourCoordinates[0] += x; }

                    //If z axis is negative, move backwards instead of forwards
                    if (zNeg) { neighbourCoordinates[1] -= z; }
                    else { neighbourCoordinates[1] += z; }

                    //Get the index of the grid tile
                    int index = gridCoordinates[neighbourCoordinates[0], neighbourCoordinates[1]];

                    //Add the relevant index tile
                    tileBuildingNeighbours.Add(gridTiles[index].GetComponent<TileBuildingModel>());
                }
            }
        }
        catch
        {
            return null;
        }

        //Return temporary list of neighbours
        return tileBuildingNeighbours;
    }

    //Instantiate separate tile objects
    protected private void InstantiateGridTiles()
    {
        //Ensures non null prefab was supplied
        if (gridTile == null)
        {
            throw new System.ArgumentNullException();
        }

        //Ensures no invalid sizes are give
        if (xSize <= 0 || zSize <= 0)
        {
            throw new System.Exception(new System.ArgumentOutOfRangeException().ToString());
        }

        //Instantiate the grid as a 2D array to represent coordinates
        gridCoordinates = new int[xSize, zSize];

        //Loops over each axis in order to get the correct position shift of each tile based on their expected position
        //Loop over z axis 
        for (int tile = 0, z = 0; z < zSize; z++)
        {
            //Loop over x axis
            for (int x = 0; x < xSize; x++)
            {
                //Construct position shift vector
                Vector3 shift = new Vector3(x, 0, z);

                //Instantiate grid tile
                GameObject gridTileObject = Instantiate(gridTile, this.gameObject.transform.position + shift, Quaternion.identity, this.gameObject.transform);
               
                //Command the tile to instantiate itself correctly with coordinate values
                gridTileObject.GetComponent<GridTile>().InstantiateTile(new int[]{x,z});

                //Set the coordinates of the grid index value
                gridCoordinates[x, z] = tile;

                //Rename the tile for ease of reading and use
                gridTileObject.name = "GridTile " + (tile + 1) + " (Row " + (z + 1) + ", Column " + (x + 1) + ")";

                //Add the object to a list for easy access and modification
                gridTiles.Add(gridTileObject);

                //Move onto the next tile
                tile++;
            }
        }

        //Debug log the number of generated tiles
        DebugManager.DebugLog("Generated " + gridTiles.Count + " grid tiles.");
    }
}
