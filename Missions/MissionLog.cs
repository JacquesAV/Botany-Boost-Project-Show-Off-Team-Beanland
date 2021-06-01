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
    //Checkbox that gets image changed
    private GameObject checkboxObject=null;

    [SerializeField]
    //Image for the checkbox to toggle
    private Sprite uncheckedBox = null, checkedBox = null;

    //Initialize the log
    public void InitializeLogVisual(Mission mission)
    {
        //Set the correct text
        moneyRewardText.SetText(mission.GetMissionReward().ToString());
        descriptionText.SetText(mission.GetMissionDescription().ToString());
        progressText.SetText(mission.GetProgressText());
        checkboxObject.GetComponent<Image>().sprite = uncheckedBox;

        //Update the checkbox
        if (mission.GetIsMissionComplete())
        {
            checkboxObject.GetComponent<Image>().sprite = checkedBox;
        }
        else
        {
            checkboxObject.GetComponent<Image>().sprite = uncheckedBox;
        }
    }
}
