using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Event that informs subscribers of something
public class SendDebugLog : EventData
{
    public readonly string debuglog;

    public SendDebugLog(string givenLog) : base(EventType.RECREIVEDEBUG)
    {
        debuglog = givenLog;
    }
}
