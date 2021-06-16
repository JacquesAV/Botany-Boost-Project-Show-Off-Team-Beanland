using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementMission : Mission
{
    [SerializeField] private PlaceableType mustBeObjectType = PlaceableType.None; //The object type that is permitted/tracked for the mission
    [SerializeField] private InsectType mustAttractInsectType = InsectType.None; //If the goal should attract insects
    [SerializeField] private bool mustYieldProduce; //If the goal should produce food
    [SerializeField] private bool mustAttractBirds; //If the goal should attract birds
    [SerializeField] protected private int placementGoal = 0; //The number of objects that must be placed
    private int currentPlacements = 0; //The number of objects that must be placed

    protected private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.MISSIONDATA, OnMissionConditon); //Process objects that were bought/sold
    }
    protected private void OnDisable()
    {
        //Unsubscribes the method and event type to the current manager
        EventManager.currentManager.Unsubscribe(EventType.MISSIONDATA, OnMissionConditon);
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
        SetProgressText(currentPlacements + "/" + placementGoal);
    }

    //Custom set progress fraction
    public override void UpdateProgressFraction()
    {
        //Cast in order to allow for 0 division
        progressFraction = (float)(double)currentPlacements / placementGoal;
    }

    //Override method as different missions will have different conditions for mission completion
    protected private override void OnMissionConditon(EventData eventData) 
    {
        //Ignore if mission is already complete, UNLESS it can still be affected (isEndWeek) 
        if (isMissionComplete && !isEndOfWeekReward) { return; }

        //If positive addition
        if (eventData is PlacedObjectMissionData)
        {
            //Cast the event so it can be used
            PlacedObjectMissionData objectScores = (PlacedObjectMissionData)eventData;

            //Check if the object is of the correct type (Undefined requirement allows for any objects!)
            if(mustBeObjectType == PlaceableType.None || objectScores.placeableType == mustBeObjectType)
            {
                //Check if the object is of the correct insect type (None requirement allows for any objects!)
                if (mustAttractInsectType == InsectType.None || objectScores.insectType == mustAttractInsectType)
                {
                    //Check if the object is meant to yield produce 
                    if (mustYieldProduce == false || objectScores.yieldsProduce == mustYieldProduce)
                    {
                        //Check if the object is meant to attract birds
                        if (mustAttractBirds == false || objectScores.attractsBirds == mustAttractBirds)
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

                                //Prevent the counter from going over the goal
                                currentPlacements = placementGoal;
                            }

                            //Update the progress text
                            UpdateProgressText();
                            UpdateProgressFraction();

                            //Inform subscribers (Ideally mission manager) of changes
                            EventManager.currentManager.AddEvent(new MissionUpdated());
                        }
                    }
                }
            }
        }
        //If negative addition
        else if(eventData is RemovedObjectMissionData)
        {
            //Cast the event so it can be used
            RemovedObjectMissionData objectScores = (RemovedObjectMissionData)eventData;

            //Check if the object is of the correct type (Undefined requirement allows for any objects!)
            if (mustBeObjectType == PlaceableType.None || objectScores.placeableType == mustBeObjectType)
            {
                //Check if the object is of the correct insect type (None requirement allows for any objects!)
                if (mustAttractInsectType == InsectType.None || objectScores.insectType == mustAttractInsectType)
                {
                    //Check if the object is meant to yield produce 
                    if (mustYieldProduce == false || objectScores.yieldsProduce == mustYieldProduce)
                    {
                        //Check if the object is meant to attract birds
                        if (mustAttractBirds == false || objectScores.attractsBirds == mustAttractBirds)
                        {
                            //Remove from the current placements
                            currentPlacements--;

                            //Prevent from going under 0
                            if (currentPlacements < 0)
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
                            UpdateProgressFraction();

                            //Inform subscribers (Ideally mission manager) of changes
                            EventManager.currentManager.AddEvent(new MissionUpdated());
                        }
                    }
                }
            }
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.MISSIONDATA was received but is not of class ObjectMissionData or its derived classes.");
        }
    }
}
