using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    //Variables that are tracked in order to calculate score
    [SerializeField]
    private int totalMoney = 50; //Total money the user has to spend
    [SerializeField]
    private int beeThreshold=50, butterflyThreshold = 50, beetleThreshold = 50; //Thresholds for bees/butterflies/beetles to appear at
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
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.OBJECTBOUGHTSCORES was received but is not of class ObjectBoughtScores.");
        }
    }
    private void OnObjectSoldScores(EventData eventData)
    {
        if (eventData is ObjectSoldScores)
        {
            //Cast the event so it can be used
            ObjectSoldScores objectScores = (ObjectSoldScores)eventData;

            //Handle the incoming data
            HandleIncomingSoldScores(objectScores);
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

    //Handles the events related to scores being added or removed
    private void HandleIncomingObjectBuyRequest(ObjectBuyRequest objectRequest)
    {
        //Check if purchase is possible (Is there enough money?)
        if(IsAffordable(objectRequest.requestedObject.GetCost()))
        {
            //Fire off confirming that it is possible
            EventManager.currentManager.AddEvent(new ObjectBuyRequestResult(objectRequest.requestedObject, true));
            DebugManager.DebugLog("Approved purchase for "+ objectRequest.requestedObject.GetName() +"!");
        }
        else
        {
            //Fire off an event confirming that it is not possible
            EventManager.currentManager.AddEvent(new ObjectBuyRequestResult(null, false));
            DebugManager.DebugLog("Denied purchase for " + objectRequest.requestedObject.GetName() + "!");
        }
    }

    private void HandleIncomingSoldScores(ObjectSoldScores objectScores)
    {
        //Apply all recieved scores
        AddMoney(objectScores.refund);
        RemoveBiodiversity(objectScores.biodiversity, objectScores.insectType, objectScores.insectAttractiveness);
        RemoveCarbonIntake(objectScores.carbonIntake);
        RemoveAppeal(objectScores.attractiveScore);

        //Update final scores and fire event
        UpdateTotalScores();
    }
    #endregion

    private void UpdateTotalScores()
    {
        //Update the total score
        totalScore = totalBiodiversity + totalCarbonIntake - (totalInfections + totalInvasiveness);

        //Fire off event with information
        EventManager.currentManager.AddEvent(new TotalScoresUpdated(totalMoney, totalBiodiversity, totalCarbonIntake, totalAppeal, totalInvasiveness, totalInfections));
    }
    private bool IsAffordable(int givenCost)
    {
        //Check that money would not go negative
        if(totalMoney - givenCost < 0)
        {
            //Purchase not possible
            return false;
        }
        //Else the purchase is possible
        return true;
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
}
