using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour, IPointerClickHandler
{
    //private PlaceableData placeableToSelect;//The item that will be selected the next time HandleInput is called

    //------------------------------------------------------------------------------------------------------------------------
    //                                                  OnPointerClick()
    //------------------------------------------------------------------------------------------------------------------------ 
    //This method is called everytime the view receives a mouse click event
    public void OnPointerClick(PointerEventData eventData)
    {
        //Check if the mouse clicked on an placeable container. 
        IPlaceableContainer placeableContainer = eventData.pointerCurrentRaycast.gameObject.GetComponent<IPlaceableContainer>();

        //If the game object that was clicked on has a component which implements the IItemContainer interface, PlaceableContainer
        //will be assigned with that component, otherwise PlaceableContainer would be null, meaning the mouse didn't click on any
        //item containers in the view. If you are clicking on the item container but it still returns null, check to see if there
        //is any other UI elements blocking the raycast, if so mark their 'Raycast Target' property as false in Unity Editor
        if (placeableContainer != null)
        {
            //placeableToSelect = placeableContainer.Placeable;//Use IPlaceableContainer's getter to get the item in the container, assign it to placeableToSelect,
                                                             //now it will be selected the next time HandleInput is called
            EventManager.currentManager.AddEvent(new PlaceableSelectedOnGUI(placeableContainer.Placeable));
        }
    }
}
