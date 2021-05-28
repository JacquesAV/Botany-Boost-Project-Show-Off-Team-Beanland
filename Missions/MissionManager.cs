using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionManager : MonoBehaviour
{
    [SerializeField]
    private int numberOfActiveMissions = 1; //Number of active missions at one time
    private List<GameObject> missionObjects = new List<GameObject>(); //List of mission objects
    private List<GameObject> availableMissionsPool = new List<GameObject>(); //List of all potential/incomplete missions
    private List<GameObject> activeMissions = new List<GameObject>(); //List of the currently active missions
    private List<GameObject> completedMissions = new List<GameObject>(); //List of all completed missions

    private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.WEEKPASSED, OnWeekPassed);
    }
    private void OnDisable()
    {
        //Unsubscribes the method and event type to the current manager
        EventManager.currentManager.Unsubscribe(EventType.WEEKPASSED, OnWeekPassed);
    }

    //Gets called when a WeekHasPassed event has been called
    private void OnWeekPassed(EventData eventData)
    {
        if (eventData is WeekHasPassed)
        {
            //Populate a list of new missions and clear the previous one
            UpdateMissions();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.WEEKPASSED was received but is not of class WeekHasPassed.");
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //Load in the missions
        LoadResourceMissions();

        //Reset the mission pools in case of junk/old data
        ResetMissionPools();
    }

    //First time the missions should be initialized
    private void LoadResourceMissions()
    {
        //Clear missions object list
        missionObjects.Clear();

        //Retrieve missions from resource folder
        List<GameObject> loadedObjects = Resources.LoadAll("Missions", typeof(GameObject)).Cast<GameObject>().ToList();

        //Check through mission objects to ensure that there is a mission class, if not throw an error
        foreach (GameObject missionObject in loadedObjects)
        {
            if (!missionObject.TryGetComponent(out Mission temp))
            {
                throw new System.Exception("GameObject: " + missionObject.name + " does not have mission data");
            }
        }

        //Instantiate mission objects
        foreach (GameObject missionObject in loadedObjects)
        {
            //Create temporary object
            GameObject temporaryMissionObject = Instantiate(missionObject, transform);
            temporaryMissionObject.name = temporaryMissionObject.GetComponent<Mission>().GetType().ToString();

            //Set object to disabled
            temporaryMissionObject.SetActive(false);

            //Add to the mission objects so that an active reference exists
            missionObjects.Add(temporaryMissionObject);
        }
    }

    //Updates the list of active missions, adding failed/completed ones to the completed missions and moving onto new ones
    private void UpdateMissions()
    {
        //If there are no available missions or not enough available missions, reset the list available list
        if (availableMissionsPool.Count < numberOfActiveMissions)
        {
            Debug.Log("Resseting pool");
            //Initialize fresh missions
            ResetMissionPools();
            PoolNewActiveMissions();
        }
        else
        {
            Debug.Log("Using standard pool");
            //Clear active missions and then add new missions
            LeakActiveMissions();
            PoolNewActiveMissions();
        }
        Debug.Log("Number of missions remaining in the pool: " + availableMissionsPool.Count);
        //Fire off event with the new active missions
        EventManager.currentManager.AddEvent(new CurrentActiveMissions(activeMissions));
    }

    //For when the missions should be reset
    private void ResetMissionPools()
    {
        //Clear active missions pool
        activeMissions.Clear();

        //Clear completed missions pool
        completedMissions.Clear();

        //Clear available missions pool
        availableMissionsPool.Clear();

        //Save all available missions to the mission pool and reset them
        foreach (GameObject missionObject in missionObjects)
        {
            //Reset the mission data
            missionObject.GetComponent<Mission>().ResetMission();

            //Disable the object
            missionObject.SetActive(false);

            //Add to missions pool 
            availableMissionsPool.Add(missionObject);
        }
    }

    //Get a set number of active missions
    private void PoolNewActiveMissions()
    {
        //Get a set number of active missions
        while (activeMissions.Count < numberOfActiveMissions && availableMissionsPool.Count > 0)
        {
            //Get random index
            int index = Random.Range(0, availableMissionsPool.Count);

            //Set to active
            availableMissionsPool[index].SetActive(true);

            //Add that mission to the active missions list
            activeMissions.Add(availableMissionsPool[index]);

            //Remove the mission from the pool of available missions
            availableMissionsPool.RemoveAt(index);
        }
    }

    private void LeakActiveMissions()
    {
        //Move the active missions to the completed missions list and clear the list
        foreach(GameObject missionObject in activeMissions)
        {
            //Reset the object
            missionObject.GetComponent<Mission>().ResetMission();

            //Disable the object
            missionObject.SetActive(false);

            //Add to the completed missions list
            completedMissions.Add(missionObject);
        }

        //Clear the list of active missions
        activeMissions.Clear();
    }
}
