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

    private void Start()
    {
        beetleFX = Resources.Load<GameObject>("Effects/BeetleEffect");
        beeFX = Resources.Load<GameObject>("Effects/BeetleEffect");
        butteflyFX = Resources.Load<GameObject>("Effects/ButterflyEffect");
        diseaseFX = Resources.Load<GameObject>("Effects/DiseaseEffect");
        invaderFX = Resources.Load<GameObject>("Effects/InvaderEffect");
    }

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
}
