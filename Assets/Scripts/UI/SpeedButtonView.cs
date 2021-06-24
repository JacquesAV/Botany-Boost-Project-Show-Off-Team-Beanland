using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButtonView : MonoBehaviour
{
    //Dictinary of the buttons and speed icons
    public List<SpeedButtonImageToggle> speedButtonToggles;

    public void UpdateVisuals(SpeedButtonImageToggle clickedButton)
    {
        //Iterate over each and update
        foreach(SpeedButtonImageToggle toggle in speedButtonToggles)
        {
            //If the calling button, toggle active
            if(clickedButton == toggle)
            {
                toggle.ToggleActiveSprite(true);
            }
            //Else toggle inactive
            else
            {
                toggle.ToggleActiveSprite(false);
            }
        }
    }
}
