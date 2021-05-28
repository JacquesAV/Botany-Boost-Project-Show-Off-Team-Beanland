using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;

public class GridViewPlaceableContainer : MonoBehaviour, IPlaceableContainer
{
    public PlaceableData Placeable => placeable;//Public getter for the placeable, required by IPlaceableContainer interface.

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI biodiversityText;
    [SerializeField] private TextMeshProUGUI carbonIntakeText;
    [SerializeField] private TextMeshProUGUI appealText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private PlaceableData placeable;

    //------------------------------------------------------------------------------------------------------------------------
    //                                                  Initialize()
    //------------------------------------------------------------------------------------------------------------------------
    public void Initialize(PlaceableData placeable)
    {
        //stores the item
        this.placeable = placeable;

        Sprite sprite = placeable.GetDisplayImage();
        //Sprite replace
        if (sprite != null && icon != null)
        {
            icon.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Warning: Could not set icon for placeable image");
        }
        //bio text replace
        if (biodiversityText != null)
        {
            biodiversityText.text = placeable.GetBiodiversity().ToString();
        }
        else
        {
            Debug.LogWarning("biodiversity text reference was not set");
        }
        //carbon text replace
        if (carbonIntakeText != null)
        {
            carbonIntakeText.text = placeable.GetCarbonIntake().ToString();
        }
        else
        {
            Debug.LogWarning("carbonIntake text reference was not set");
        }
        //appeal text replace
        if (appealText != null)
        {
            appealText.text = placeable.GetAppeal().ToString();
        }
        else
        {
            Debug.LogWarning("appeal text reference was not set");
        }
        //money text replace
        if (moneyText != null)
        {
            moneyText.text = placeable.GetCost().ToString();
        }
        else
        {
            Debug.LogWarning("money text reference was not set");
        }
    }
}
