using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Event that informs subscribers of something
public class DebuggerStartEvent : EventData
{
    public readonly bool debuggerStarted;

    public DebuggerStartEvent(bool givenBool) : base(EventType.DEBUGGERSTARTEVENT)
    {
        debuggerStarted = givenBool;
    }
}
