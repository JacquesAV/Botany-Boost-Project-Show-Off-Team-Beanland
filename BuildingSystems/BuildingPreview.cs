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

            //Update location
            UpdateDisplayLocation();

            //Update colors
            HighlightCycle();
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
        //Temporary rotation
        Quaternion newRotation = Quaternion.Euler(0, 90 * (int)newOrientation, 0);

        //Set the rotation
        transform.rotation = newRotation;

        //Do not proceed if no hovered tile exists
        //Should be allowed to fail silently as there are a few instances where hovered tile is meant to fail
        if (hoveredTiled == null) { return; }

        //Update the correct display
        UpdateDisplayLocation();

        //Update colors
        HighlightCycle();
    }

    private void HighlightCycle()
    {
        //Change tile colors for viewing
        UnhighlightTiles();

        //Find new connected tiles
        connectedTiles = BuildingManager.currentManager.GetSelectedConnectingTiles();

        //Change tile colors for viewing
        HighlightTileColors();
    }

    //Highlight functionality
    public void HighlightTileColors()
    {
        //Do not proceed if connected tiles are null
        if (connectedTiles is null || connectedTiles.Count == 0) { return; }

        //Checks for neighbouring tiles and update color
        foreach (TileBuildingModel tile in connectedTiles)
        {
            tile.ChangeMaterialColor(Color.yellow);
        }
    }

    public void UnhighlightTiles()
    {
        //Do not proceed if connected tiles are null or empty
        if (connectedTiles is null || connectedTiles.Count == 0) { return; }

        //Unhighlight old tiles
        foreach (TileBuildingModel tile in connectedTiles)
        {
            tile.UpdateMaterialColor();
        }
       
    }
}
