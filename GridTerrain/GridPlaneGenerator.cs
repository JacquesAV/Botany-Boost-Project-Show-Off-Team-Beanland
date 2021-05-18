using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlaneGenerator : MonoBehaviour
{
    //Prefab of the grid tile
    public GameObject gridTile = null;

    //Decides the number of tiles across the x and z axis
    public int xSize = 0, zSize = 0;

    //List of each grid tile that gets created
    protected private List<GameObject> gridTiles = new List<GameObject>();

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

    //Returns a list of neighbouring tiles based on a source tile and dimensions of a placed object
    public List<TileBuildingModel> GetGridTileNeighbours(GameObject sourceTile, Vector2 dimensions)
    {
        //Temporary list
        List<TileBuildingModel> tileBuildingNeighbours = new List<TileBuildingModel>();

        //Source tile index
        int sourceIndex = gridTiles.IndexOf(sourceTile);

        //Get tiles based on the source tile and dimensions
        for(int x=1; x <= dimensions.x; x++)
        { 
            for (int z = 1; z <= dimensions.y; z++)
            {
                //Source index + size
                int neighbourIndex = sourceIndex + (x - 1) + ((z-1)*zSize);

                //Add the relevant index tile
                tileBuildingNeighbours.Add(gridTiles[neighbourIndex].GetComponent<TileBuildingModel>());

                //Color the tile for debugging purposes
                gridTiles[neighbourIndex].GetComponent<Renderer>().material.color = Color.red;

                //Example of a 6x5 grid with a 2x2 object sourcing at index 7
                //24,25,26,27,28,29
                //18,19,20,21,22,23
                //12,C, C, 15,16,17
                //6, S, C, 9, 10,11
                //0, 1, 2, 3, 4, 5

                //S = 7 [2,2] = Source
                //C = 8, 13, 14 = Neighbour
                //C8 [3,2] = sourceIndex + (2-1) + (2-1)
            }
        }

       //tileBuildingNeighbours.Add();

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
        if (xSize <=0 || zSize <= 0)
        {
            throw new System.Exception(new System.ArgumentOutOfRangeException().ToString());
        }

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
               
                //Command the tile to instantiate itself correctly
                gridTileObject.GetComponent<GridTile>().InstantiateTile();

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
