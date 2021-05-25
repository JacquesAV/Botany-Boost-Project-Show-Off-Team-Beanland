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

    private void Update()
    {
        CheckForHoveredTile();
    }
    private void CheckForHoveredTile()
    {
        //Check if pointer is over a ui element
        if (!EventSystem.current.IsPointerOverGameObject(-1))
        {
            //Raycast to see where the mouse currently is (if a tile, then display and update location)
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                //If colliding with a gridTile
                if (hit.transform.tag is "GridTile")
                {
                    //If not already the selected tile, update the relevant information
                    if (hit.transform.gameObject != hoveredTiled)
                    {
                        //Set the new tile
                        hoveredTiled = hit.transform.gameObject;

                        //Do not proceed if no hovered tile exists
                        if (hoveredTiled == null) { throw new MissingReferenceException(); }

                        //Update the location
                        UpdateDisplayLocation();

                        //Updates colors
                        HighlightCycle();
                    }
                }
            }
            return;
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

        //Update the correct display
        UpdateDisplayLocation();

        HighlightCycle();
    }

    private void HighlightCycle()
    {
        //Change tile colors for viewing
        UnhighlightTiles();

        //Find new connected tiles
        connectedTiles = BuildingManager.currentManager.activeGridGenerator.GetGridTileNeighbours(hoveredTiled, BuildingManager.currentManager.selectedObjectPrefab.GetOrientatedDimensions());

        //Change tile colors for viewing
        HighlightTileColors();
    }

    //Highlight functionality
    private void HighlightTileColors()
    {
        //Checks for neighbouring tiles and update color
        foreach (TileBuildingModel tile in connectedTiles)
        {
            tile.ChangeMaterialColor(Color.yellow);
        }
    }

    public void UnhighlightTiles()
    {
        //Unhighlight old tiles
        foreach (TileBuildingModel tile in connectedTiles)
        {
            tile.UpdateMaterialColor();
        }
    }
}
