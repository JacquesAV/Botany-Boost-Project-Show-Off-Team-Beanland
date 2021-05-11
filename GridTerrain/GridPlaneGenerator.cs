using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlaneGenerator : MonoBehaviour
{
    //Decides the number of tiles across the x and z axis
    public int xSize = 10;
    public int zSize = 10;

    //Prefab of the grid tile
    public GameObject gridTile=null;

    //List of each grid tile that gets created
    private List<GameObject> gridTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Construct tile data list
        ConstructTileDataList();

        //Instantiate separate tile objects
        InstantiateGridTiles();
    }

    //Construct tile data list
    private void ConstructTileDataList()
    {
        //Populate list of grid tiles using the prefab based on dimensions of grid
        for (int tiles = 0; tiles < zSize * xSize; tiles++)
        {
            gridTiles.Add(gridTile);
        }
    }

    //Instantiate separate tile objects
    private void InstantiateGridTiles()
    {
        //Loops over each axis in order to get the correct position shift of each tile based on their expected position
        //Loop over z axis 
        for (int tile = 0, z = 0; z < zSize; z++)
        {
            //Loop over x axis
            for (int x = 0; x < xSize; x++)
            {
                //Construct position shift vector
                Vector3 shift = new Vector3(x,0,z);

                //Instantiate grid tile
                Instantiate(gridTiles[tile], this.gameObject.transform.position + shift, Quaternion.identity, this.gameObject.transform);
                
                //Command the tile to instantiate itself correctly
                gridTiles[tile].GetComponent<GridTile>().InstantiateTile();
            }

            //Move onto the next tile
            tile++;
        }
    }
}
