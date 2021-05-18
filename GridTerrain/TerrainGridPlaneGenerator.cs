using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Automatically add component if essential component is missing
[RequireComponent(typeof(Terrain))]

//Wraps a grid mesh to a terrain based on the height maps of it
public class TerrainGridPlaneGenerator : GridPlaneGenerator
{
    private Terrain activeTerrain = null;

    // Start is called before the first frame update
    void Start()
    {
        //Get the active terrain component
        activeTerrain = this.GetComponent<Terrain>();

        //Throw exception if no terrain was found/attached
        if(activeTerrain==null)
        {
            throw new System.Exception(new MissingComponentException().ToString());
        }

        //Update generators information based on its dimensions and height
        GetTerrainData();

        //Instantiate the grid tiles acording to the terrain size and position
        InstantiateGridTiles();

        //Updates the grid tiles vertices based on the height map of the terrain
        UpdateGridTileHeight();
    }

    //Gets relevant terrain data for the grid generation
    private void GetTerrainData()
    {
        //Sets the terrain data for ease of use
        TerrainData terrainData = activeTerrain.terrainData;

        //Updates the x and z size of the grid
        xSize = Mathf.FloorToInt(terrainData.size.x);
        zSize = Mathf.FloorToInt(terrainData.size.z);
    }

    private void UpdateGridTileHeight()
    {
        //Iterate over each tile to update their height
        foreach (GameObject tile in gridTiles)
        {
            //Pass through terrain data for each grid tile to update their vertice height
            tile.GetComponent<GridTile>().UpdateVerticeHeight(ref activeTerrain);
        }

        //Debug that tile height was updated
        DebugManager.DebugLog("Updated Tile Height");
    }

}
