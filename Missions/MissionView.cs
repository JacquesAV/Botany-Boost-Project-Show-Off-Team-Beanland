using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionView : MonoBehaviour
{
    [SerializeField]
    private GameObject missionLog = null; //The display container for the mission information
    private Dictionary<Mission,GameObject> missionsAndLogs = new Dictionary<Mission, GameObject>(); //Objects being displayed with their assosiated mission

    private void OnEnable()
    {
        //Subscribes the method and event type to the current manager
        EventManager.currentManager.Subscribe(EventType.CURRENTACTIVEMISSIONS, OnNewMissions);
    }
    private void OnDisable()
    {
        //Unsubscribes the method and event type to the current manager
        EventManager.currentManager.Unsubscribe(EventType.CURRENTACTIVEMISSIONS, OnNewMissions);
    }

    //If new missions arrive
    private void OnNewMissions(EventData eventData)
    {
        if (eventData is CurrentActiveMissions)
        {
            //Cast the event data into a usable list of object references
            List<GameObject> missionObjects = ((CurrentActiveMissions)eventData).activeMissions;

            //Instantiate mission logs with the data
            InstantiateMissionLogs(missionObjects);
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.CURRENTACTIVEMISSIONS was received but is not of class CurrentActiveMissions.");
        }
    }

    //Create mission logs with new missions
    private void InstantiateMissionLogs(List<GameObject> missionObjects)
    {
        //First destroy/clear old logs
        foreach (KeyValuePair<Mission, GameObject> missionLog in missionsAndLogs.ToDictionary(x => x.Key, x => x.Value))
        {
            //Destroy the old log
            Destroy(missionsAndLogs[missionLog.Key]);
        }

        //Clear the missions and logs
        missionsAndLogs.Clear();

        //Create new logs
        foreach (GameObject missionObject in missionObjects)
        {
            //Instantiate the log view and establish temporary variables
            GameObject logObject = Instantiate(missionLog, transform);
            Mission mission = missionObject.GetComponent<Mission>();

            //Update its view data
            logObject.GetComponent<MissionLog>().InitializeLogVisual(mission);

            //Add the key pair
            missionsAndLogs.Add(mission, logObject);
        }
    }
}
