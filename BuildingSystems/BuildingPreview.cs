using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public Material validMaterial; //Valid placement
    public Material invalidMaterial; //Invalid placement
    private GameObject hoveredTiled = null; //Tile that is being hovered over

    private void Update()
    {
        CheckForHoveredTile();
    }
    private void CheckForHoveredTile()
    {
        //Raycast to see where the mouse currently is (if a tile, then display and update location)
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            //If colliding with a gridTile
            if (hit.transform.tag == "GridTile")
            {
                //If not already the selected tile, update the relevant information
                if(hit.transform.gameObject != hoveredTiled)
                {
                    //Set the new tile
                    hoveredTiled = hit.transform.gameObject;

                    //Update the location
                    UpdateDisplayLocation();
                }
            }
        }
    }

    private void UpdateDisplayLocation()
    {
        //Update the location based on the position intended offset for the tile
        this.transform.position = hoveredTiled.transform.position + hoveredTiled.GetComponent<TileBuildingModel>().RecalculateGridOffset();
    }

    public void UpdateRotation(PlaceableOrientation newOrientation)
    {
        //Temporary rotation
        Quaternion newRotation = Quaternion.Euler(0, 90 * (int)newOrientation, 0);

        //Rotate based on the current orientation
        this.transform.rotation = newRotation;
    }
}
