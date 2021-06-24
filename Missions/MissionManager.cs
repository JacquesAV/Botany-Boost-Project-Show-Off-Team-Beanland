using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionManager : MonoBehaviour
{
    [SerializeField]
    private int numberOfActiveMissions = 1; //Number of active missions at one time
    private List<List<GameObject>> missionObjects = new List<List<GameObject>>(); //List of mission objects
    private List<GameObject> availableMissionsPool = new List<GameObject>(); //List of all potential/incomplete missions
    private List<GameObject> activeMissions = new List<GameObject>(); //List of the currently active missions
    private List<GameObject> completedMissions = new List<GameObject>(); //List of all completed missions
    private int currentWeekIndex = 0; //The current week, important in separating missions by week cycle (starting at 0 for indexing purposes!)

    private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.WEEKPASSED, OnWeekPassed); //Process when a week has passed
        EventManager.currentManager.Subscribe(EventType.MISSIONUPDATED, OnMissionUpdated); //Process when a mission has been updated
    }
    private void OnDisable()
    {
        //Unsubscribes the method and event type to the current manager
        EventManager.currentManager.Unsubscribe(EventType.WEEKPASSED, OnWeekPassed);
        EventManager.currentManager.Unsubscribe(EventType.MISSIONUPDATED, OnMissionUpdated);
    }

    //Gets called when a WeekHasPassed event has been called
    private void OnWeekPassed(EventData eventData)
    {
        if (eventData is WeekHasPassed)
        {
            //Increase the week counter
            currentWeekIndex++;

            //Fire off rewards for missions that reward you at the end of the week (if they're completed!)
            ProcessEndOfWeekMissions();

            //Add new weekly missions to the active pool
            PoolWeekSpecificMissions(currentWeekIndex);

            //Populate a list of new missions and clear the previous one
            UpdateNewMissions();
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.WEEKPASSED was received but is not of class WeekHasPassed.");
        }
    }

    private void OnMissionUpdated(EventData eventData)
    {
        if (eventData is MissionUpdated)
        {
            //Fire off event of current active missions with updated information
            EventManager.currentManager.AddEvent(new CurrentActiveMissions(activeMissions,true));
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.MISSIONUPDATED was received but is not of class MissionUpdated.");
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        //Load in the missions
        LoadResourceMissions();

        //Reset the mission pools in case of junk/old data
        ResetMissionPools();
    }

    private void Start()
    {
        //Initialize with the intended first set of missions
        UpdateNewMissions();
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

        //Temporary int to track number of missions
        int numberOfMissions = 0;

        //Instantiate mission objects
        foreach (GameObject missionObject in loadedObjects)
        {
            //Create temporary objects
            GameObject temporaryMissionGameObject = Instantiate(missionObject, transform);
            Mission temporaryMissionObject = temporaryMissionGameObject.GetComponent<Mission>();
            temporaryMissionGameObject.name = temporaryMissionObject.GetType().ToString();

            //Set object to disabled
            temporaryMissionGameObject.SetActive(false);

            //Add to the mission objects based on its week counter so that an active reference exists
            int weekOfAppearance = temporaryMissionObject.GetAppearsFromWeek();
            UpdateMissionObjectMatrixIndices(weekOfAppearance);
            missionObjects[weekOfAppearance-1].Add(temporaryMissionGameObject);

            //Add to the temporary tracking variable
            numberOfMissions++;
        }

        DebugManager.DebugLog("Succesfully loaded in " + numberOfMissions + " missions!");
    }

    //Processes end of week missions so that rewards are fired off (if completed)
    private void ProcessEndOfWeekMissions()
    {
        //Iterate over each and call the end of week method (mission class handles if this is valid)
        foreach (GameObject missionObject in activeMissions)
        {
            //Reset the mission data
            missionObject.GetComponent<Mission>().OnWeekPassedReward();
        }
    }

    //Updates the list of active missions, adding failed/completed ones to the completed missions and moving onto new ones
    private void UpdateNewMissions()
    {
        //If there are no available missions or not enough available missions, reset the available list of missions
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
        EventManager.currentManager.AddEvent(new CurrentActiveMissions(activeMissions,false));
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

        //Save all available missions to the mission pool and reset them based on their week of availability
        for (int week = 0; week <= currentWeekIndex; week++)
        {
            PoolWeekSpecificMissions(week);
        }
    }

    //Pools the new missions that belong to the new upcoming week 
    private bool PoolWeekSpecificMissions(int givenWeek)
    {
        //Error handling to prevent pooling from non-existant weeks, return early without new additions
        if (givenWeek >= missionObjects.Count) { return false; }

        //Iterate over the week being pooled missions
        foreach (GameObject missionObject in missionObjects[givenWeek])
        {
            //Reset the mission data
            missionObject.GetComponent<Mission>().ResetMission();

            //Disable the object
            missionObject.SetActive(false);

            //Add to missions pool 
            availableMissionsPool.Add(missionObject);
        }

        //Return as sucessful
        return true;
    }

    //Get a set number of active missions
    private void PoolNewActiveMissions()
    {
        //Get a set number of active missions while under the number of active missions and has available missions to pull
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

    //Moves the active missions into the completed missions
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

    private void UpdateMissionObjectMatrixIndices(int givenWeek)
    {
        //While the weekly missions count does not match the targetted number of weeks, create it
        while(missionObjects.Count < givenWeek)
        {
            //Create the new week slot
            missionObjects.Add(new List<GameObject>());
        }
    }
}
