using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBuildingModel : MonoBehaviour
{
    public PlaceableData savedPlaceableData = null; //The data of the selected object
    public GameObject objectView = null; //Prefab of the selected object
    private GridTile gridTile = null; //The gridTile connected to the object
    
    //Colors for when moving over a tile
    public Color highlightedColor;
    public Color unhighlightedColor;
    private Color currentColor;

    private Vector3 gridOffset = new Vector3(0.5f, 0, 0.5f); //Offset of the item to be centered in the grid

    private void Start()
    {
        //Get the grid tile script attached to the object
        gridTile = this.gameObject.GetComponent<GridTile>();

        //Update the height offset of the grid
        gridOffset.y += gridTile.GetAverageHeight();
    }

    //When the user presses down on a tile
    public void OnMouseDown()
    {
        //Check for the correct state
        if(BuildingManager.currentManager.managerState == BuildingState.building)
        {
            OnBuildClick();
        }
        else if (BuildingManager.currentManager.managerState == BuildingState.destroying)
        {
            OnSellClick();
        }
    }

    private void OnBuildClick()
    {
        //Checks if the tile is already occupied
        if (savedPlaceableData != null)
        {
            //Debug that building location is invalid
            DebugManager.DebugLog("Invalid location for placement");
            return;
        }

        //Update the saved data on the tile
        savedPlaceableData = BuildingManager.currentManager.selectedObjectPrefab;

        //Place the object view
        objectView = Instantiate(savedPlaceableData.GetPrefab(), this.transform);
        objectView.transform.position += gridOffset;

        //Rename the object for the editor
        objectView.name = savedPlaceableData.GetName() + " View";

        //Apply the orientation to the view
        ApplyOrientation();

        //Debug
        DebugManager.DebugLog(savedPlaceableData.GetName()+" has been placed!");
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

        //Remove the object
        Destroy(objectView);
        objectView = null;
        savedPlaceableData = null;
    }

    //Change color when mouse enters
    public void OnMouseEnter()
    {
        ChangeHighlight(highlightedColor);
    }

    //Change color when mouse leaves
    public void OnMouseExit()
    {
        ChangeHighlight(unhighlightedColor);
    }

    public void ChangeHighlight(Color givenColor)
    {
        //Set the color
        currentColor = givenColor;

        //Set the color on the MeshRenderer
        this.GetComponent<Renderer>().material.color = currentColor;
    }

    public Vector3 GetGridOffset()
    {
        return gridOffset;
    } 

    private void ApplyOrientation()
    {
        //Update the object rotation based on the orientation
        objectView.transform.rotation = Quaternion.Euler(0, 90 * (int)savedPlaceableData.GetOrientation(), 0);
    }
}
