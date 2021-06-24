using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

[System.Serializable]
public abstract class Mission : MonoBehaviour
{
    [Header("Mission Details and Requirements")]
    protected private string missionDescription = "Mission Description"; //The message that should explain the mission
    [SerializeField]
    protected private int missionReward = 0; //The amount of money to be earned after completing a mission
    [SerializeField]
    protected private bool isEndOfWeekReward = false; //Designates if a mission can be completed before or at the end of a week
    [SerializeField]
    [Range(1,100)]
    protected private int appearsFromWeek = 1; //Designates during what week a mission is able to appear at
    protected private bool isMissionComplete = false; //Bool that states wether a mission has been completed or not
    protected private string progressText; //String that states the progress of the current mission
    protected private float progressFraction; //Fraction of the mission that is complete 0-1
    protected private bool isInverseProgressRelationship = false; //Represents if the mission has "inverse" progress for the bar

    //Reset the mission parameters
    public virtual void ResetMission()
    {
        isMissionComplete = false;

        //Update the progress text
        UpdateProgressText();

        //Update the progress fraction
        UpdateProgressFraction();
    }

    #region Getters
    //Gets the bool for if the mission is complete
    public bool GetIsMissionComplete()
    {
        return isMissionComplete;
    }

    //Gets the mission reward value
    public int GetMissionReward()
    {
        return missionReward;
    }

    //Gets the description of the mission
    public string GetMissionDescription()
    {
        return missionDescription;
    }

    //Gets the progress text
    public string GetProgressText()
    {
        return progressText;
    }

    //Gets the progress fractional value
    public float GetProgressFraction()
    {
        return progressFraction;
    }

    //Gets the week that the mission appears in
    public int GetAppearsFromWeek()
    {
        return appearsFromWeek;
    }

    //Gers if the relationship of the progress is inverse
    public bool GetIsInverseProgress()
    {
        return isInverseProgressRelationship;
    }
    #endregion

    //Sets the mission completion state
    public void SetMissionComplete(bool state)
    {
        isMissionComplete = state;
    }

    //Sets the progress of a mission
    public void SetProgressText(string givenText)
    {
        progressText = givenText;
    }

    //Abstract format update for progress text as missions vary
    public abstract void UpdateProgressText();

    //Abstract format for the progress fraction to be calculated
    public abstract void UpdateProgressFraction();

    //Abstract method as different missions will have different conditions for mission completion
    protected private abstract void OnMissionConditon(EventData eventData);

    //Check for passed week and if reward should be fired off
    public void OnWeekPassedReward()
    {
        //Check if completed and is week end task
        if (isEndOfWeekReward && isMissionComplete)
        {
            //Fire off reward
            FireMissionCompletedReward();
        }
    }

    //Fire off an event with the mission rewards
    public void FireMissionCompletedReward()
    {
        //Fire off an event with the reward
        EventManager.currentManager.AddEvent(new MissionCompleted(missionReward));
    }

    //Update description based on language
    public void UpdateDescriptionLanguage(LocalizeStringEvent localizeStringEvent)
    {
        //Error handling
        if (localizeStringEvent != null)
        {
            //Update the description
            missionDescription = localizeStringEvent.StringReference.GetLocalizedString();

            //Inform subscribers (Ideally mission manager) of changes
            EventManager.currentManager.AddEvent(new MissionUpdated());
        }
        else
        {
            Debug.LogWarning("No localized string event attached correctly, are you sure you set it up?");
        }
    }
}