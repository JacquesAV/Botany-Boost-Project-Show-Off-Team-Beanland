using System.Collections;
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

//Event that informs subscribers of plant being infected
public class PlantInfected: EventData
{
    public PlantInfected() : base(EventType.GOTSICK)
    {
        
    }
}

//Event that informs subscribers of plant being cured
public class PlantCured: EventData
{
    public PlantCured() : base(EventType.GOTCURED)
    {

    }
}
//Event that informs subscribers of plant being invaded
public class PlantInvaded : EventData
{
    public PlantInvaded() : base(EventType.GOTINVADED)
    {

    }
}
//Event that informs subscribers of plant invasive species being removed
public class PlantGassed : EventData
{
    public PlantGassed() : base(EventType.GOTGASSED)
    {

    }
}

//Event that informs subscribers of changes in the score manager, the ideal is to be used by a display view
public class TotalScoresUpdated : EventData
{
    public readonly int money;
    public readonly int biodiversity;
    public readonly int carbonIntake;
    public readonly int attractiveScore;
    public readonly int invasiveness;
    public readonly int disease;

    public TotalScoresUpdated(int givenMoney, int givenBiodiversity, int givenCarbonIntake, int givenAttractiveScore, int givenInvasiveness, int givenDisease) : base(EventType.TOTALSCORESUPDATED)
    {
        money = givenMoney;
        biodiversity = givenBiodiversity;
        carbonIntake = givenCarbonIntake;
        attractiveScore = givenAttractiveScore;
        invasiveness = givenInvasiveness;
        disease = givenDisease;
    }
}

//Event that informs subscribers of changes in the score manager, the ideal is to be used by a display view
public class ObjectPlacedScores : EventData
{
    public readonly int cost;
    public readonly int biodiversity;
    public readonly int carbonIntake;
    public readonly int attractiveScore;
    public readonly InsectType insectType;
    public readonly int insectAttractiveness;

    public ObjectPlacedScores(int givenCost, int givenBiodiversity, int givenCarbonIntake, int givenAttractiveScore, InsectType givenInsectType, int givenInsectAttractiveness) : base(EventType.OBJECTPLACEDSCORES)
    {
        cost = givenCost;
        biodiversity = givenBiodiversity;
        carbonIntake = givenCarbonIntake;
        attractiveScore = givenAttractiveScore;
        insectType = givenInsectType;
        insectAttractiveness = givenInsectAttractiveness;
    }
}

//Event that informs subscribers of changes in the score manager, the ideal is to be used by a display view
public class ObjectRemovedScores : EventData
{
    public readonly int refund;
    public readonly int biodiversity;
    public readonly int carbonIntake;
    public readonly int attractiveScore;
    public readonly InsectType insectType;
    public readonly int insectAttractiveness;

    public ObjectRemovedScores(int givenCost, int givenBiodiversity, int givenCarbonIntake, int givenAttractiveScore, InsectType givenInsectType, int givenInsectAttractiveness) : base(EventType.OBJECTREMOVEDSCORES)
    {
        refund = givenCost;
        biodiversity = givenBiodiversity;
        carbonIntake = givenCarbonIntake;
        attractiveScore = givenAttractiveScore;
        insectType = givenInsectType;
        insectAttractiveness = givenInsectAttractiveness;
    }
}