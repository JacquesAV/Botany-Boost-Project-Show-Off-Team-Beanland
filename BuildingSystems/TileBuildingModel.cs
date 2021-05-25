using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

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
        gridTile = gameObject.GetComponent<GridTile>();

        //Default colors are the material colors placed on the grid
        unoccupiedColor = GetComponent<Renderer>().material.color;
        currentColor = unoccupiedColor;
    }

    //When the user presses down on a tile
    public void OnMouseDown()
    {        //Check if pointer is over a ui element
        if (!EventSystem.current.IsPointerOverGameObject(-1))
        {
            //Check for the correct state
            if (BuildingManager.currentManager.managerState == BuildingState.building)
            {
                //Building Functionalities
                if (!IsTilesVerified()) { return; }
                OnBuildClick();
            }
            else if (BuildingManager.currentManager.managerState == BuildingState.destroying)
            {
                //Destroying Functionalities
                OnSellClick();
                UnlinkNeighbours();
            }
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
        foreach (TileBuildingModel tile in BuildingManager.currentManager.activeGridGenerator.GetGridTileNeighbours(gameObject, BuildingManager.currentManager.selectedObjectPrefab.GetOrientatedDimensions()).ToList())
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
        //Fire off event to check if building price is possible and subscribe to an event so that it may recieve the result with ease
        EventManager.currentManager.Subscribe(EventType.OBJECTBUYREQUESTRESULT, OnObjectBuyRequestResult);
        EventManager.currentManager.AddEvent(new ObjectBuyRequest(BuildingManager.currentManager.selectedObjectPrefab));
    }

    private void OnObjectBuyRequestResult(EventData eventData)
    {
        if (eventData is ObjectBuyRequestResult)
        {
            //Cast the event so it can be used
            ObjectBuyRequestResult objectResult = (ObjectBuyRequestResult)eventData;

            //Unsubscribe from waiting for a request
            EventManager.currentManager.Unsubscribe(EventType.OBJECTBUYREQUESTRESULT, OnObjectBuyRequestResult);

            //Handle the incoming data if result was allowed
            if(objectResult.wasAllowed)
            {
                HandleIncomingObjectBuyResult(objectResult);
            }
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.OBJECTBUYREQUESTRESULT was received but is not of class ObjectBuyRequestResult.");
        }
    }
    private void HandleIncomingObjectBuyResult(ObjectBuyRequestResult objectResult)
    {
        //Update the saved data on the tile
        savedPlaceableData = objectResult.requestedObject;

        //Place the object view
        objectView = Instantiate(savedPlaceableData.GetPrefab(), transform);

        //Add the data script to the object view
        objectView.AddComponent<PlaceableData>().Initialize(savedPlaceableData);

        //Add PlantEffect script to placeables that are plants
        string placeableType = savedPlaceableData.GetPlaceableType();
        if (placeableType != "Flooring" && placeableType != "Ornament")
        {
            objectView.AddComponent<PlantEffects>();
            objectView.tag = "Plant";
            SphereCollider sphere = objectView.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            objectView.layer = 2; //Set to ignore raycast layer
        }

        //Update the position based on the intended offset of the object
        objectView.transform.position += RecalculateGridOffset();

        //Rename the object for the unity editor
        objectView.name = savedPlaceableData.GetName() + " View";

        //Apply the orientation to the view
        ApplyOrientation();

        //Debug
        DebugManager.DebugLog(savedPlaceableData.GetName() + " has been placed!");

        //Fire off event with created information
        EventManager.currentManager.AddEvent(new ObjectBoughtScores(savedPlaceableData.GetCost(), savedPlaceableData.GetBiodiversity(), savedPlaceableData.GetCarbonIntake(), savedPlaceableData.GetAttractiveScore(), savedPlaceableData.GetInsectType(), savedPlaceableData.GetInsectAttractiveness()));

        //Then link neighbours
        LinkNeighbours();
    }

    private void LinkNeighbours()
    {
        //Clear the connected tiles of any old neighbours
        connectedTiles.Clear();

        //Get neighbours from the grid generator using the current dimensions of the tile
        connectedTiles = BuildingManager.currentManager.activeGridGenerator.GetGridTileNeighbours(gameObject,savedPlaceableData.GetOrientatedDimensions());

        //Add source tile to list so it gets cleared when another tile is clicked
        connectedTiles.Add(this);

        //Update neighbour information
        foreach (TileBuildingModel tile in connectedTiles.ToList())
        {
            //Set the object view to the source view
            tile.objectView = objectView;

            //Set the placeable data
            tile.savedPlaceableData = savedPlaceableData;

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

        //Fire off event with sold information
        EventManager.currentManager.AddEvent(new ObjectSoldScores(savedPlaceableData.GetCost(), savedPlaceableData.GetBiodiversity(), savedPlaceableData.GetCarbonIntake(), savedPlaceableData.GetAttractiveScore(), savedPlaceableData.GetInsectType(), savedPlaceableData.GetInsectAttractiveness()));
        
        //Debug
        DebugManager.DebugLog(savedPlaceableData.GetName() + " has been removed!");
    }

    public void UnlinkNeighbours()
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
    {        //Check if pointer is over a ui element
        if (!EventSystem.current.IsPointerOverGameObject(-1))
        {
            ChangeMaterialColor(hoveredColor);
        }
    }

    //Change color when mouse leaves
    public void OnMouseExit()
    {
        ChangeMaterialColor(currentColor);
    }

    public void ChangeMaterialColor(Color givenColor)
    {
        //Set the color on the MeshRenderer
        GetComponent<Renderer>().material.color = givenColor;
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
