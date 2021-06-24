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
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI biodiversityText;
    [SerializeField] private TextMeshProUGUI carbonIntakeText;
    [SerializeField] private TextMeshProUGUI appealText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private PlaceableData placeable;
    private Tooltip tooltip;
    //------------------------------------------------------------------------------------------------------------------------
    //                                                  Initialize()
    //------------------------------------------------------------------------------------------------------------------------
    public void Initialize(PlaceableData placeable)
    {
        //setup tooltip   
        tooltip = GetComponent<Tooltip>();
        if (tooltip != null)
        {
            string outputText="";
            foreach (string textLine in placeable.GetSummarisedText())
            {
                outputText += "\u2022"+textLine+"\n";
            }
            tooltip.SetDisplayText(outputText);
        }
        else
        {
            Debug.LogWarning("No tooltip found, extra info will not be displayed");
        }
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
        //name text replace
        if (nameText != null)
        {
            nameText.text = placeable.GetName();
        }
        else
        {
            Debug.LogWarning("biodiversity text reference was not set");
        }
        //bio text replace
        if (biodiversityText != null)
        {
            biodiversityText.text = "+" + placeable.GetBiodiversity().ToString();
        }
        else
        {
            Debug.LogWarning("biodiversity text reference was not set");
        }
        //carbon text replace
        if (carbonIntakeText != null)
        {
            carbonIntakeText.text = "+" + placeable.GetCarbonIntake().ToString();
        }
        else
        {
            Debug.LogWarning("carbonIntake text reference was not set");
        }
        //appeal text replace
        if (appealText != null)
        {
            appealText.text = "+" + placeable.GetAppeal().ToString();
        }
        else
        {
            Debug.LogWarning("appeal text reference was not set");
        }
        //money text replace
        if (moneyText != null)
        {
            moneyText.text = "-" + placeable.GetCost().ToString();
        }
        else
        {
            Debug.LogWarning("money text reference was not set");
        }
    }
}
