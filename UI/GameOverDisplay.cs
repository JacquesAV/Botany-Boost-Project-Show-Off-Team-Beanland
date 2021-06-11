using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverDisplay : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject finishScreen;
    [SerializeField] private GameObject resolutionScreen;
    [SerializeField] private GameObject leaderboardScreen;

    [Header("Base Scores")]
    [SerializeField] private TextMeshProUGUI biodiversityText;
    [SerializeField] private TextMeshProUGUI appealText;
    [SerializeField] private TextMeshProUGUI carbonIntakeText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI invasivenessText;
    [SerializeField] private TextMeshProUGUI diseaseText;

    [Header("Total Scores")]
    [SerializeField] private TextMeshProUGUI TotalScoreFinishScreen;
    [SerializeField] private TextMeshProUGUI TotalScoreResolutionScreen;
    [SerializeField] private TextMeshProUGUI TotalScoreLeaderboardScreen;



    private bool brokeRecord = false;

    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.GAMEOVER, OnGameOver);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.GAMEOVER, OnGameOver);
    }

    private void OnGameOver(EventData eventData)
    {
        //receives game over an reqests scores
        if (eventData is GameOver)
        {
            finishScreen.SetActive(true);
            EventManager.currentManager.AddEvent(new RequestScoreData());
            EventManager.currentManager.Subscribe(EventType.TOTALSCORESUPDATED, OnScoresUpdated);
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.GAMEOVER was received but is not of class GameOver.");
        }
    }

    private void OnScoresUpdated(EventData eventData)
    {
        //Cast the event so it can be used
        if (eventData is TotalScoresUpdated updatedScores)
        {

            //Get all scores
            int bio = updatedScores.biodiversity;
            int carb = updatedScores.carbonIntake;
            int appeal = updatedScores.appealScore;
            int money = updatedScores.money;
            int inva = updatedScores.invasiveness;
            int infec = updatedScores.infections;

            //Display and calculate scoring
            CalculateScores(bio, carb, appeal, money, inva, infec);
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.TOTALSCORESUPDATED was received but is not of class TotalScoresUpdated.");
        }
    }

    private void CalculateScores(int bio, int carb, int appeal, int money, int inva, int infec)
    {
        //Display base scores
        #region Text screen updates
        //bio
        if (biodiversityText != null)
        {
            biodiversityText.text = bio.ToString();
        }
        else
        {
            Debug.LogWarning("Bio text was not selected for finished score screen, please do so");
        }

        //carbon
        if (carbonIntakeText != null)
        {
            carbonIntakeText.text = carb.ToString();
        }
        else
        {
            Debug.LogWarning("carbon text was not selected for finished score screen, please do so");
        }

        //appeal
        if (appealText != null)
        {
            appealText.text = appeal.ToString();
        }
        else
        {
            Debug.LogWarning("appeal text was not selected for finished score screen, please do so");
        }

        //money
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
        else
        {
            Debug.LogWarning("money text was not selected for finished score screen, please do so");
        }

        //invasive
        if (invasivenessText != null)
        {
            invasivenessText.text = inva.ToString();
        }
        else
        {
            Debug.LogWarning("invasive text was not selected for finished score screen, please do so");
        }

        //disease
        if (diseaseText != null)
        {
            diseaseText.text = infec.ToString();
        }
        else
        {
            Debug.LogWarning("disease text was not selected for finished score screen, please do so");
        }
        #endregion
        //calculate total score
        int totalScore = bio + carb + appeal + money - (inva * 10) - (infec * 10);

        //display on text
        #region total score text updates
        //Score on finish screen

        if (TotalScoreFinishScreen != null)
        {
            TotalScoreFinishScreen.text = totalScore.ToString();
        }
        else
        {
            Debug.LogWarning("total score finish text was not selected for finished score screen, please do so");
        }

        //Score on resolution screen
        if (TotalScoreResolutionScreen != null)
        {
            TotalScoreResolutionScreen.text = "Total: " + totalScore.ToString();
        }
        else
        {
            Debug.LogWarning("total score resolution text was not selected for finished score screen, please do so");
        }

        if (TotalScoreLeaderboardScreen != null)
        {
            TotalScoreLeaderboardScreen.text = totalScore.ToString(); ;
        }
        else
        {
            Debug.LogWarning("total score leaderboard text was not selected for finished score screen, please do so");
        }
        #endregion

        //check if its a top 10 score
        brokeRecord = CheckIfEligibleForLeaderboard(totalScore);
    }

    private bool CheckIfEligibleForLeaderboard(int totalScore)
    {
        //fetch the leaderboard scores and check if its better than the 10th place
        int tenthPlaceScore = 1000;
        if (totalScore > tenthPlaceScore)
        {
            return true;
        }
        return false;
    }

    public void GoToResolutionScreen()
    {
        if (brokeRecord)
        {
            leaderboardScreen.SetActive(true);
        }
        else
        {
            resolutionScreen.SetActive(true);
        }
    }
}
