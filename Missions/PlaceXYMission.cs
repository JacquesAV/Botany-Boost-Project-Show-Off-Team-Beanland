using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceXYMission : Mission
{
    [SerializeField]
    protected private PlaceableType objectType = PlaceableType.Undefined; //The object type that is permitted/tracked for the mission
    [SerializeField]
    protected private int placementGoal = 0; //The number of objects that must be placed
    private int currentPlacements = 0; //The number of objects that must be placed

    protected private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.MISSIONDATAXYZ, OnMissionConditon); //Process objects that were bought/sold
    }
    protected private void OnDisable()
    {
        //Unsubscribes the method and event type to the current manager
        EventManager.currentManager.Unsubscribe(EventType.MISSIONDATAXYZ, OnMissionConditon);
    }

    //Reset the mission parameters
    public override void ResetMission()
    {
        //base reset
        base.ResetMission();

        //Reset current progress
        currentPlacements = 0;

        //Update the progress text
        UpdateProgressText();
    }

    //Custom set progress text format
    public override void UpdateProgressText()
    {
        SetProgressText("Progress: " + currentPlacements + "/" + placementGoal);
    }

    //Override method as different missions will have different conditions for mission completion
    protected private override void OnMissionConditon(EventData eventData) 
    {
        //Ignore if mission is already complete, UNLESS it can still be affected (isEndWeek) 
        if (isMissionComplete && !isEndOfWeekReward) { return; }

        //If positive addition
        if (eventData is PlacedObjectMissionDataXYZ)
        {
            //Cast the event so it can be used
            PlacedObjectMissionDataXYZ objectScores = (PlacedObjectMissionDataXYZ)eventData;

            //Check if the object is of the correct type (Undefined requirement allows for any objects!)
            if(objectType == PlaceableType.Undefined || objectScores.placeableType == objectType)
            {
                //Add to the current placements
                currentPlacements++;

                //Check if completion condition is met
                if (currentPlacements >= placementGoal)
                {
                    //Set the mission as complete
                    SetMissionComplete(true);

                    //If the reward is immediate, fire off immediately
                    if (!isEndOfWeekReward)
                    {
                        //Fire off the event
                        FireMissionCompletedReward();
                    }
                }

                //Update the progress text
                UpdateProgressText();

                //Inform subscribers (Ideally mission manager) of changes
                EventManager.currentManager.AddEvent(new MissionUpdated());
            }
        }
        //If negative addition
        else if(eventData is RemovedObjectMissionDataXYZ)
        {
            //Cast the event so it can be used
            RemovedObjectMissionDataXYZ objectScores = (RemovedObjectMissionDataXYZ)eventData;

            //Check if the object is of the correct type (Undefined requirement allows for any objects!)
            if (objectType == PlaceableType.Undefined || objectScores.placeableType == objectType)
            {
                //Remove from the current placements
                currentPlacements--;

                //Prevent from going under 0
                if(currentPlacements < 0)
                {
                    currentPlacements = 0;
                }

                //Check if completion condition is not valid
                if (currentPlacements < placementGoal)
                {
                    //Set the mission as incomplete
                    SetMissionComplete(false);
                }

                //Update the progress text
                UpdateProgressText();

                //Inform subscribers (Ideally mission manager) of changes
                EventManager.currentManager.AddEvent(new MissionUpdated());
            }
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.MISSIONDATAXYZ was received but is not of class ObjectMissionDataXYZ or its derived classes.");
        }
    }
}
