using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Mission : MonoBehaviour
{
    [SerializeField]
    protected private string missionDescription = "Sample Description"; //The message that should explain the mission
    [SerializeField]
    protected private int missionReward = 0; //The amount of money to be earned after completing a mission
    private bool isMissionComplete = false; //Bool that states wether a mission has been completed or not

    //Gets the bool for if the mission is complete
    public void ResetMission()
    {
        isMissionComplete = false;
    }

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

    //Sets the mission completion state
    public void SetMissionComplete(bool state)
    {
        isMissionComplete = state;
    }

    private void OnMissionCompletionConditon(EventData eventData)
    {

    }

    private void MissionCompleted()
    {

    }
}