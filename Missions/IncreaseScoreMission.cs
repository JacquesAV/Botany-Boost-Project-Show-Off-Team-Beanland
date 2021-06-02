using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseScoreMission : ScoreMission
{
    [SerializeField] protected private int scoreIncreaseGoal = 0; //The goal for the score increase
    private int startingScore = 0; //The starting score of the player in the mission

    //Reset the mission parameters
    public override void ResetMission()
    {
        //Base reset
        base.ResetMission();

        //Reset current progress
        currentScore = 0;
        startingScore = 0;
        missionHasInitialized = false;

        //Update the progress text
        UpdateProgressText();
    }

    //Custom set progress text format
    public override void UpdateProgressText()
    {
        SetProgressText("Progress: " + currentScore + "/" + scoreIncreaseGoal);
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
            int currentScoreData = GetFilteredScore(scoreData);

            //If the mission has not initialized with the correct values yet, do so and return
            if (!missionHasInitialized)
            {
                InitializeMission(currentScoreData);
                UpdateProgressText();
                return;
            }

            //Get the difference between the current score and starting score for the difference
            currentScore = currentScoreData - startingScore;

            //Check if completion condition is met
            if (currentScore >= scoreIncreaseGoal)
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

            //Check if mission went below 0 on score increase
            if(currentScore < 0)
            {
                //Reset with new starting value
                InitializeMission(scoreData);
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

    private void InitializeMission(int givenStartingScore)
    {
        //Set the starting score
        startingScore = givenStartingScore;

        //Reset the current score increase
        currentScore = 0;

        //Mark the mission as initialized
        missionHasInitialized = true;
    }

    private void InitializeMission(TotalScoresUpdated scoreData)
    {
        InitializeMission(GetFilteredScore(scoreData));
    }
}
