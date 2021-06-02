using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The types of scores, used for mission score filtering
public enum ScoreType
{
    Biodiversity = 0,
    CarbonIntake,
    Appeal,
    Invasiveness,
    Infections,
    TotalScore
}

//Abstract class that provides core/shared methods for score related missions
public abstract class ScoreMission : Mission
{
    [SerializeField] protected private ScoreType targettedScore; //The score type that is permitted/tracked for the mission
    protected private int currentScore = 0; //The current score of the mission
    protected private bool missionHasInitialized = false; //Important bool so that the first incoming scores are considered as the mission starting point

    protected private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.TOTALSCORESUPDATED, OnMissionConditon); //Process incoming score data

        //Fire off event requesting a specific score
        EventManager.currentManager.AddEvent(new RequestScoreData());
    }
    protected private void OnDisable()
    {
        //Unsubscribes the method and event type to the current manager
        EventManager.currentManager.Unsubscribe(EventType.TOTALSCORESUPDATED, OnMissionConditon);
    }
    protected private int GetFilteredScore(TotalScoresUpdated scoreData)
    {
        //Gets the filtered targetted score from the given score data
        switch (targettedScore)
        {
            case ScoreType.Biodiversity:
                return scoreData.biodiversity;

            case ScoreType.CarbonIntake:
                return scoreData.carbonIntake;

            case ScoreType.Appeal:
                return scoreData.appealScore;

            case ScoreType.Infections:
                return scoreData.infections;

            case ScoreType.Invasiveness:
                return scoreData.invasiveness;

            case ScoreType.TotalScore:
                return scoreData.totalScore;

            default:
                return 0;
        }
    }
}
