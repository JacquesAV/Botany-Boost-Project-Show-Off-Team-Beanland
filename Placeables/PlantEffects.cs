using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEffects : MonoBehaviour
{
    private PlaceableData placeableData;
    private readonly int radius=2;
    private float baseDiseaseChance = 0.25f;
    private float diseaseSpreadModifier=0.1f;
    private bool isSick=false;

    private float baseInvaderSpawnChance = 0.25f;
    private bool hasInvaders = false;

    private void Start()
    {
        placeableData=gameObject.GetComponent<PlaceableData>();
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
            //checks if the plant got infected
            float diseaseChance = baseDiseaseChance + (samePlantCount * diseaseSpreadModifier) * 100;
            if (Random.Range(1, 100) <= diseaseChance)
            {
                DebugManager.DebugLog("You just got diseased");
                isSick = true;
                EventManager.currentManager.AddEvent(new PlantInfected());
            }
            else
            {
                DebugManager.DebugLog("You are helfy");
            }
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
            Debug.Log(combinedPlantSpawnChances);
            //checks if the plant got an invasive species
            float invadorChance = baseInvaderSpawnChance + (combinedPlantSpawnChances * baseInvaderSpawnChance) * 100;
            if (Random.Range(1, 100) <= invadorChance)
            {
                DebugManager.DebugLog("There is an imposter among us");
                hasInvaders = true;
                EventManager.currentManager.AddEvent(new PlantInvaded());
            }
            else
            {
                DebugManager.DebugLog("I am ino");
            }
        }
    }
    public void SetPlaceableData(PlaceableData data)
    {
        placeableData=data;
    }

    public void OnDayPassed(EventData eventData)
    {
        if (eventData is DayHasPassed)
        {
            CalculateDiseaseChance();
            CalculateInvasiveSpeciesSpawnChance();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.PURCHASEBEGIN was received but is not of class DayHasPassed.");
        } 
    }

}
