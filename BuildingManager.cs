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

    public GridPlaneGenerator activeGridGenerator = null; //Reference to the active grid that the building manager will interact with
    public PlaceableData selectedObjectPrefab; //Prefab of the selected object

    private GameObject selectedObjectPreview; //Active preview of the selected object

    public BuildingState managerState = BuildingState.inactive; //State of the manager (for swapping users interactive capabilities)
    private PlaceableOrientation currentOrientation = PlaceableOrientation.Forward; //Current rotation/orientation of the structure being placed

    private void Update()
    {
        if (managerState == BuildingState.building) {BuildModeRotate(); } //Looks out for key commands to rotate objects
    }
        

    public void EnableBuildingMode()
    {
        //Enables the build mode
        managerState = BuildingState.building;
        DebugManager.DebugLog("Building manager set to build mode!");

        EnableBuildPreview();
    }

    public void EnableDestroyingMode()
    {
        //Enables the sell mode
        managerState = BuildingState.destroying;
        DebugManager.DebugLog("Building manager set to destroy mode!");

        DisableBuildPreview();
    }
    public void EnableInactiveMode()
    {
        //Enables the inactive mode
        managerState = BuildingState.inactive;
        DebugManager.DebugLog("Building manager set to inactive mode!");

        DisableBuildPreview();
    }

    private void EnableBuildPreview()
    {
        //Error handling
        if (selectedObjectPrefab == null){ throw new MissingReferenceException();}

        //Create a build preview object
        selectedObjectPreview = Instantiate(selectedObjectPrefab.GetPrefab());
        selectedObjectPreview.AddComponent<BuildingPreview>();

        //Rename the object for the unity editor
        selectedObjectPreview.name = selectedObjectPrefab.GetName() + " Preview Object";
    }

    private void DisableBuildPreview()
    {
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Loop around
            if(currentOrientation == PlaceableOrientation.Left)
            {
                currentOrientation = PlaceableOrientation.Forward;
            }
            else
            {
                currentOrientation++;
            }

            //Update orientations
            UpdatePreviewOrientation();
        }
        //Rotate left
        else if (Input.GetKeyDown(KeyCode.Q))
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

            //Update orientations
            UpdatePreviewOrientation();
        }
    }

    //Updates the orientation of the preview and object that would be rotated
    private void UpdatePreviewOrientation()
    {
        //Debug change
        DebugManager.DebugLog("Rotation changed to " + Enum.GetName(typeof(PlaceableOrientation), currentOrientation) + " orientation!");

        //Rotate based on the current orientation
        selectedObjectPreview.GetComponent<BuildingPreview>().UpdateRotation(currentOrientation);

        //Set the correct rotation to the built object
        selectedObjectPrefab.SetOrientation(currentOrientation);
    }
}
