using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminateScoreMission : ScoreMission
{
    //Reset the mission parameters
    public override void ResetMission()
    {
        //Base reset
        base.ResetMission();

        //Set to inverse progress relationship
        isInverseProgressRelationship = true;

        //Reset current progress
        currentScore = 0;
        missionHasInitialized = false;

        //Update the progress text
        UpdateProgressText();

        //Update the progress fraction
        UpdateProgressFraction();
    }

    //Custom set progress text format
    public override void UpdateProgressText()
    {
        SetProgressText(currentScore.ToString());
    }

    //Custom set progress fraction
    public override void UpdateProgressFraction()
    {
        //Cast in order to allow for 0 division
        progressFraction = (float)(double) currentScore / startingScore;
    }

    //Override method as different missions will have different conditions for mission completion
    protected private override void OnMissionConditon(EventData eventData)
    {
        //Ignore if mission is already complete, UNLESS it can still be affected (isEndWeek) 
        if (isMissionComplete && !isEndOfWeekReward) { return; }

        //If positive addition
        if (eventData is TotalScoresUpdated)
        {
            //Cast the event so it can be used
            TotalScoresUpdated scoreData = (TotalScoresUpdated)eventData;

            //Get the intended current score
            currentScore = GetFilteredScore(scoreData);

            //If the mission has not initialized with the correct values yet
            if (!missionHasInitialized)
            {
                InitializeMission(currentScore);
            }

            //Check if completion condition is met
            if (currentScore == 0)
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

            //Check if score went back above 0
            if(isEndOfWeekReward && isMissionComplete && currentScore > 0)
            {
                //Set the mission as complete
                SetMissionComplete(false);
            }

            //Update the progress text
            UpdateProgressText();
            UpdateProgressFraction();

            //Inform subscribers (Ideally mission manager) of changes
            EventManager.currentManager.AddEvent(new MissionUpdated());
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.TOTALSCORESUPDATED was received but is not of class TotalScoresUpdated.");
        }
    }
}
