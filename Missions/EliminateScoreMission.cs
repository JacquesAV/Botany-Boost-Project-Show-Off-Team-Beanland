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

        //Reset current progress
        currentScore = 0;

        //Update the progress text
        UpdateProgressText();
    }

    //Custom set progress text format
    public override void UpdateProgressText()
    {
        SetProgressText("Remaining: " + currentScore);
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

            //Update the progress text
            UpdateProgressText();

            //Inform subscribers (Ideally mission manager) of changes
            EventManager.currentManager.AddEvent(new MissionUpdated());
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.TOTALSCORESUPDATED was received but is not of class TotalScoresUpdated.");
        }
    }
}
