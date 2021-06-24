using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerScoreManager : MonoBehaviour
{
    //Variables that are tracked in order to calculate score
    [SerializeField] private int totalMoney = 50; //Total money the user has to spend
    [Range(0, 1000)] [SerializeField] private int weeklyMoneyGain = 100; 
    [SerializeField] private int curingCost = 50; //Cost per cure attempt
    [SerializeField] private int gassingCost = 50; //Cost per gas attempt
    [SerializeField] [Range(0.1f,1f)] private float refundRation = 0.5f; //Selling ratio/refund
    [SerializeField] private int scoreThreshold=120;//Threshold for losing game
    [SerializeField] private int beeThreshold = 50, butterflyThreshold = 50, beetleThreshold = 50; //Thresholds for bees/butterflies/beetles to appear at
    private bool reachedBeeThreshold = false, reachedButterflyThreshold = false, reachedBeetleThreshold = false;
    private int totalBiodiversity = 0; //Total biodiversity, based on plants and insect attractiveness
    private int objectBiodiversity = 0; //Biodiversity, based on objects
    private int beeScore = 0; //Insect attractiveness (insect biodiversity)
    private int butterflyScore = 0; //Insect attractiveness (insect biodiversity)
    private int beetleScore = 0; //Insect attractiveness (insect biodiversity)
    private int totalCarbonIntake = 0; //Total carbon intake score, is not influenced by others
    private int totalAppeal = 0; //Total attractiveness of the objects in general, more of a "human" likeability score
    private int totalInvasiveness = 0; //Level of invasiveness, calculated from the number of negatively affected plants
    private int totalInfections = 0; //Level of disease, calculated from the number of negatively affected plants
    private int totalScore = 0; //Total combination of all scores

    private int currentWeek = 1;
    private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.OBJECTBOUGHTSCORES, OnObjectBoughtScores); //Process objects that were bought
        EventManager.currentManager.Subscribe(EventType.OBJECTSOLDSCORES, OnObjectSoldScores); //Process sold object
        EventManager.currentManager.Subscribe(EventType.OBJECTBUYREQUEST, OnObjectBuyRequest); //Process a buy request
        EventManager.currentManager.Subscribe(EventType.PLANTINVADED, OnPlantInvaded); //Increase invaded plant count
        EventManager.currentManager.Subscribe(EventType.PLANTGASSED, OnPlantGassed); //Decrease invaded plant count
        EventManager.currentManager.Subscribe(EventType.PLANTINFECTED, OnPlantInfected); //Increase sick plant count
        EventManager.currentManager.Subscribe(EventType.PLANTCURED, OnPlantCured); //Decrease sick plant count
        EventManager.currentManager.Subscribe(EventType.PLANTCUREGASREQUEST, OnCureGasRequest); //Process a cure/gas request
        EventManager.currentManager.Subscribe(EventType.MISSIONCOMPLETED, OnMissionComplete); //Increase money count based on reward
        EventManager.currentManager.Subscribe(EventType.MoneyEarned, OnMoneyEarned); //Increase money count based on given
        EventManager.currentManager.Subscribe(EventType.REQUESTSCOREDATA, OnScoreRequest); //Process when a request for the scores
        EventManager.currentManager.Subscribe(EventType.WEEKLYSCOREINCREASE, OnWeekScoreIncrease); //Process when week finishes
    }
    private void OnDisable()
    {
        //Unsubscribes the method and event type from the current manager
        EventManager.currentManager.Unsubscribe(EventType.OBJECTBOUGHTSCORES, OnObjectBoughtScores);
        EventManager.currentManager.Unsubscribe(EventType.OBJECTSOLDSCORES, OnObjectSoldScores);
        EventManager.currentManager.Unsubscribe(EventType.OBJECTBUYREQUEST, OnObjectBuyRequest);
        EventManager.currentManager.Unsubscribe(EventType.PLANTINVADED, OnPlantInvaded);
        EventManager.currentManager.Unsubscribe(EventType.PLANTGASSED, OnPlantGassed);
        EventManager.currentManager.Unsubscribe(EventType.PLANTINFECTED, OnPlantInfected);
        EventManager.currentManager.Unsubscribe(EventType.PLANTCURED, OnPlantCured);
        EventManager.currentManager.Unsubscribe(EventType.PLANTCUREGASREQUEST, OnCureGasRequest);
        EventManager.currentManager.Unsubscribe(EventType.MISSIONCOMPLETED, OnMissionComplete);
        EventManager.currentManager.Unsubscribe(EventType.MoneyEarned, OnMoneyEarned);
        EventManager.currentManager.Unsubscribe(EventType.REQUESTSCOREDATA, OnScoreRequest);
        EventManager.currentManager.Unsubscribe(EventType.WEEKLYSCOREINCREASE, OnWeekScoreIncrease);
    }

    private void Awake()
    {
        //Update the scores when the game starts
        UpdateTotalScores();
    }

    #region OnEvents
    private void OnObjectBoughtScores(EventData eventData)
    {
        if (eventData is ObjectBoughtScores)
        {
            //Cast the event so it can be used
            ObjectBoughtScores objectScores = (ObjectBoughtScores)eventData;

            //Handle the incoming data
            HandleIncomingBoughtScores(objectScores);
            CheckIfInsectReachedThresholds();

            CheckIfQuotaMet();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.OBJECTBOUGHTSCORES was received but is not of class ObjectBoughtScores.");
        }
    }
    private void OnObjectSoldScores(EventData eventData)
    {
        //Cast the event so it can be used
        if (eventData is ObjectSoldScores objectScores)
        {
            //Handle the incoming data
            HandleIncomingSoldScores(objectScores);
            CheckIfInsectLostThresholds();

            CheckIfQuotaMet();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.OBJECTSOLDSCORES was received but is not of class ObjectSoldScores.");
        }
    }
    private void OnObjectBuyRequest(EventData eventData)
    {
        if (eventData is ObjectBuyRequest)
        {
            //Cast the event so it can be used
            ObjectBuyRequest objectRequest = (ObjectBuyRequest)eventData;

            //Handle the incoming data
            HandleIncomingObjectBuyRequest(objectRequest);
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.OBJECTBUYREQUEST was received but is not of class ObjectBuyRequest.");
        }
    }
    private void OnPlantInvaded(EventData eventData)
    {
        if (eventData is PlantInvaded)
        {
            //Add to invasion count
            totalInvasiveness++;

            //Request a score update
            UpdateTotalScores();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.PLANTINVADED was received but is not of class PlantInvaded.");
        }
    }
    private void OnPlantGassed(EventData eventData)
    {
        if (eventData is PlantGassed)
        {
            //Add to invasion count
            totalInvasiveness--;

            //Request a score update
            UpdateTotalScores();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.PLANTGASSED was received but is not of class PlantGassed.");
        }
    }
    private void OnPlantInfected(EventData eventData)
    {
        if (eventData is PlantInfected)
        {
            //Add to invasion count
            totalInfections++;

            //Request a score update
            UpdateTotalScores();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.PLANTINFECTED was received but is not of class PlantInfected.");
        }
    }
    private void OnPlantCured(EventData eventData)
    {
        if (eventData is PlantCured)
        {
            //Add to invasion count
            totalInfections--;

            //Request a score update
            UpdateTotalScores();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.PLANTCURED was received but is not of class PlantCured.");
        }
    }
    private void OnCureGasRequest(EventData eventData)
    {
        if (eventData is PlantCureGasRequest)
        {
            //Cast the event so it can be used
            PlantCureGasRequest request = (PlantCureGasRequest)eventData;

            //Handle the incoming data
            HandleIncomingPlantCureGasRequest(request);
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.PLANTCUREGASREQUEST was received but is not of class PlantCureGasRequest.");
        }
    }
    private void OnMissionComplete(EventData eventData)
    {
        if (eventData is MissionCompleted)
        {
            //Cast the event so it can be used
            MissionCompleted missionReward = (MissionCompleted)eventData;

            //Add to user money
            AddMoney(missionReward.rewardAmount);

            //Request a score update
            UpdateTotalScores();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.MISSIONCOMPLETED was received but is not of class MissionCompleted.");
        }
    }
    private void OnMoneyEarned(EventData eventData)
    {
        if (eventData is MoneyEarned)
        {
            //Cast the event so it can be used
            MoneyEarned moneyData = (MoneyEarned)eventData;

            //Add to user money
            AddMoney(moneyData.moneyEarned);

            //Request a score update
            UpdateTotalScores();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.MoneyEarned was received but is not of class MoneyEarned.");
        }
    }
    private void OnScoreRequest(EventData eventData)
    {
        if (eventData is RequestScoreData)
        {
            //Fire off event with the total scores
            EventManager.currentManager.AddEvent(new TotalScoresUpdated(totalMoney, totalBiodiversity, totalCarbonIntake, totalAppeal, totalInvasiveness, totalInfections, totalScore));
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.REQUESTSCOREDATA was received but is not of class RequestScoreData.");
        }
    }
    private void OnWeekScoreIncrease(EventData eventData)
    {

        if (eventData is ScoreIncreasePerWeek)
        {
            CheckGameOver();
            AddMoney(weeklyMoneyGain);
            UpdateTotalScores();
            currentWeek++;
            //Cast the event so it can be used
            ScoreIncreasePerWeek scoreIncrease = (ScoreIncreasePerWeek)eventData;

            //increases score threshold
            float powerVal = 1 + 1 / (float)currentWeek*scoreIncrease.scalar;

            powerVal = Mathf.Pow(powerVal, currentWeek)-1;

            powerVal= (powerVal )* scoreIncrease.modifier;

            scoreThreshold = (int)powerVal;
            Debug.Log("Score threshold is now at: " + scoreThreshold);

            EventManager.currentManager.AddEvent(new QuotaUpdated(scoreThreshold));
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.MISSIONCOMPLETED was received but is not of class MissionCompleted.");
        }
    }
    #endregion

    #region Event Data Handlers
    //Handles the events related to a purchase being requested
    private void HandleIncomingBoughtScores(ObjectBoughtScores objectScores)
    {
        //Apply all recieved scores
        RemoveMoney(objectScores.cost);
        AddBiodiversity(objectScores.biodiversity, objectScores.insectType, objectScores.insectAttractiveness);
        AddCarbonIntake(objectScores.carbonIntake);
        AddAppeal(objectScores.attractiveScore);

        //Update final scores and fire event
        UpdateTotalScores();
    }

    //Handles the events related to scores being added
    private void HandleIncomingObjectBuyRequest(ObjectBuyRequest objectRequest)
    {
        //Check if purchase is possible (Is there enough money?)
        if (IsAffordable(objectRequest.requestedObject.GetCost()))
        {
            //Fire off confirming that it is possible
            EventManager.currentManager.AddEvent(new ObjectBuyRequestResult(objectRequest.requestedObject, true));
            DebugManager.DebugLog("Approved purchase for " + objectRequest.requestedObject.GetName() + "!");
        }
        else
        {
            //Fire off an event confirming that it is not possible
            EventManager.currentManager.AddEvent(new ObjectBuyRequestResult(null, false));
            DebugManager.DebugLog("Denied purchase for " + objectRequest.requestedObject.GetName() + "!");
        }
    }
    
    //Handles the events related to scores being removed
    private void HandleIncomingSoldScores(ObjectSoldScores objectScores)
    {
        //Apply all recieved scores
        AddMoney((int)(objectScores.refund*refundRation));
        RemoveBiodiversity(objectScores.biodiversity, objectScores.insectType, objectScores.insectAttractiveness);
        RemoveCarbonIntake(objectScores.carbonIntake);
        RemoveAppeal(objectScores.attractiveScore);

        //Update final scores and fire event
        UpdateTotalScores();
    }

    //Handles the events related to if the cure/gas request if possible
    private void HandleIncomingPlantCureGasRequest(PlantCureGasRequest request)
    {
        //Temporary variables
        bool cureApproved = false;
        bool gasApproved = false;

        //Check if request is possible (Is there enough money?)
        //If cure request and is sick
        if (request.isCureRequest && request.isSick)
        {
            if (IsAffordable(curingCost))
            {
                //Mark as approved
                cureApproved = true;

                //Subtract cost
                RemoveMoney(curingCost);
            }
        }

        //If gas request and is invaded
        if (request.isGasRequest && request.isInvaded)
        {
            if (IsAffordable(gassingCost))
            {
                //Mark as approved
                gasApproved = true;

                //Subtract cost
                RemoveMoney(gassingCost);
            }
        }

        //Fire off event with what is approved or dissaproved
        EventManager.currentManager.AddEvent(new PlantCureGasRequestResult(cureApproved, gasApproved));

        //Update the scores
        UpdateTotalScores();
    }
    #endregion

    private void UpdateTotalScores()
    {
        //Update the total score
        totalScore = totalBiodiversity + totalCarbonIntake - (totalInfections + totalInvasiveness);

        //Fire off event with information
        EventManager.currentManager.AddEvent(new TotalScoresUpdated(totalMoney, totalBiodiversity, totalCarbonIntake, totalAppeal, totalInvasiveness, totalInfections, totalScore));
    }
    private bool IsAffordable(int givenCost)
    {
        //Check that money would not go negative
        if (totalMoney - givenCost < 0)
        {
            //Purchase not possible
            return false;
        }
        //Else the purchase is possible
        return true;
    }

    private void CheckIfInsectReachedThresholds()
    {
        //The bee score is enough to spawn bees
        if (beeThreshold <= beeScore && !reachedBeeThreshold)
        {
            reachedBeeThreshold = true;
            //Send out event that threshold was reach
            EventManager.currentManager.AddEvent(new BeeThresholdReached());
        }
        //The beetle score is enough to spawn beetles
        if (beetleThreshold <= beetleScore && !reachedBeetleThreshold)
        {
            reachedBeetleThreshold = true;
            //Send out event that threshold was reach
            EventManager.currentManager.AddEvent(new BeetleThresholdReached());
        }
        //The butterly score is enough to spawn butterflies
        if (butterflyThreshold <= butterflyScore && !reachedButterflyThreshold)
        {
            reachedButterflyThreshold = true;
            //Send out event that threshold was reach
            EventManager.currentManager.AddEvent(new ButterflyThresholdReached());
        }
    }
    private void CheckIfInsectLostThresholds()
    {
        //The bee score is enough to spawn bees
        if (beeThreshold > beeScore && reachedBeeThreshold)
        {
            reachedBeeThreshold = false;
            //Send out event that threshold was reach
            EventManager.currentManager.AddEvent(new BeeThresholdLost());
        }
        //The beetle score is enough to spawn beetles
        if (beetleThreshold > beetleScore && reachedBeetleThreshold)
        {
            reachedBeetleThreshold = false;
            //Send out event that threshold was reach
            EventManager.currentManager.AddEvent(new BeetleThresholdLost());
        }
        //The butterly score is enough to spawn butterflies
        if (butterflyThreshold > butterflyScore && reachedButterflyThreshold)
        {
            reachedButterflyThreshold = false;
            //Send out event that threshold was reach
            EventManager.currentManager.AddEvent(new ButterflyThresholdLost());
        }
    }
    private void CheckIfQuotaMet()
    {
        //biodiversity
        if (totalBiodiversity < scoreThreshold)
        {
            EventManager.currentManager.AddEvent(new BiodiversityQuota(true));
        }
        else
        {
            EventManager.currentManager.AddEvent(new BiodiversityQuota(false));
        }
        //carbon
        if (totalCarbonIntake < scoreThreshold)
        {
            EventManager.currentManager.AddEvent(new CarbonIntakeQuota(true));
        }
        else
        {
            EventManager.currentManager.AddEvent(new CarbonIntakeQuota(false));
        }
        //appeal
        if (totalAppeal < scoreThreshold)
        {
            EventManager.currentManager.AddEvent(new AppealQuota(true));
        }
        else
        {
            EventManager.currentManager.AddEvent(new AppealQuota(false));
        }
    }

    private void CheckGameOver()
    {
        //If total scores are below threshold, they lose
        if (totalBiodiversity < scoreThreshold||totalCarbonIntake<scoreThreshold||totalAppeal<scoreThreshold)
        {
            EventManager.currentManager.AddEvent(new GameOver());
            Debug.Log("Game Over");
        }
    }

    #region Score Changing Related Methods
    //Adds money to the total money
    private void AddMoney(int money)
    {
        totalMoney += money;
    }

    //Removes money to the total money
    private void RemoveMoney(int money)
    {
        totalMoney -= money;
    }

    //Adds money to the total money
    private void AddCarbonIntake(int carbon)
    {
        totalCarbonIntake += carbon;
    }

    //Removes money to the total money
    private void RemoveCarbonIntake(int carbon)
    {
        totalCarbonIntake -= carbon;
    }

    //Adds attractivenesss to the total attractiveness
    private void AddAppeal(int appeal)
    {
        totalAppeal += appeal;
    }

    //Removes attractivenesss to the total attractiveness
    private void RemoveAppeal(int appeal)
    {
        totalAppeal -= appeal;
    }

    //Adds to the different biodiversity scores and updates the total
    private void AddBiodiversity(int givenObjectBiodiversity, InsectType givenInsect, int givenInsectScore)
    {
        //Add to the object biodiversity
        objectBiodiversity += givenObjectBiodiversity;

        //Add to the insect attractiveness scores
        switch (givenInsect)
        {
            case InsectType.Bee:
                //Add to the bee scores
                beeScore += givenInsectScore;
                break;

            case InsectType.Beetle:
                //Add to the beetle scores
                beetleScore += givenInsectScore;
                break;

            case InsectType.Butterfly:
                //Add to the butterfly scores
                butterflyScore += givenInsectScore;
                break;

            default:
                //Do nothing as no insect score is intended
                break;
        }

        //Recalculate total biodiversity score
        totalBiodiversity = objectBiodiversity + beeScore + beetleScore + butterflyScore;
    }

    //Remove from the different biodiversity scores and updates the total
    private void RemoveBiodiversity(int givenObjectBiodiversity, InsectType givenInsect, int givenInsectScore)
    {
        //Call AddBiodiversity but with negative values to subtract
        AddBiodiversity(-givenObjectBiodiversity, givenInsect, -givenInsectScore);
    }
    #endregion

    #region GameProgression

    #endregion
}
