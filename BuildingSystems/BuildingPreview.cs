using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPreview : MonoBehaviour
{
    public Material validMaterial; //Valid placement
    public Material invalidMaterial; //Invalid placement
    private GameObject hoveredTiled = null; //Tile that is being hovered over
    private List<TileBuildingModel> connectedTiles = new List<TileBuildingModel>(); //List of tiles that connect to the source tile
    private PlaceableOrientation currentOrientation = PlaceableOrientation.Forward;
    private bool isUprightObject = false;

    private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.CURRENTHOVEREDTILE, OnCurrentHoveredTile);
    }

    private void OnDisable()
    {
        //Unsubscribes the method and event type to the current manager
        EventManager.currentManager.Unsubscribe(EventType.CURRENTHOVEREDTILE, OnCurrentHoveredTile);
    }

    //Recieved current hovered tile through an event and update acordingly
    private void OnCurrentHoveredTile(EventData eventData)
    {
        if (eventData is CurrentHoveredTile)
        {
            //Cast the data to be used
            CurrentHoveredTile currentHoveredTileData = (CurrentHoveredTile)eventData;

            //Set the new hovered tile
            hoveredTiled = currentHoveredTileData.currentHoveredTile;

            //Updates the actual visuals of the preview
            UpdateVisuals();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.CURRENTHOVEREDTILE was received but is not of class CurrentHoveredTile.");
        }
    }

    private void UpdateDisplayLocation()
    {
        //Update the location based on the position intended offset for the tile
        transform.position = hoveredTiled.transform.position + hoveredTiled.GetComponent<TileBuildingModel>().RecalculateGridOffset();
    }

    public void UpdateRotation(PlaceableOrientation newOrientation)
    {
        //Set the rotation
        transform.rotation = Quaternion.Euler(0, 90 * (int)newOrientation, 0);

        //Update the current orientation
        currentOrientation = newOrientation;
    }

    public void UpdateVisuals()
    {
        //Do not proceed if no hovered tile exists
        //Should be allowed to fail silently as there are a few instances where hovered tile is meant to fail
        if (hoveredTiled == null) { return; }

        //Change tile colors for viewing
        UnhighlightTiles();

        //Find new connected tiles
        connectedTiles = BuildingManager.currentManager.GetSelectedConnectingTiles();

        //Change tile colors for viewing
        HighlightTileColors();

        //Rotate the object to its intended normal 
        ApplyNormalOrientation();

        //Update location
        UpdateDisplayLocation();
    }

    //Highlight functionality
    public void HighlightTileColors()
    {
        //Do not proceed if connected tiles are null
        if (!HasConnectingTiles()) { return; }

        //Checks for neighbouring tiles and update color
        foreach (TileBuildingModel tile in connectedTiles)
        {
            tile.ChangeMaterialColor(Color.yellow);
        }
    }

    public void UnhighlightTiles()
    {
        //Do not proceed if connected tiles are null or empty
        if (!HasConnectingTiles()) { return; }

        //Unhighlight old tiles
        foreach (TileBuildingModel tile in connectedTiles)
        {
            tile.UpdateMaterialColor();
        }
    }

    private void ApplyNormalOrientation()
    {
        //Do not proceed if connected tiles are null or empty or if the object is meant to be upright
        if (!HasConnectingTiles() || isUprightObject) { return; }

        //Get a reset orientation to prevent normals from carrying over
        UpdateRotation(currentOrientation);

        //Apply correct normal orientation
        transform.rotation = Quaternion.FromToRotation(Vector3.up, GetAverageNormal()) * transform.rotation;
    }

    //Bool for if connected tiles are null or empty
    private bool HasConnectingTiles()
    {
        if (connectedTiles is null || connectedTiles.Count == 0) { return false; }
        else { return true; }
    }

    private Vector3 GetAverageNormal()
    {
        //Temporary variable
        Vector3 averageNormal = Vector3.zero;

        //Add each normal together
        foreach (TileBuildingModel tile in connectedTiles)
        {
            //Add the normal
            averageNormal += tile.gameObject.GetComponent<GridTile>().GetAverageNormal();
        }

        //Divide for average
        averageNormal = averageNormal / connectedTiles.Count;

        //Return
        return averageNormal;
    }

    public void SetIsUprightObject(bool givenBool)
    {
        //Update the upright state
        isUprightObject = givenBool;
    }
}
