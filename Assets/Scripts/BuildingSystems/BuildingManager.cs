using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum PlayerInteractionState
{
    building=0,
    destroying,
    inactive,
    curing,
    gassing
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
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.CLICKEDPLACEABLEGUI, OnPlaceableUISelect);
        EventManager.currentManager.Subscribe(EventType.CURRENTHOVEREDTILE, OnCurrentHoveredSourceTile);
        EventManager.currentManager.Subscribe(EventType.ACTIVATEINTERACTIONSTATE, OnInteractionStateChange);
        EventManager.currentManager.Subscribe(EventType.GAMEOVER, OnGameOver);
    }

    private void OnDisable()
    {
        //Unsubscribes the method and event type to the current manager
        EventManager.currentManager.Unsubscribe(EventType.CLICKEDPLACEABLEGUI, OnPlaceableUISelect);
        EventManager.currentManager.Unsubscribe(EventType.CURRENTHOVEREDTILE, OnCurrentHoveredSourceTile);
        EventManager.currentManager.Unsubscribe(EventType.ACTIVATEINTERACTIONSTATE, OnInteractionStateChange);
        EventManager.currentManager.Unsubscribe(EventType.GAMEOVER, OnGameOver);
    }

    public GridPlaneGenerator activeGridGenerator = null; //Reference to the active grid that the building manager will interact with
    public PlaceableData selectedObjectPrefab; //Prefab of the selected object

    private GameObject selectedObjectPreview; //Active preview of the selected object
    private Vector3 flatObjectOffset = new Vector3(0.5f, 0, 0.5f); //Offset for the preview and placed object without considering the grid height
    private Vector2 defaultTileDimensions = new Vector2(1,1);

    public PlayerInteractionState managerState = PlayerInteractionState.inactive; //State of the manager (for swapping users interactive capabilities)
    private PlaceableOrientation currentOrientation = PlaceableOrientation.Forward; //Current rotation/orientation of the structure being placed

    private List<TileBuildingModel> selectedConnectedTiles = new List<TileBuildingModel>(); //Current list of connected tiles to the current source/hover tile
    private List<TileBuildingModel> previousOverlappingTiles = new List<TileBuildingModel>(); //Previous list of overlapping tiles
    private GameObject hoveredTile = null; //The tile currently being hovered over
    private bool gameOver = false;

    private void Start()
    {
        //Automatically fetch a default starting tile
        if (hoveredTile==null) { hoveredTile = activeGridGenerator.GetGridTilesReference()[0]; }
    }

    private void Update()
    {
        if (managerState == PlayerInteractionState.building) {BuildModeRotate(); } //Looks out for key commands to rotate objects
        if (Input.GetKeyDown(KeyCode.Escape)) { OnEscapeKey(); } //Looks out for the escape key
    }

    private void LateUpdate()
    {
        if (managerState == PlayerInteractionState.building) { HighlightOverlap(); } //Highlights overlapping objects and the preview red
    }

    private void OnEscapeKey()
    {
        //Fire off event with current building state
        EventManager.currentManager.AddEvent(new BuildingManagerEscapeKeyPressed(managerState));

        //Exit out of other modes
        EventManager.currentManager.AddEvent(new ActivateInteractionState(PlayerInteractionState.inactive));
    }

    #region Building state Mode Enablers/Disablers
    public void EnableBuildingMode()
    {
        //Checks if preview is already active, if yes, then remove the old preview
        if(selectedObjectPreview != null)
        {
            DisableBuildPreview();
        }

        //Enables the build mode
        managerState = PlayerInteractionState.building;

        //Debug that building mode is activated
        DebugManager.DebugLog("Building manager set to build mode!");

        //Update the flat offset that will be applied to the preview and placed object
        UpdateOffsetAndDimensions();

        //Enable the build preview of the object
        EnableBuildPreview();

        //Fire off event with hovered tile
        EventManager.currentManager.AddEvent(new CurrentHoveredTile(hoveredTile));
    }

    public void EnableDestroyingMode()
    {
        //Enables the sell mode
        managerState = PlayerInteractionState.destroying;

        //Debug that destroying mode is activated
        DebugManager.DebugLog("Building manager set to destroy mode!");

        DisableBuildPreview();

        //Fire off event with current orientations
        EventManager.currentManager.AddEvent(new CurrentObjectOffsetUpdated(new Vector3(defaultTileDimensions.x / 2, 0, defaultTileDimensions.y / 2), defaultTileDimensions));
    }

    public void EnableInactiveMode()
    {
        //Enables the inactive mode
        managerState = PlayerInteractionState.inactive;

        //Debug that inactive mode is activated
        DebugManager.DebugLog("Building manager set to inactive mode!");

        //Disable the build preview of the object
        DisableBuildPreview();

        //Fire off event with current orientations
        EventManager.currentManager.AddEvent(new CurrentObjectOffsetUpdated(new Vector3(defaultTileDimensions.x / 2, 0, defaultTileDimensions.y / 2), defaultTileDimensions));
    }

    public void EnableCuringMode()
    {
        //Enables the inactive mode
        managerState = PlayerInteractionState.curing;

        //Debug that inactive mode is activated
        DebugManager.DebugLog("Building manager set to curing mode!");

        //Disable the build preview of the object
        DisableBuildPreview();

        //Fire off event with current orientations
        EventManager.currentManager.AddEvent(new CurrentObjectOffsetUpdated(new Vector3(defaultTileDimensions.x / 2, 0, defaultTileDimensions.y / 2), defaultTileDimensions));
    }

    public void EnableGassingMode()
    {
        //Enables the inactive mode
        managerState = PlayerInteractionState.gassing;

        //Debug that inactive mode is activated
        DebugManager.DebugLog("Building manager set to gassing mode!");

        //Disable the build preview of the object
        DisableBuildPreview();

        //Fire off event with current orientations
        EventManager.currentManager.AddEvent(new CurrentObjectOffsetUpdated(new Vector3(defaultTileDimensions.x / 2, 0, defaultTileDimensions.y / 2), defaultTileDimensions));
    }

    private void EnableBuildPreview()
    {
        //Error handling
        if (selectedObjectPrefab == null)
        {
           Debug.LogWarning("No object prefab selected, but don't worry this is expected!");
        }

        //Create a build preview object
        selectedObjectPreview = Instantiate(selectedObjectPrefab.GetPrefab());
        selectedObjectPreview.AddComponent<ObjectViewHighlight>();
        selectedObjectPreview.AddComponent<BuildingPreview>();

        //Rename the object for the unity editor
        selectedObjectPreview.name = selectedObjectPrefab.GetName() + " Preview Object";

        //Update the orientation of the preview
        UpdatePreviewOrientation();
    }

    private void DisableBuildPreview()
    {
        //Ignore if already destroyed
        if (selectedObjectPreview == null) { Debug.LogWarning("No object prefab selected, but don't worry this is expected!"); return; }

        //Unhighlight cells on the preview
        selectedObjectPreview.GetComponent<BuildingPreview>().UnhighlightTiles();

        //Destroy the build preview object
        Destroy(selectedObjectPreview);

        //Set back to null
        selectedObjectPreview = null;

        //Unhighlight old tiles if applicable
        UnhighlightOldOverlap();
    }
    #endregion

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

        //Create a temporary reference to make method calls less intensive
        BuildingPreview selectedObjectPreviewScript = selectedObjectPreview.GetComponent<BuildingPreview>();

        //Update the visuals based on rotations
        selectedObjectPreviewScript.SetIsUprightObject(selectedObjectPrefab.GetIsUprightObject());

        //Rotate based on the current orientation
        selectedObjectPreviewScript.UpdateRotation(currentOrientation);

        //Update the visuals based on rotations
        selectedObjectPreviewScript.UpdateVisuals();

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

        //Fire off event with current orientations
        EventManager.currentManager.AddEvent(new CurrentObjectOffsetUpdated(flatObjectOffset, dimensionOrientated));

        //Update the intended orientation for the built object
        selectedObjectPrefab.SetOrientatedDimensions(dimensionOrientated);

        //Update the list of connected tiles so that it correctly reflects the rotation
        UpdateConnectedTiles();
    }

    private void OnCurrentHoveredSourceTile(EventData eventData)
    {
        if (eventData is CurrentHoveredTile)
        {
            //Cast the data to be used
            CurrentHoveredTile currentHoveredTileData = (CurrentHoveredTile)eventData;

            //Set the new hovered tile
            hoveredTile = currentHoveredTileData.currentHoveredTile;

            //Update the connected tiles
            UpdateConnectedTiles();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.CURRENTHOVEREDTILE was received but is not of class CurrentHoveredTile.");
        }
    }

    private void UpdateConnectedTiles()
    {
        //Update the list of connecting tiles
        selectedConnectedTiles = activeGridGenerator.GetGridTileNeighbours(hoveredTile, selectedObjectPrefab.GetOrientatedDimensions());
    }

    public Vector3 getFlatObjectOffset()
    {
        return flatObjectOffset;
    }

    public List<TileBuildingModel> GetSelectedConnectingTiles()
    {
        return selectedConnectedTiles;
    }

    public bool ConnectingTilesListWasPopulated()
    {
        //Checks that connecting tiles are neither empty nor null
        if (selectedConnectedTiles == null || selectedConnectedTiles.Count <= 0)
        {
            //Return invalid
            return false;
        }
        //Return valid
        return true;
    }

    public void HighlightOverlap()
    {
        //Continue if tiles list was populated
        if (!ConnectingTilesListWasPopulated())
        {
            //If invalid, highlight the preview object
            HighlightPreviewObject(true); 
            return; 
        }

        //Temporary boolean and building model reference
        bool foundOverlap = false;
        List<TileBuildingModel> overlappingTileModels = new List<TileBuildingModel>();

        //Highlight Building Preview if tile is occupied
        foreach (TileBuildingModel tile in selectedConnectedTiles)
        {
            //If already occupied, save the occupied tile
            if (tile.savedPlaceableData && tile.objectView)
            {
                foundOverlap = true;
                overlappingTileModels.Add(tile);

                //Check if in the list of previously overlapping objects, if yes then remove from that list
                if (previousOverlappingTiles.Contains(tile))
                {
                    previousOverlappingTiles.Remove(tile);
                }
            }
            //tile.ChangeMaterialColor(tile.occupiedColor);
        }

        //Unhighlight old tiles
        UnhighlightOldOverlap();

        //Set the new list of overlapping tiles before filtering
        previousOverlappingTiles = overlappingTileModels;

        //Filter through the overlapping tiles for repeating object views and set the highlight red
        foreach (TileBuildingModel tile in overlappingTileModels.Distinct(new TileObjectViewComparer()).ToList())
        {
            //Toggle red
            tile.SetRedHighlight();
        }

        //If overlapping tiles were found, highlight the preview red
        HighlightPreviewObject(foundOverlap);
    }

    private void HighlightPreviewObject(bool shouldHighlight)
    {
        //If should highlight due to overlapping tiles, highlight the preview red
        if (selectedObjectPreview != null && selectedObjectPreview.TryGetComponent(out BuildingPreview preview))
        {
            if (shouldHighlight)
            {
                //Set the highlight to red
                preview.SetRedHighlight();
            }
            else
            {
                //Reset the color
                preview.ResetHighlight();
            }
        }
    }

    private void UnhighlightOldOverlap()
    {
        //Unhighlight old tiles
        foreach (TileBuildingModel tile in previousOverlappingTiles)
        {
            //Error handling, meant to fail silently as previous overlapping tiles may have been recently been cleared of their model
            if (tile == null) { return; }

            //Reset the color
            tile.ResetHighlight();
            //tile.ChangeMaterialColor(tile.hoveredColor);
        }
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
                EventManager.currentManager.AddEvent(new ActivateInteractionState(PlayerInteractionState.building));
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

    private void OnInteractionStateChange(EventData eventData)
    {
        if (gameOver)
        {
            return;
        }
        if (eventData is ActivateInteractionState)
        {
            //Cast the event for ease of use
            ActivateInteractionState interactionState = (ActivateInteractionState)eventData;

            //Apply the state change based on the input
            switch (interactionState.interactionState)
            {
                case PlayerInteractionState.building:
                    EnableBuildingMode();
                    break;

                case PlayerInteractionState.destroying:
                    EnableDestroyingMode();
                    break;

                case PlayerInteractionState.curing:
                    EnableCuringMode();
                    break;

                case PlayerInteractionState.gassing:
                    EnableGassingMode();
                    break;

                case PlayerInteractionState.inactive:
                    EnableInactiveMode();
                    break;
            }
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.ACTIVATEINTERACTIONSTATE was received but is not of class ActivateInteractionState.");
        }
    }

    private void OnGameOver(EventData eventData)
    {
        //turns off functionality after game is over
        gameOver = true;

        managerState = PlayerInteractionState.inactive;
        EnableInactiveMode();
    }
}