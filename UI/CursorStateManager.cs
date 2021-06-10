using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStateManager : MonoBehaviour
{
    //Visuals of the intended cursor state
    public Texture2D buildingCursor, destroyingCursor, curingCursor, gassingCursor, inactiveCursor;
    private bool isDisabled = false;
    public PlayerInteractionState currentInteractionState;

    private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.ACTIVATEINTERACTIONSTATE, OnInteractionStateChange);
        EventManager.currentManager.Subscribe(EventType.GAMEOVER, OnGameOver);
    }

    private void OnDisable()
    {
        //Unsubscribes the method and event type to the current manager
        EventManager.currentManager.Unsubscribe(EventType.ACTIVATEINTERACTIONSTATE, OnInteractionStateChange);
        EventManager.currentManager.Unsubscribe(EventType.GAMEOVER, OnGameOver);
    }

    //Manually change the cursor without consideration of the currently set state
    private void UpdateMouseCursor(PlayerInteractionState givenState)
    {
        //Apply the state change based on the input
        switch (givenState)
        {
            case PlayerInteractionState.building:
                //Set cursor icon
                Cursor.SetCursor(buildingCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
                break;

            case PlayerInteractionState.destroying:
                //Set cursor icon
                Cursor.SetCursor(destroyingCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
                break;

            case PlayerInteractionState.curing:
                //Set cursor icon
                Cursor.SetCursor(curingCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
                break;

            case PlayerInteractionState.gassing:
                //Set cursor icon
                Cursor.SetCursor(gassingCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
                break;

            case PlayerInteractionState.inactive:
                //Set cursor icon
                Cursor.SetCursor(inactiveCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
                break;
        }
    }

    //Update using the active
    private void UpdateMouseCursor()
    {
        UpdateMouseCursor(currentInteractionState);
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

            //Update the cursor
            UpdateMouseCursor();
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

            //Update the cursor
            UpdateMouseCursor();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.GAMEOVER was received but is not of class GameOver.");
        }
    }
}
