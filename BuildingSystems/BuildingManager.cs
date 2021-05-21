using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BuildingState
{
    building=0,
    destroying,
    inactive
}

public class BuildingManager : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------------------------------
    //                                                  Singleton
    //------------------------------------------------------------------------------------------------------------------------
    //Getter and setter for the current manager, static so that there is only one manager at any given time
    public static BuildingManager currentManager { get; set; } = null;

    //Awake function ensures that only one copy exists in the scene at a given time
    private void Awake()
    {
        if (currentManager == null)
        {
            currentManager = this; //Sets the active build manager to this instance of it
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.CLICKEDPLACEABLEGUI, OnPlaceableUISelect);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.CLICKEDPLACEABLEGUI, OnPlaceableUISelect);
    }

    public GridPlaneGenerator activeGridGenerator = null; //Reference to the active grid that the building manager will interact with
    public PlaceableData selectedObjectPrefab; //Prefab of the selected object

    private GameObject selectedObjectPreview; //Active preview of the selected object
    private Vector3 flatObjectOffset = new Vector3(0.5f, 0, 0.5f); //Offset for the preview and placed object without considering the grid height

    public BuildingState managerState = BuildingState.inactive; //State of the manager (for swapping users interactive capabilities)
    private PlaceableOrientation currentOrientation = PlaceableOrientation.Forward; //Current rotation/orientation of the structure being placed

    private void Update()
    {
        if (managerState == BuildingState.building) {BuildModeRotate(); } //Looks out for key commands to rotate objects
    }

    public void EnableBuildingMode()
    {
        //Checks if already in build mode or if preview is active
        //If yes, then remove the old preview to
        if(managerState == BuildingState.building || selectedObjectPreview!=null)
        {
            DisableBuildPreview();
        }

        //Enables the build mode
        managerState = BuildingState.building;

        //Debug that building mode is activated
        DebugManager.DebugLog("Building manager set to build mode!");

        //Update the flat offset that will be applied to the preview and placed object
        UpdateOffsetAndDimensions();

        //Enable the build preview of the object
        EnableBuildPreview();
    }

    public void EnableDestroyingMode()
    {
        //Enables the sell mode
        managerState = BuildingState.destroying;

        //Debug that destroying mode is activated
        DebugManager.DebugLog("Building manager set to destroy mode!");

        DisableBuildPreview();
    }
    public void EnableInactiveMode()
    {
        //Enables the inactive mode
        managerState = BuildingState.inactive;

        //Debug that inactive mode is activated
        DebugManager.DebugLog("Building manager set to inactive mode!");

        //Disable the build preview of the object
        DisableBuildPreview();
    }

    private void EnableBuildPreview()
    {
        //Error handling
        if (selectedObjectPrefab == null)
        { 
            throw new MissingReferenceException();
        }

        //Create a build preview object
        selectedObjectPreview = Instantiate(selectedObjectPrefab.GetPrefab());
        selectedObjectPreview.AddComponent<BuildingPreview>();

        //Rename the object for the unity editor
        selectedObjectPreview.name = selectedObjectPrefab.GetName() + " Preview Object";

        //Update the orientation of the preview
        UpdatePreviewOrientation();
    }

    private void DisableBuildPreview()
    {
        //Unhighlight cells on the preview
        selectedObjectPreview.GetComponent<BuildingPreview>().UnhighlightTiles();

        //Destroy the build preview object
        Destroy(selectedObjectPreview);

        //Set back to null
        selectedObjectPreview = null;
    }

    //Gets the rotation
    //Rotating Rightwards: Forward (0) -> Right (1) -> Back (2) -> Left (3)
    //Rotating Leftwards: Left (3) -> Back (2) -> Right (1) -> Forward (0)
    private void BuildModeRotate()
    {
        //Rotate right
        if (Input.GetKeyDown(KeyCode.Period))
        {
            //Loop around
            if (currentOrientation == PlaceableOrientation.Left)
            {
                currentOrientation = PlaceableOrientation.Forward;
            }
            else
            {
                currentOrientation++;
            }

            //Update the offset
            UpdateOffsetAndDimensions();

            //Update orientations
            UpdatePreviewOrientation();
        }
        //Rotate left
        else if (Input.GetKeyDown(KeyCode.Comma))
        {
            //Loop around
            if (currentOrientation == PlaceableOrientation.Forward)
            {
                currentOrientation = PlaceableOrientation.Left;
            }
            else
            {
                currentOrientation--;
            }

            //Update the offset
            UpdateOffsetAndDimensions();

            //Update orientations
            UpdatePreviewOrientation();
        }
    }

    //Updates the orientation of the preview and object that would be rotated
    private void UpdatePreviewOrientation()
    {
        //Debug the change in rotation
        DebugManager.DebugLog("Rotation changed to " + Enum.GetName(typeof(PlaceableOrientation), currentOrientation) + " orientation!");

        //Rotate based on the current orientation
        selectedObjectPreview.GetComponent<BuildingPreview>().UpdateRotation(currentOrientation);

        //Set the correct rotation to the built object
        selectedObjectPrefab.SetOrientation(currentOrientation);
    }

    //Updates the offset based on the objects dimensions
    private void UpdateOffsetAndDimensions()
    {
        //Example offset for 1x1 object (0.5f, x, 0.5f)
        //Example offset for 1x2 object (0.5f, x, 1f)
        Vector2 offsetOrientated = selectedObjectPrefab.GetDimensions();

        //Example orientation for 2x3 object rotated right()
        Vector2 dimensionOrientated = offsetOrientated;

        //Rotate the dimensions based on the current orientation
        switch (currentOrientation)
        {
            case PlaceableOrientation.Forward:
                //No changes to dimensions as default orientation is forward
                break;

            case PlaceableOrientation.Right:
                //Rotate the dimensions/offset rightwards
                offsetOrientated = new Vector2(offsetOrientated.y, -offsetOrientated.x + 2);
                dimensionOrientated = new Vector2(dimensionOrientated.y, -dimensionOrientated.x);
                break;

            case PlaceableOrientation.Back:
                //Rotate the dimensions/offset backwards
                offsetOrientated = new Vector2(-offsetOrientated.x + 2, -offsetOrientated.y + 2);
                dimensionOrientated = new Vector2(-dimensionOrientated.x, -dimensionOrientated.y);
                break;

            case PlaceableOrientation.Left:
                //Rotate the dimensions/offset leftwards
                offsetOrientated = new Vector2(-offsetOrientated.y + 2, offsetOrientated.x);
                dimensionOrientated = new Vector2(-dimensionOrientated.y, dimensionOrientated.x);
                break;
        }

        //Update the intended flat offset for the preview and built object
        flatObjectOffset = new Vector3(offsetOrientated.x/2, 0, offsetOrientated.y/2);

        //Update the intended orientation for the built object
        selectedObjectPrefab.SetOrientatedDimensions(dimensionOrientated);
    }

    public Vector3 getFlatObjectOffset()
    {
        return flatObjectOffset;
    }

    private void OnPlaceableUISelect(EventData eventData)
    {
        if (eventData is PlaceableSelectedOnGUI)
        {
            //Adds the event data received to a class for use 
            PlaceableSelectedOnGUI placeableGUISelect = (PlaceableSelectedOnGUI)eventData;
            //subtracts money from player 
            if (placeableGUISelect.placeable != null)
            {
                selectedObjectPrefab = placeableGUISelect.placeable;
                //EnableBuildingMode();
                StartCoroutine(EnableBuildingModeCoroutine());
            }
            else
            {
                throw new System.NullReferenceException("placeable data was null");
            }            
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.CLICKEDPLACEABLEGUI was received but is not of class PlaceableSelectedOnGUI.");
        }
    }
    IEnumerator EnableBuildingModeCoroutine()
    {
        yield return new WaitForEndOfFrame();

        EnableBuildingMode();
    }
}