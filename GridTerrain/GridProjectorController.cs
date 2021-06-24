using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(DecalProjector))]
public class GridProjectorController : MonoBehaviour
{
    private GameObject hoveredTile = null; //The active tile that the projector should follow
    private DecalProjector projector = null; //the projector on the object
    private bool isDisabled = false; //If the projector should be disabled
    private PlayerInteractionState currentInteractionState; //The current interaction state of the player
    private Vector3 flatOffset = new Vector3(0.5f, 0, 0.5f); //Offset for the projection
    private Vector2 dimensions = new Vector2(1, 1); //Dimensions and tiling of the projection
    public int projectorDepth = 5; //Depth of the projector

    private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.CURRENTHOVEREDTILE, OnCurrentHoveredSourceTile);
        EventManager.currentManager.Subscribe(EventType.CURRENTOBJECTOFFSETUPDATED, OnNewObjectOffset);
        EventManager.currentManager.Subscribe(EventType.ACTIVATEINTERACTIONSTATE, OnInteractionStateChange);
        EventManager.currentManager.Subscribe(EventType.GAMEOVER, OnGameOver);
    }

    private void OnDisable()
    {
        //Unsubscribes the method and event type to the current manager
        EventManager.currentManager.Unsubscribe(EventType.CURRENTHOVEREDTILE, OnCurrentHoveredSourceTile);
        EventManager.currentManager.Unsubscribe(EventType.CURRENTOBJECTOFFSETUPDATED, OnNewObjectOffset);
        EventManager.currentManager.Unsubscribe(EventType.ACTIVATEINTERACTIONSTATE, OnInteractionStateChange);
        EventManager.currentManager.Unsubscribe(EventType.GAMEOVER, OnGameOver);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get the projector
        projector = gameObject.GetComponent<DecalProjector>();

        //Throw an error if no projector was found
        if (projector == null) { throw new MissingComponentException(); }
    }

    void FixedUpdate()
    {
        UpdateProjectorSize();
        UpdateProjector();
    }

    private void UpdateProjectorSize()
    {
        //Return if no targets
        if (hoveredTile == null) { return; }

        //Rotate the dimensions based on the current orientation
        switch (currentInteractionState)
        {
            case PlayerInteractionState.building:
                //Activate projector
                projector.enabled = true;
                break;

            case PlayerInteractionState.destroying:
                //Activate projector
                projector.enabled = true;
                break;

            case PlayerInteractionState.gassing:
                //Activate projector
                projector.enabled = true;
                break;

            case PlayerInteractionState.curing:
                //Activate projector
                projector.enabled = true;
                break;

            case PlayerInteractionState.inactive:
                //Disable projector
                projector.enabled = true;
                break;

            default:
                //Enable projector
                projector.enabled = false;
                break;
        }
    }

    private void UpdateProjector()
    {
        //Return if no targets
        if (hoveredTile == null) { return; }

        //Modify size and tiling
        projector.size = new Vector3(dimensions.x, dimensions.y, projectorDepth);
        projector.uvScale = dimensions;

        //Calculate new position
        Vector3 newPosition = new Vector3(hoveredTile.transform.position.x, projector.transform.position.y, hoveredTile.transform.position.z);
        
        //Apply projector size offset
        newPosition += flatOffset;

        //Update position
        projector.transform.position = newPosition;
    }

    private void OnCurrentHoveredSourceTile(EventData eventData)
    {
        if (eventData is CurrentHoveredTile)
        {
            //Cast the data to be used
            CurrentHoveredTile currentHoveredTileData = (CurrentHoveredTile)eventData;

            //Set the new hovered tile
            hoveredTile = currentHoveredTileData.currentHoveredTile;

            //Update the projector
            UpdateProjector();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.CURRENTHOVEREDTILE was received but is not of class CurrentHoveredTile.");
        }
    }

    private void OnNewObjectOffset(EventData eventData)
    {
        if (eventData is CurrentObjectOffsetUpdated)
        {
            //Cast the data to be used
            CurrentObjectOffsetUpdated eventOffset = (CurrentObjectOffsetUpdated)eventData;

            //Set the new hovered tile
            flatOffset = eventOffset.offset;

            //Set the new targetted size
            dimensions = new Vector2(Mathf.Abs(eventOffset.dimension.x), Mathf.Abs(eventOffset.dimension.y));

            //Update the projector
            UpdateProjector();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.CURRENTOBJECTOFFSETUPDATED was received but is not of class CurrentObjectOffsetUpdated.");
        }
    }

    //Change the visual for the cursor based on the interaction state
    private void OnInteractionStateChange(EventData eventData)
    {
        //Return statement if cursor manager is disabled
        if (isDisabled) { return; }

        //Check if it is the correct event and execute
        if (eventData is ActivateInteractionState)
        {
            //Cast the event for ease of use
            ActivateInteractionState interactionState = (ActivateInteractionState)eventData;

            //Set the current state
            currentInteractionState = interactionState.interactionState;

            //Update the projector
            UpdateProjector();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.ACTIVATEINTERACTIONSTATE was received but is not of class ActivateInteractionState.");
        }
    }

    //Listen out for gameover in order to force the cursor to remain as inactive
    private void OnGameOver(EventData eventData)
    {
        if (eventData is GameOver)
        {
            //Force the state and disable incoming changes
            isDisabled = true;
            currentInteractionState = PlayerInteractionState.inactive;

            //Update the projector
            UpdateProjector();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.GAMEOVER was received but is not of class GameOver.");
        }
    }
}
