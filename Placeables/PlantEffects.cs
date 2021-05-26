using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEffects : MonoBehaviour
{
    private PlaceableData placeableData;//the data of the object that it is attached to
    private readonly int radius=2;//the radius of the spread
    private float baseDiseaseChance = 0.25f;//the base chance of getting disease
    private float diseaseSpreadModifier=0.1f;//the modifier to obtaining a disease
    private bool isSick=false;//if the plant got a disease

    private float baseInvaderSpawnChance = 0.25f;//base chance of an invasive species spawning on the plant
    private bool hasInvaders = false;//if the plant has an invaded

    private int lifespanInDays;//the time that the plant lives

    private void Start()
    {
        //set variables to the ones of the prefab
        placeableData=gameObject.GetComponent<PlaceableData>();
        lifespanInDays = placeableData.GetLifespan();
        baseDiseaseChance = placeableData.GetBaseDiseaseChance();
        diseaseSpreadModifier = placeableData.GetDiseaseSpreadModifier();
    }

    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.DAYPASSED, OnDayPassed);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.DAYPASSED, OnDayPassed);
    }

    public void OnDayPassed(EventData eventData)
    {
        //when day passes
        if (eventData is DayHasPassed)
        {
            CalculateDiseaseChance();
            CalculateInvasiveSpeciesSpawnChance();
            CheckIfPlantDied();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.PURCHASEBEGIN was received but is not of class DayHasPassed.");
        } 
    }

    private void CheckIfPlantDied()
    {
        //If the lifespan is considered dead
        if (lifespanInDays < 1)
        {
            //If infected or invaded, reduce the counter amount
            if(hasInvaders)
            {
                EventManager.currentManager.AddEvent(new PlantGassed());
            }
            if (isSick)
            {
                EventManager.currentManager.AddEvent(new PlantCured());
            }

            //Debug
            //DebugManager.DebugLog("plant has dieded");

            //Remove the plant benefits from the score manager (with 0 refunded money)
            EventManager.currentManager.AddEvent(new ObjectSoldScores(0, placeableData.GetBiodiversity(), placeableData.GetCarbonIntake(), placeableData.GetAppeal(), placeableData.GetInsectType(), placeableData.GetInsectAttractiveness()));
            
            //Unlink the grid from the plant to free it up for other objects
            gameObject.GetComponentInParent<TileBuildingModel>().UnlinkNeighbours();
        }
        //If completely healthy, reset its lifespan count
        if (!isSick && !hasInvaders)
        {
            lifespanInDays = placeableData.GetLifespan();
        }
    }
    //Check if a plant will get sick
    private void CalculateDiseaseChance()
    {
        if (!isSick)
        {
            float samePlantCount = 0;
            //checks if the plant
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius);
            foreach (Collider hitCollider in hitColliders)
            {
                //check if its not the same object
                if (hitCollider.transform != transform)
                {
                    //makes sure its a plant
                    if (hitCollider.CompareTag("Plant"))
                    {
                        //make sure they have the same name
                        if (hitCollider.gameObject.GetComponent<PlaceableData>().GetName() == placeableData.GetName())
                        {
                            samePlantCount++;
                        }
                    }
                }
            }
            //checks if the plant got infected with percentile chance
            float diseaseChance = baseDiseaseChance + (samePlantCount * diseaseSpreadModifier) * 100;
            if (Random.Range(1, 100) <= diseaseChance)
            {
                //DebugManager.DebugLog("You just got diseased");
                isSick = true;
                EventManager.currentManager.AddEvent(new PlantInfected());
            }
            else
            {
                //DebugManager.DebugLog("You are helfy");
            }
        }
        else
        {
            //if it is sick then the plant will have 1 less day to live
            lifespanInDays--;
        }
    }
    //Check if a plant will get invasive species
    private void CalculateInvasiveSpeciesSpawnChance()
    {
        if (!hasInvaders)
        {
            float combinedPlantSpawnChances = 0;
            //checks if the plant
            GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
            foreach (GameObject plant in plants)
            {
                //check if its not the same object
                if (plant.transform != transform)
                {
                    //makes sure its a plant
                    if (plant.CompareTag("Plant"))
                    {
                        combinedPlantSpawnChances += plant.GetComponent<PlaceableData>().GetInvaderSpreadChance();
                    }
                }
            }
            //checks if the plant got an invasive species
            float invadorChance = baseInvaderSpawnChance + (combinedPlantSpawnChances * baseInvaderSpawnChance) * 100;
            if (Random.Range(1, 100) <= invadorChance)
            {
                //DebugManager.DebugLog("There is an imposter among us");
                hasInvaders = true;
                EventManager.currentManager.AddEvent(new PlantInvaded());
            }
            else
            {
                //DebugManager.DebugLog("I am ino");
            }
        }
        else
        {
            //if it is sick then the plant will have 1 less day to live
            lifespanInDays--;
        }
    }
    public void SetPlaceableData(PlaceableData data)
    {
        placeableData = data;
    }

    public void PlantCured()
    {
        //set plant to not being sick and send out event that it was cured
        isSick = false;
        EventManager.currentManager.AddEvent(new PlantCured());
    }
    public void PlantGassed()
    {
        //set plant to not being invaded and send out event that it was gasses
        hasInvaders = false;
        EventManager.currentManager.AddEvent(new PlantGassed());
    }
}
