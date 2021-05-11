using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private bool debugStarted = true;

    private void Start()
    {
        //Subscribes script to check for any debugs
        EventManager.currentManager.Subscribe(EventType.RECREIVEDEBUG, OnDebugStart);
    }

    private void Update()
    {
        AlternateDebugging();
        CheckIfActive();
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

    private void AlternateDebugging()
    {
        //alternates the script between sending logs and not
        if (Input.GetKeyDown(KeyCode.F7))
        {
            if (debugStarted)
            {
                Debug.Log("Debugger Disabled");
                EventManager.currentManager.Unsubscribe(EventType.RECREIVEDEBUG, OnDebugStart); 
            }
            else
            { 
                EventManager.currentManager.Subscribe(EventType.RECREIVEDEBUG, OnDebugStart);
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
            DebugLog("Ping");
        }
    }

    public static void DebugLog(string log)
    {
        EventManager.currentManager.AddEvent(new SendDebugLog(log));
    }

}
