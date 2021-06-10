using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionLog : MonoBehaviour
{
    [SerializeField]
    //Text panels that are meant to visualize the mission
    private TextMeshProUGUI moneyRewardText = null, descriptionText = null, progressText = null;

    [SerializeField]
    //Progress bar using a slider
    private SliderProgressBar progressSlider = null;

    [SerializeField]
    //Colors based on the bar completion relationship and state
    private Color completionColor, inverseCompletionColor;

    //Initialize the log
    public void InitializeLogVisual(Mission mission)
    {
        //Set the correct text
        moneyRewardText.SetText(mission.GetMissionReward().ToString());
        descriptionText.SetText(mission.GetMissionDescription().ToString());
        progressText.SetText(mission.GetProgressText());

        //Change the progress bar and order of colors based on the completion relationship
        progressSlider.SetTargetProgress(mission.GetProgressFraction());
        if (mission.GetIsInverseProgress())
        {
            //Update the colors
            progressSlider.SetBackgroundColor(completionColor);
            progressSlider.SetBarColor(inverseCompletionColor);
        }
        else
        {
            //Update the colors
            progressSlider.SetBarColor(completionColor);
        }

        //Check if the mission is complete
        if (mission.GetIsMissionComplete())
        {
            //Set to strikethrough font style
            descriptionText.fontStyle = FontStyles.Strikethrough;
        }
        else
        {
            //Set to normal font style
            descriptionText.fontStyle = FontStyles.Normal;
        }
    }
}
