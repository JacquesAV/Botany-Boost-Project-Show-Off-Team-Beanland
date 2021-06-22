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
    public PlantInfected() : base(EventType.PLANTINFECTED)
    {
        
    }
}

//Event that informs subscribers of plant being cured
public class PlantCured: EventData
{
    public PlantCured() : base(EventType.PLANTCURED)
    {

    }
}

//Event that informs subscribers of plant being invaded
public class PlantInvaded : EventData
{
    public PlantInvaded() : base(EventType.PLANTINVADED)
    {

    }
}

//Event that informs subscribers of plant invasive species being removed
public class PlantGassed : EventData
{
    public PlantGassed() : base(EventType.PLANTGASSED)
    {

    }
}

//Event that informs player score manager of a cure, gas or both request
public class PlantCureGasRequest : EventData
{
    public readonly bool isCureRequest;
    public readonly bool isGasRequest;
    public readonly bool isSick;
    public readonly bool isInvaded;

    public PlantCureGasRequest(bool givenIsCureRequest, bool givenIsGasRequest, bool givenIsSick, bool givenIsInvaded) : base(EventType.PLANTCUREGASREQUEST)
    {
        isCureRequest = givenIsCureRequest;
        isGasRequest = givenIsGasRequest;
        isSick = givenIsSick;
        isInvaded = givenIsInvaded;
    }
}

//Event that informs player score manager of a cure, gas or both request
public class PlantCureGasRequestResult : EventData
{
    public readonly bool wasCureApproved;
    public readonly bool wasGasApproved;

    public PlantCureGasRequestResult(bool givenCureApproval, bool givenGasApproval) : base(EventType.PLANTCUREGASREQUESTRESULT)
    {
        wasCureApproved = givenCureApproval;
        wasGasApproved = givenGasApproval;
    }
}

//Event that informs subscribers of changes in the score manager, the ideal is to be used by a display view
public class TotalScoresUpdated : EventData
{
    public readonly int money;
    public readonly int biodiversity;
    public readonly int carbonIntake;
    public readonly int appealScore;
    public readonly int invasiveness;
    public readonly int infections;
    public readonly int totalScore;

    public TotalScoresUpdated(int givenMoney, int givenBiodiversity, int givenCarbonIntake, int givenAppealScore, int givenInvasiveness, int givenInfections, int givenTotalScore) : base(EventType.TOTALSCORESUPDATED)
    {
        money = givenMoney;
        biodiversity = givenBiodiversity;
        carbonIntake = givenCarbonIntake;
        appealScore = givenAppealScore;
        invasiveness = givenInvasiveness;
        infections = givenInfections;
        totalScore = givenTotalScore;
    }
}

//Event that informs subscribers of changes in the score manager, the ideal is to be used by a display view
public class ObjectBoughtScores : EventData
{
    public readonly int cost;
    public readonly int biodiversity;
    public readonly int carbonIntake;
    public readonly int attractiveScore;
    public readonly InsectType insectType;
    public readonly int insectAttractiveness;

    public ObjectBoughtScores(int givenCost, int givenBiodiversity, int givenCarbonIntake, int givenAttractiveScore, InsectType givenInsectType, int givenInsectAttractiveness) : base(EventType.OBJECTBOUGHTSCORES)
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
public class ObjectSoldScores : EventData
{
    public readonly int refund;
    public readonly int biodiversity;
    public readonly int carbonIntake;
    public readonly int attractiveScore;
    public readonly InsectType insectType;
    public readonly int insectAttractiveness;

    public ObjectSoldScores(int givenRefund, int givenBiodiversity, int givenCarbonIntake, int givenAttractiveScore, InsectType givenInsectType, int givenInsectAttractiveness) : base(EventType.OBJECTSOLDSCORES)
    {
        refund = givenRefund;
        biodiversity = givenBiodiversity;
        carbonIntake = givenCarbonIntake;
        attractiveScore = givenAttractiveScore;
        insectType = givenInsectType;
        insectAttractiveness = givenInsectAttractiveness;
    }
}

//Event that informs subscribers that a buy request is being made
public class ObjectBuyRequest : EventData
{
    public readonly PlaceableData requestedObject;

    public ObjectBuyRequest(PlaceableData givenRequestedObject) : base(EventType.OBJECTBUYREQUEST)
    {
        requestedObject = givenRequestedObject;
    }
}

//Event that informs subscribers that a buy request was denied or allowed
public class ObjectBuyRequestResult : EventData
{
    public readonly PlaceableData requestedObject;
    public readonly bool wasAllowed;

    public ObjectBuyRequestResult(PlaceableData givenRequestedObject, bool givenWasAllowed) : base(EventType.OBJECTBUYREQUESTRESULT)
    {
        requestedObject = givenRequestedObject;
        wasAllowed = givenWasAllowed;
    }
}

//Event that informs subscribers of the current hovered tile
public class CurrentHoveredTile : EventData
{
    public readonly GameObject currentHoveredTile;
    public CurrentHoveredTile(GameObject givenTile) : base(EventType.CURRENTHOVEREDTILE)
    {
        currentHoveredTile = givenTile;
    }
}

//Event that informs subscribers of new active missions
public class CurrentActiveMissions : EventData
{
    public readonly List<GameObject> activeMissions;
    public readonly bool isUpdate;
    public CurrentActiveMissions(List<GameObject> givenActiveMissions, bool givenIsUpdate) : base(EventType.CURRENTACTIVEMISSIONS)
    {
        activeMissions = givenActiveMissions;
        isUpdate = givenIsUpdate;
    }
}

//Event that informs subscribers of a mission being completed and its reward value
public class MissionCompleted : EventData
{
    public readonly int rewardAmount;
    public MissionCompleted(int givenReward) : base(EventType.MISSIONCOMPLETED)
    {
        rewardAmount = givenReward;
    }
}

//Event that informs subscribers of mission relevant info
public class ObjectMissionData : EventData
{
    public readonly PlaceableType placeableType;
    public readonly InsectType insectType;
    public readonly bool yieldsProduce;
    public readonly bool attractsBirds;
    public ObjectMissionData(PlaceableType givenPlaceableType, InsectType givenInsectType, bool givenYieldsProduce, bool givenAttractsBirds) : base(EventType.MISSIONDATA)
    {
        placeableType = givenPlaceableType;
        insectType = givenInsectType;
        yieldsProduce = givenYieldsProduce;
        attractsBirds = givenAttractsBirds;
    }
}

//Event that informs subscribers of mission relevant info
public class PlacedObjectMissionData : ObjectMissionData
{
    public PlacedObjectMissionData(PlaceableType givenPlaceableType, InsectType givenInsectType, bool givenYieldsProduce, bool givenAttractsBirds) : base(givenPlaceableType, givenInsectType, givenYieldsProduce, givenAttractsBirds)
    {
    }
}

//Event that informs subscribers of mission relevant info
public class RemovedObjectMissionData : ObjectMissionData
{
    public RemovedObjectMissionData(PlaceableType givenPlaceableType, InsectType givenInsectType, bool givenYieldsProduce, bool givenAttractsBirds) : base(givenPlaceableType, givenInsectType, givenYieldsProduce, givenAttractsBirds)
    {
    }
}

//Event that informs subscribers of mission relevant info
public class RequestScoreData : EventData
{
    public RequestScoreData() : base(EventType.REQUESTSCOREDATA)
    {
    }
}

//Event that informs subscribers of a mission experiencing a change/update
public class MissionUpdated : EventData
{
    public MissionUpdated() : base(EventType.MISSIONUPDATED)
    {
    }
}

//Event that informs subscribers of a threshold being reached
public class BeetleThresholdReached : EventData
{
    public BeetleThresholdReached() : base(EventType.BEETLETHRESHOLDREACHED)
    {
    }
}

//Event that informs subscribers of a threshold being lost
public class BeetleThresholdLost : EventData
{
    public BeetleThresholdLost() : base(EventType.BEETLETHRESHOLDLOST)
    {
    }
}

//Event that informs subscribers of a threshold being reached
public class ButterflyThresholdReached : EventData
{
    public ButterflyThresholdReached() : base(EventType.BUTTERFLYTHRESHOLDREACHED)
    {
    }
}

//Event that informs subscribers of a threshold being lost
public class ButterflyThresholdLost : EventData
{
    public ButterflyThresholdLost() : base(EventType.BUTTERFLYTHRESHOLDLOST)
    {
    }
}

//Event that informs subscribers of a threshold being reached
public class BeeThresholdReached : EventData
{
    public BeeThresholdReached() : base(EventType.BEETHRESHOLDREACHED)
    {
    }
}

//Event that informs subscribers of a threshold being lost
public class BeeThresholdLost : EventData
{
    public BeeThresholdLost() : base(EventType.BEETHRESHOLDLOST)
    {
    }
}

//Event that informs subscribers that the escape key was pressed 
public class BuildingManagerEscapeKeyPressed : EventData
{
    public readonly PlayerInteractionState playerState;

    public BuildingManagerEscapeKeyPressed(PlayerInteractionState givenState) : base(EventType.BUILDINGMANAGERESCAPEKEYPRESSED)
    {
        playerState = givenState;
    }
}

//Event that informs subscribers about a requested change in the player interaction state
public class ActivateInteractionState : EventData
{
    public readonly PlayerInteractionState interactionState;

    public ActivateInteractionState(PlayerInteractionState givenState) : base(EventType.ACTIVATEINTERACTIONSTATE)
    {
        interactionState = givenState;
    }
}

//Event that informs subscribers about day time being sped up
public class SpeedUpDays : EventData
{
    public readonly float daySpeedIncreasePercentile;

    public SpeedUpDays(float givenDaySpeedPercentile) : base(EventType.DAYSPEEDUP)
    {
        daySpeedIncreasePercentile = givenDaySpeedPercentile;
    }
}

//Event that informs subscribers about scores being increased
public class ScoreIncreasePerWeek : EventData
{
    public readonly int modifier;
    public readonly int scalar;
    public ScoreIncreasePerWeek(int modifier, int scalar) : base(EventType.WEEKLYSCOREINCREASE)
    {
        this.modifier = modifier;
        this.scalar = scalar;
    }
}

//Event that informs subscribers about game being over
public class GameOver : EventData
{
    public GameOver() : base(EventType.GAMEOVER)
    {
    }
}

//Event that informs subscribers about game being over
public class CurrentObjectOffsetUpdated : EventData
{
    public readonly Vector3 offset;
    public readonly Vector2 dimension;

    public CurrentObjectOffsetUpdated(Vector3 givenOffset, Vector2 givenDimension) : base(EventType.CURRENTOBJECTOFFSETUPDATED)
    {
        offset = givenOffset;
        dimension = givenDimension;
    }
}

//Event that informs subscribers about game being over
public class LowestLeaderboardScore : EventData
{
    public readonly int score;
    public readonly int leaderboardSize;

    public LowestLeaderboardScore(int givenScore,int leaderboardSize) : base(EventType.LowestLeaderboardScore)
    {
        score = givenScore;
        this.leaderboardSize = leaderboardSize;
    }
}

//Event that informs subscribers of a money being added
public class MoneyEarned : EventData
{
    public readonly int moneyEarned;
    public MoneyEarned(int givenMoney) : base(EventType.MoneyEarned)
    {
        moneyEarned = givenMoney;
    }
}