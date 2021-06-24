using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField]
    //Text panels that are meant to visualize the recieved data/changes in player scores
    private TextMeshProUGUI biodiversityText = null, carbonIntakeText = null, appealText = null, moneyText = null, invasivenessText = null, diseaseText = null;

    private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.TOTALSCORESUPDATED, OnScoresUpdated);

    }
    private void OnDisable()
    {
        //Unsubscribes the method and event type from the current manager
        EventManager.currentManager.Subscribe(EventType.TOTALSCORESUPDATED, OnScoresUpdated);
    }

    private void OnScoresUpdated(EventData eventData)
    {
        if (eventData is TotalScoresUpdated)
        {
            //Cast the event so it can be used
            TotalScoresUpdated updatedScores = (TotalScoresUpdated)eventData;

            //Updates all the score view text to match the scores
            biodiversityText.text = updatedScores.biodiversity.ToString();
            carbonIntakeText.text = updatedScores.carbonIntake.ToString();
            appealText.text = updatedScores.appealScore.ToString();
            moneyText.text = updatedScores.money.ToString();
            invasivenessText.text = updatedScores.invasiveness.ToString();
            diseaseText.text = updatedScores.infections.ToString();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.TOTALSCORESUPDATED was received but is not of class TotalScoresUpdated.");
        }
    }
}
