using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlantHolder : MonoBehaviour
{
    private static GameObject beetleFX;
    private static GameObject beeFX;
    private static GameObject butteflyFX;
    private static GameObject diseaseFX;
    private static GameObject invaderFX;

    private static bool showBeetles, showBees, showButterfly;

    private void Start()
    {
        //loads all of the different effects of the plants.
        beetleFX = Resources.Load<GameObject>("Effects/BeetleEffect");
        beeFX = Resources.Load<GameObject>("Effects/BeeEffect");
        butteflyFX = Resources.Load<GameObject>("Effects/ButterflyEffect");
        diseaseFX = Resources.Load<GameObject>("Effects/DiseaseEffect");
        invaderFX = Resources.Load<GameObject>("Effects/InvaderEffect");
    }

    private void OnEnable()
    {
        //Subscribes to different threshold events
        EventManager.currentManager.Subscribe(EventType.BEETHRESHOLDREACHED, OnThresholdReached);
        EventManager.currentManager.Subscribe(EventType.BEETHRESHOLDLOST, OnThresholdLost);
        EventManager.currentManager.Subscribe(EventType.BEETLETHRESHOLDREACHED, OnThresholdReached);
        EventManager.currentManager.Subscribe(EventType.BEETLETHRESHOLDLOST, OnThresholdLost);
        EventManager.currentManager.Subscribe(EventType.BUTTERFLYTHRESHOLDREACHED, OnThresholdReached);
        EventManager.currentManager.Subscribe(EventType.BUTTERFLYTHRESHOLDLOST, OnThresholdLost);
    }
    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.BEETHRESHOLDREACHED, OnThresholdReached);
        EventManager.currentManager.Unsubscribe(EventType.BEETHRESHOLDLOST, OnThresholdLost);
        EventManager.currentManager.Unsubscribe(EventType.BEETLETHRESHOLDREACHED, OnThresholdReached);
        EventManager.currentManager.Unsubscribe(EventType.BEETLETHRESHOLDLOST, OnThresholdLost);
        EventManager.currentManager.Unsubscribe(EventType.BUTTERFLYTHRESHOLDREACHED, OnThresholdReached);
        EventManager.currentManager.Unsubscribe(EventType.BUTTERFLYTHRESHOLDLOST, OnThresholdLost);
    }
    //Checks which insects are below their threshold
    private void OnThresholdLost(EventData eventData)
    {
        if (eventData is BeeThresholdLost)
        {
            showBees = false;
        }
        if (eventData is BeetleThresholdLost)
        {
            showBeetles = false;
        }
        if (eventData is ButterflyThresholdLost)
        {
            showButterfly = false;
        }
    }

    //Checks which insects are above their threshold
    private void OnThresholdReached(EventData eventData)
    {
        
        if (eventData is BeeThresholdReached)
        {
            showBees = true;
        }
        if (eventData is BeetleThresholdReached)
        {
            showBeetles = true;
        }
        if (eventData is ButterflyThresholdReached)
        {
            showButterfly = true;
        }
    }



    #region Getters
    public static GameObject GetBeetleFX()
    {
        return beetleFX;
    }

    public static GameObject GetBeeFX()
    {
        return beeFX;
    }

    public static GameObject GetButterflyFX()
    {
        return butteflyFX;
    }

    public static GameObject GetDiseaseFX()
    {
        return diseaseFX;
    }

    public static GameObject GetInvaderFX()
    {
        return invaderFX;
    }

    public static bool GetShowBeetles()
    {
        return showBeetles;
    }
    public static bool GetShowBees()
    {
        return showBees;

    }
    public static bool GetShowButterflies()
    {
        return showButterfly;
    }
    #endregion
}
