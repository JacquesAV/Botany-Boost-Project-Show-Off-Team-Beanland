using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileBuildingModel : MonoBehaviour
{
    public PlaceableData savedPlaceableData = null; //The data of the selected object
    public GameObject objectView = null; //Prefab of the selected object
    private GridTile gridTile = null; //The gridTile connected to the object

    private List<TileBuildingModel> connectedTiles = new List<TileBuildingModel>(); //The gridTile connected to the object

    //Colors for when moving over a tile
    public Color hoveredColor;
    public Color occupiedColor;
    private Color unoccupiedColor;
    private Color currentColor;

    private Vector3 gridOffset = new Vector3(0.5f, 0, 0.5f); //Offset of the item to be centered in the grid, default to 1x1

    private void Start()
    {
        //Get the grid tile script attached to the object
        gridTile = this.gameObject.GetComponent<GridTile>();

        //Default colors are the material colors placed on the grid
        unoccupiedColor = this.GetComponent<Renderer>().material.color;
        currentColor = unoccupiedColor;
    }

    //When the user presses down on a tile
    public void OnMouseDown()
    {
        //Check for the correct state
        if(BuildingManager.currentManager.managerState == BuildingState.building)
        {
            if (!IsTilesVerified()) { return; }
            OnBuildClick();
            LinkNeighbours();
        }
        else if (BuildingManager.currentManager.managerState == BuildingState.destroying)
        {
            OnSellClick();
            UnlinkNeighbours();
        }
    }

    private bool IsTilesVerified()
    {
        //Checks if the tile is already occupied
        if (savedPlaceableData != null)
        {
            //Debug that building location is invalid
            DebugManager.DebugLog("Invalid location for placement");
            return false;
        }

        //Checks if neighbouring tiles are already occupied
        foreach (TileBuildingModel tile in BuildingManager.currentManager.activeGridGenerator.GetGridTileNeighbours(this.gameObject, BuildingManager.currentManager.selectedObjectPrefab.GetOrientatedDimensions()).ToList())
        {
            //If occupied, return
            if (tile.savedPlaceableData != null)
            {
                return false;
            }
        }

        //Return as valid
        return true;
    }

    private void OnBuildClick()
    {
        //Update the saved data on the tile
        savedPlaceableData = BuildingManager.currentManager.selectedObjectPrefab;

        //Place the object view
        objectView = Instantiate(savedPlaceableData.GetPrefab(), this.transform);

        //Add the data script to the object view
        objectView.AddComponent<PlaceableData>().Initialize(savedPlaceableData);

        //Update the position based on the intended offset of the object
        objectView.transform.position += RecalculateGridOffset();

        //Rename the object for the editor
        objectView.name = savedPlaceableData.GetName() + " View";

        //Apply the orientation to the view
        ApplyOrientation();

        //Debug
        DebugManager.DebugLog(savedPlaceableData.GetName()+" has been placed!");
    }

    private void LinkNeighbours()
    {
        //Clear the connected tiles of any old neighbours
        connectedTiles.Clear();

        //Get neighbours from the grid generator using the current dimensions of the tile
        connectedTiles = BuildingManager.currentManager.activeGridGenerator.GetGridTileNeighbours(this.gameObject,savedPlaceableData.GetOrientatedDimensions());

        //Add source tile to list so it gets cleared when another tile is clicked
        connectedTiles.Add(this);

        //Update neighbour information
        foreach (TileBuildingModel tile in connectedTiles.ToList())
        {
            //Set the object view to the source view
            tile.objectView = this.objectView;

            //Set the placeable data
            tile.savedPlaceableData = this.savedPlaceableData;

            //Update the references connected tiles on each other tile
            tile.connectedTiles = connectedTiles;

            //Color the tile for debugging purposes
            tile.ChangeCurrentMaterialColor(occupiedColor);
        }
    }

    private void OnSellClick()
    {
        //Checks if the tile is already occupied
        if (savedPlaceableData == null)
        {
            //Debug that building location is invalid
            DebugManager.DebugLog("Nothing to remove!");
            return;
        }

        //Debug
        DebugManager.DebugLog(savedPlaceableData.GetName() + " has been removed!");
    }

    private void UnlinkNeighbours()
    {
        //Clear neighbour data
        foreach (TileBuildingModel tile in connectedTiles.ToList())
        {
            //Clearing method
            tile.ClearDataView();
        }

        //Clear the connected tiles list of former neighbours
        connectedTiles.Clear();
    }

    private void ClearDataView()
    {
        //Remove the object
        Destroy(objectView);

        //Clear stored information
        objectView = null;
        savedPlaceableData = null;

        //Update the colors
        ChangeCurrentMaterialColor(unoccupiedColor);
    }

    //Change color when mouse enters
    public void OnMouseEnter()
    {
        ChangeMaterialColor(hoveredColor);
    }

    //Change color when mouse leaves
    public void OnMouseExit()
    {
        ChangeMaterialColor(currentColor);
    }

    public void ChangeMaterialColor(Color givenColor)
    {
        //Set the color on the MeshRenderer
        this.GetComponent<Renderer>().material.color = givenColor;
    }

    public void ChangeCurrentMaterialColor(Color givenColor)
    {
        currentColor = givenColor;
        ChangeMaterialColor(currentColor);
    }

    public void UpdateMaterialColor()
    {
        ChangeMaterialColor(currentColor);
    }

    //Should only be called once when placing an object
    public Vector3 RecalculateGridOffset()
    {
        //Update the base offset without height considered based on the current selected building object
        gridOffset = BuildingManager.currentManager.getFlatObjectOffset();

        //Update the height offset of the grid
        gridOffset.y += gridTile.GetAverageHeight();

        //Return
        return gridOffset;
    }

    private void ApplyOrientation()
    {
        //Update the object rotation based on the orientation
        objectView.transform.rotation = Quaternion.Euler(0, 90 * (int)savedPlaceableData.GetOrientation(), 0);
    }
}
