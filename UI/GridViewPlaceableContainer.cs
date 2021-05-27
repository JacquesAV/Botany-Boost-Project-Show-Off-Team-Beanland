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

        if (sprite != null)
        {
            icon.sprite = sprite;
        }

        biodiversityText.text = placeable.GetBiodiversity().ToString();
        carbonIntakeText.text = placeable.GetCarbonIntake().ToString();
        appealText.text = placeable.GetAppeal().ToString();
        moneyText.text = placeable.GetCost().ToString();
    }
}
