using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    //Singleton
    //Getter and setter for the current debugger, static so that there is only one manager at any given time
    public static DebugManager currentDebugger { get; set; } = null;
    private bool debugStarted = true;
    [SerializeField] private Transform debugUI=null;

    //Awake function ensures that only one copy exists in the scene at a given time
    private void Awake()
    {
        if (currentDebugger == null)
        {
            currentDebugger = this; //Sets the active manager to this instance of it
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Subscribes script to check for any debugs
        EventManager.currentManager.Subscribe(EventType.RECREIVEDEBUG, OnDebugStart);

        if (debugUI == null)
        {
            debugUI = GameObject.Find("Canvas").transform.Find("DebugPannel");
        }
    }

    private void Update()
    {
        ToggleDebugging();
        CheckIfActive();
        SkipCurrentDay();
        SkipCurrentWeek();
    }

    public void OnDebugStart(EventData eventData)
    {
        //if debug log event is sent, will print it only when the debug log is active.
        if (eventData is SendDebugLog log)
        {
            Debug.Log(log.debuglog);
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.RECREIVEDEBUG was received but is not of class SendDebugLog.");
        }
    }

    private void ToggleDebugging()
    {
        //Toggles the script between sending logs and not
        if (Input.GetKeyDown(KeyCode.F7))
        {
            if (debugStarted)
            {
                Debug.Log("Debugger Disabled");
                debugUI.transform.gameObject.SetActive(false);
                EventManager.currentManager.Unsubscribe(EventType.RECREIVEDEBUG, OnDebugStart);
            }
            else
            {
                EventManager.currentManager.Subscribe(EventType.RECREIVEDEBUG, OnDebugStart);
                debugUI.transform.gameObject.SetActive(true);
                DebugLog("Debugger Enabled");
            }
            debugStarted = !debugStarted;
        }
    }

    private void CheckIfActive()
    {
        //Sends a ping whenever key is pressed.
        if (Input.GetKeyDown(KeyCode.F8))
        {
            int uwuValue = Random.Range(1, 5);
            switch (uwuValue)
            {
                case 1:
                    DebugLog("Rawr X3 *nuzzles* How are you?");
                    break;
                case 2:
                    DebugLog("*pounces on you* you're so warm o3o *notices you have a bulge* someone's happy!");
                    break;
                        case 3:
                    DebugLog("UwU what’s this ?!");
                    break;
                case 4:
                    DebugLog("*nuzzles your necky wecky* ~murr~ hehe ;) *rubbies your bulgy wolgy* you're so big! ");
                    break;
                case 5:
                    DebugLog("I have a little itch o3o *wags tails* can you please get my itch?");
                    break;
            }
            
        }
    }

    public static void DebugLog(string log)
    {
        //Send the log to the event manager for debugging
        EventManager.currentManager.AddEvent(new SendDebugLog(log));
    }

    //----------------------------------------------------------------
    //                              Cheats                   
    //----------------------------------------------------------------

    //Skip to the next day
    private void SkipCurrentDay()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            EventManager.currentManager.AddEvent(new DayHasPassed());
        }
    }

    //Skips to next week
    private void SkipCurrentWeek()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            EventManager.currentManager.AddEvent(new WeekHasPassed());
        }
    }
}
