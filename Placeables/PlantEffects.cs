using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantEffects : MonoBehaviour
{
    private PlaceableData placeableData;//the data of the object that it is attached to
    private readonly int radius = 2;//the radius of the spread
    private float baseDiseaseChance = 0.25f;//the base chance of getting disease
    private float diseaseSoftCap = 0.1f;//the soft cap of a disease
    private bool isSick = false;//if the plant got a disease

    private float baseInvaderSpawnChance = 0.25f;//base chance of an invasive species spawning on the plant
    private bool hasInvaders = false;//if the plant has an invaded

    private int lifespanInDays;//the time that the plant lives

    public TileBuildingModel tileModel = null;

    private GameObject diseaseFX;
    private GameObject invaderFX;
    private GameObject bugFX;

    private void Start()
    {
        //set variables to the ones of the prefab
        placeableData = gameObject.GetComponent<PlaceableData>();
        lifespanInDays = placeableData.GetLifespan();
        //flips the value for calculation
        baseDiseaseChance = 1-placeableData.GetBaseDiseaseChance();
        diseaseSoftCap = placeableData.GetDiseaseSoftCap();
        //Setup fx
        SetupHealthFX();
        SetupBugFX();
    }

    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.DAYPASSED, OnDayPassed);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.DAYPASSED, OnDayPassed);
    }
    #region OnEvents
    private void OnDayPassed(EventData eventData)
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
            throw new System.Exception("Error: EventData class with EventType.DAYPASSED was received but is not of class DayHasPassed.");
        }
    }
    private void OnThresholdReached(EventData eventData)
    {
        if (this != null&&gameObject!=null)
        {
            bugFX.SetActive(true);
        }
    }
    private void OnThresholdLost(EventData eventData)
    {
        if (this != null && gameObject != null)
        {
            bugFX.SetActive(false);
        }
    }
    #endregion
    #region PlantHealth
    private void CheckIfPlantDied()
    {
        //If the lifespan is considered dead
        if (lifespanInDays < 1)
        {
            //If infected or invaded, reduce the counter amount
            if (hasInvaders)
            {
                EventManager.currentManager.AddEvent(new PlantGassed());
            }
            if (isSick)
            {
                EventManager.currentManager.AddEvent(new PlantCured());
            }

            //Debug
            DebugManager.DebugLog("Plant has died - WasSick: " + isSick + " | WasInvaded: " + hasInvaders);

            //Remove the plant benefits from the score manager (with 0 refunded money)
            EventManager.currentManager.AddEvent(new ObjectSoldScores(0, placeableData.GetBiodiversity(), placeableData.GetCarbonIntake(), placeableData.GetAppeal(), placeableData.GetInsectType(), placeableData.GetInsectAttractiveness()));

            UnsubscribeFromThresholds();
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
            float samePlantDiseaseCombined = 1;
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
                            samePlantDiseaseCombined=samePlantDiseaseCombined*baseDiseaseChance;
                        }
                    }
                }
            }
            //checks if the plant got infected with percentile chance
            float diseaseChance = (1-samePlantDiseaseCombined) * diseaseSoftCap * 100;
            if (Random.Range(1, 100) <= diseaseChance)
            {
                //DebugManager.DebugLog("Plant got sick!");
                tileModel.ChangeMaterialColor(Color.cyan);

                //Set to sick and fire off event that it was infected
                isSick = true;
                EventManager.currentManager.AddEvent(new PlantInfected());
                //Enable disease particle
                if (diseaseFX != null)
                {
                    diseaseFX.SetActive(true);
                }
            }
            else
            {
                //DebugManager.DebugLog("Plant was found to be without sickness!");
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
            float invadorChance = baseInvaderSpawnChance + (combinedPlantSpawnChances * baseInvaderSpawnChance)*diseaseSoftCap * 100;
            Debug.Log(invadorChance);
            if (Random.Range(1, 100) <= invadorChance)
            {
                //DebugManager.DebugLog("Plant got invaded!");
                tileModel.ChangeMaterialColor(Color.magenta);

                //Set to invaded and fire off event that it was invaded
                hasInvaders = true;
                EventManager.currentManager.AddEvent(new PlantInvaded());
                //Enable invader particle
                if (invaderFX != null)
                {
                    invaderFX.SetActive(true);
                }
            }
            else
            {
                //DebugManager.DebugLog("Plant was found to be without invaders!");
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

    public void CurePlant()
    {
        //First check if the plant is sick
        if (!isSick)
        {
            DebugManager.DebugLog("Nothing to cure!");
            return;
        }

        //set plant to not being sick and send out event that it was cured
        isSick = false;
        EventManager.currentManager.AddEvent(new PlantCured());
        DebugManager.DebugLog("Plant was cured!");

        //disable invader effect
        if (invaderFX != null)
        {
            diseaseFX.SetActive(false);
        }
    }

    public void GasPlant()
    {
        //First check if the plant is invaded
        if (!hasInvaders)
        {
            DebugManager.DebugLog("Nothing to gas!");
            return;
        }

        //set plant to not being invaded and send out event that it was gasses
        hasInvaders = false;
        EventManager.currentManager.AddEvent(new PlantGassed());
        DebugManager.DebugLog("Plant was gassed!");

        //disable disease effect
        if (diseaseFX != null)
        {
            invaderFX.SetActive(false);
        }
    }
    #endregion
    #region PlantSetup
    private void SetupHealthFX()
    {
        //Create a variable for assignment
        GameObject p = null;
        //If plant holder contains a diseaseFX
        if ((p = PlantHolder.GetDiseaseFX()) != null)
        {
            //Create diseaseFX and disable it
            diseaseFX = Instantiate(p, transform, false);
            diseaseFX.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Warning: Could not find a DiseaseFX from PlantHolder, make sure you have script attached to an object and that there is a file in the resource folder");
        }
        //Reset p to null
        p = null;
        //If plant holder contains a invaderFX
        if ((p = PlantHolder.GetInvaderFX()) != null)
        {
            //Create invaderFX and disable it
            invaderFX = Instantiate(p, transform, false);
            invaderFX.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Warning: Could not find a InvaderFX from PlantHolder, make sure you have script attached to an object and that there is a file in the resource folder");
        }
    }
    private void SetupBugFX()
    {
        //Create a variable for assignment
        GameObject p = null;
        switch (placeableData.GetInsectType())
        {
            case InsectType.Bee:
                EventManager.currentManager.Subscribe(EventType.BEETHRESHOLDREACHED, OnThresholdReached);
                EventManager.currentManager.Subscribe(EventType.BEETHRESHOLDLOST, OnThresholdLost);
                //Create a variable for assignment
                p = null;
                //If plant holder contains a BugFX
                if ((p = PlantHolder.GetBeeFX()) != null)
                {
                    //Create BugFX and disable it if it has not reached the threshold
                    bugFX = Instantiate(p, transform, false);
                    if (!PlantHolder.GetShowBees())
                        bugFX.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Warning: Could not find a BugFX, bee, from PlantHolder, make sure you have script attached to an object and that there is a file in the resource folder");
                }
                break;
            case InsectType.Beetle:
                EventManager.currentManager.Subscribe(EventType.BEETLETHRESHOLDREACHED, OnThresholdReached);
                EventManager.currentManager.Subscribe(EventType.BEETLETHRESHOLDLOST, OnThresholdLost);
                //Create a variable for assignment
                p = null;
                //If plant holder contains a BugFX
                if ((p = PlantHolder.GetBeetleFX()) != null)
                {
                    //Create BugFX and disable it if it has not reached the threshold
                    bugFX = Instantiate(p, transform, false);
                    if (!PlantHolder.GetShowBeetles())
                        bugFX.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Warning: Could not find a BugFX, beetle, from PlantHolder, make sure you have script attached to an object and that there is a file in the resource folder");
                }
                break;
            case InsectType.Butterfly:
                EventManager.currentManager.Subscribe(EventType.BUTTERFLYTHRESHOLDREACHED, OnThresholdReached);
                EventManager.currentManager.Subscribe(EventType.BUTTERFLYTHRESHOLDLOST, OnThresholdLost);
                //Create a variable for assignment
                p = null;
                //If plant holder contains a BugFX
                if ((p = PlantHolder.GetButterflyFX()) != null)
                {
                    //Create BugFX and disable it if it has not reached the threshold
                    bugFX = Instantiate(p, transform, false);
                    if (!PlantHolder.GetShowButterflies())
                        bugFX.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Warning: Could not find a BugFX, butterfly, from PlantHolder, make sure you have script attached to an object and that there is a file in the resource folder");
                }
                break;
        }
    }
    #endregion
    private void UnsubscribeFromThresholds()
    {
        //Unsubscribes from respective thresholds
        switch (placeableData.GetInsectType())
        {
            case InsectType.Bee:
                EventManager.currentManager.Unsubscribe(EventType.BEETHRESHOLDREACHED, OnThresholdReached);
                EventManager.currentManager.Unsubscribe(EventType.BEETHRESHOLDLOST, OnThresholdLost);
                break;
            case InsectType.Beetle:
                EventManager.currentManager.Unsubscribe(EventType.BEETLETHRESHOLDREACHED, OnThresholdReached);
                EventManager.currentManager.Unsubscribe(EventType.BEETLETHRESHOLDLOST, OnThresholdLost);
                break;
            case InsectType.Butterfly:
                EventManager.currentManager.Unsubscribe(EventType.BUTTERFLYTHRESHOLDREACHED, OnThresholdReached);
                EventManager.currentManager.Unsubscribe(EventType.BUTTERFLYTHRESHOLDLOST, OnThresholdLost);
                break;
        }
    }

    //Gets if the plant is sick
    public bool GetIsSick()
    {
        return isSick;
    }

    //Gets if the plant is invaded
    public bool GetHasInvaders()
    {
        return hasInvaders;
    }
}
