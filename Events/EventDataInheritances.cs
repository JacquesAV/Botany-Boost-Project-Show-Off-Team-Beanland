﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Event that informs subscribers of a debug log
public class SendDebugLog : EventData
{
    public readonly string debuglog;

    public SendDebugLog(string givenLog) : base(EventType.RECREIVEDEBUG)
    {
        debuglog = givenLog;
    }
}

//Event that informs subscribers of a placeable being selected
public class PlaceableSelectedOnGUI : EventData
{
    public readonly PlaceableData placeable;

    public PlaceableSelectedOnGUI(PlaceableData givenPlaceable) : base(EventType.CLICKEDPLACEABLEGUI)
    {
        placeable = givenPlaceable;
    }
}

//Event that informs subscribers of a week passing in game
public class WeekHasPassed : EventData
{
    public WeekHasPassed() : base(EventType.WEEKPASSED)
    {
    }
}

//Event that informs subscribers of a day passing in game
public class DayHasPassed : EventData
{
    public DayHasPassed() : base(EventType.DAYPASSED)
    {
    }
}
