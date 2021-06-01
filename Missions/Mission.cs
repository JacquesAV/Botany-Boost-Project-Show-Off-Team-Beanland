using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Mission : MonoBehaviour
{
    [Header("Mission Details")]
    [SerializeField]
    protected private string missionDescription = "Mission Description"; //The message that should explain the mission
    [SerializeField]
    protected private int missionReward = 0; //The amount of money to be earned after completing a mission
    [SerializeField]
    protected private bool isEndOfWeekReward = false; //Designates if a mission can be completed before or at the end of a week
    protected private bool isMissionComplete = false; //Bool that states wether a mission has been completed or not
    protected private string progressText; //String that states the progress of the current mission

    //Reset the mission parameters
    public virtual void ResetMission()
    {
        isMissionComplete = false;
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

    //Virtual format update for progress text as missions vary
    public abstract void UpdateProgressText();

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
}