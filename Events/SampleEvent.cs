using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Event that informs subscribers of something
public class SampleEvent : EventData
{
    public readonly bool sampleBool;

    public SampleEvent(bool givenBool) : base(EventType.SampleEvent)
    {
        sampleBool = givenBool;
    }
}
