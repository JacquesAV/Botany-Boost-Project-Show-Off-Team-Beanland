using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolkitButton : MonoBehaviour
{
    //The state that it represents
    public PlayerInteractionState givenState = PlayerInteractionState.inactive;

    public void FireOffStateChangeRequest()
    {
        //Fire off an event with the requested interaction state change
        EventManager.currentManager.AddEvent(new ActivateInteractionState(givenState));
    }
}
