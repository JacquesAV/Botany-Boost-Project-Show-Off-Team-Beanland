using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    //Variables that are tracked in order to calculate score
    private int totalMoney = 0; //Total money the user has to spend
    private int totalBiodiversity = 0; //Total biodiversity, based on plants and insect attractiveness
    private int objectBiodiversity = 0; //Biodiversity, based on objects
    private int beeScore = 0; //Insect attractiveness (insect biodiversity)
    private int butterflyScore = 0; //Insect attractiveness (insect biodiversity)
    private int beetleScore = 0; //Insect attractiveness (insect biodiversity)
    private int totalCarbonIntake = 0; //Total carbon intake score, is not influenced by others
    private int totalAttractiveness = 0; //Total attractiveness of the objects in general, more of a "human" likeability score
    private int totalInvasiveness = 0; //Level of invasiveness, calculated from the number of negatively affected plants
    private int totalDisease = 0; //Level of disease, calculated from the number of negatively affected plants
    private int totalScore = 0; //Total combination of all scores

    private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.OBJECTPLACEDSCORES, OnObjectPlacedScores);
        EventManager.currentManager.Subscribe(EventType.OBJECTREMOVEDSCORES, OnObjectRemovedScores);
    }
    private void OnDisable()
    {
        //Unsubscribes the method and event type from the current manager
        EventManager.currentManager.Unsubscribe(EventType.OBJECTPLACEDSCORES, OnObjectPlacedScores);
        EventManager.currentManager.Unsubscribe(EventType.OBJECTREMOVEDSCORES, OnObjectRemovedScores);
    }

    private void OnObjectPlacedScores(EventData eventData)
    {
        if (eventData is ObjectPlacedScores)
        {
            //Cast the event so it can be used
            ObjectPlacedScores objectScores = (ObjectPlacedScores)eventData;

            //Handle the incoming data
            HandleIncomingPlacedScores(objectScores);
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.OBJECTPLACEDSCORES was received but is not of class ObjectPlacedScores.");
        }
    }
    private void OnObjectRemovedScores(EventData eventData)
    {
        if (eventData is ObjectRemovedScores)
        {
            //Cast the event so it can be used
            ObjectRemovedScores objectScores = (ObjectRemovedScores)eventData;

            //Handle the incoming data
            HandleIncomingRemovedScores(objectScores);
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.OBJECTREMOVEDSCORES was received but is not of class ObjectRemovedScores.");
        }
    }
    //Handles the events related to scores being added or removed
    private void HandleIncomingPlacedScores(ObjectPlacedScores objectScores)
    {
        //Apply all recieved scores
        RemoveMoney(objectScores.cost);
        AddBiodiversity(objectScores.biodiversity, objectScores.insectType, objectScores.insectAttractiveness);
        AddCarbonIntake(objectScores.carbonIntake);
        AddAttractiveness(objectScores.attractiveScore);

        //Update final scores and fire event
        UpdateTotalScores();
    }
    private void HandleIncomingRemovedScores(ObjectRemovedScores objectScores)
    {
        //Apply all recieved scores
        AddMoney(objectScores.refund);
        RemoveBiodiversity(objectScores.biodiversity, objectScores.insectType, objectScores.insectAttractiveness);
        RemoveCarbonIntake(objectScores.carbonIntake);
        RemoveAttractiveness(objectScores.attractiveScore);

        //Update final scores and fire event
        UpdateTotalScores();
    }
    private void UpdateTotalScores()
    {
        //Update the total score
        totalScore = totalBiodiversity + totalCarbonIntake - (totalDisease + totalInvasiveness);

        //Fire off event with information
        EventManager.currentManager.AddEvent(new TotalScoresUpdated(totalMoney, totalBiodiversity, totalCarbonIntake, totalAttractiveness, totalInvasiveness, totalDisease));
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
    private void AddAttractiveness(int attractiveness)
    {
        totalAttractiveness += attractiveness;
    }

    //Removes attractivenesss to the total attractiveness
    private void RemoveAttractiveness(int attractiveness)
    {
        totalAttractiveness -= attractiveness;
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
