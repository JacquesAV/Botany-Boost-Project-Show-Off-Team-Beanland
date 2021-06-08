using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExtraPlaceableInfo : MonoBehaviour
{
    [SerializeField] private GameObject extraInfoPanel;
    [SerializeField] private Image displayImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI extraInfo;

    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.CLICKEDPLACEABLEGUI, OnPlaceableUISelect);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.CLICKEDPLACEABLEGUI, OnPlaceableUISelect);
    }
    
    private void OnPlaceableUISelect(EventData eventData)
    {
        
        if (eventData is PlaceableSelectedOnGUI)
        {
            //Adds the event data received to a class for use 
            PlaceableSelectedOnGUI placeableGUISelect = (PlaceableSelectedOnGUI)eventData;

            if (extraInfoPanel != null)
            {
                extraInfoPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No extra info panel was set, this is required.");
                return;
            }

            if (displayImage != null)
            {
                displayImage.sprite = placeableGUISelect.placeable.GetDisplayImage();
            }
            else
            {
                Debug.LogWarning("No display image target object was set.");
            }

            if (nameText != null)
            {
                nameText.text = placeableGUISelect.placeable.GetName();
            }
            else
            {
                Debug.LogWarning("No plant name text was set, this is required.");
            }

            if (extraInfo != null)
            {
                extraInfo.text = placeableGUISelect.placeable.GetExtraInfoText();
            }
            else
            {
                Debug.LogWarning("No extra info text was set, this is required.");
            }
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.CLICKEDPLACEABLEGUI was received but is not of class PlaceableSelectedOnGUI.");
        }
    }
}
