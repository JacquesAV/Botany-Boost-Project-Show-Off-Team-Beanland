using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates an internal clock that similated delta time as a way to have smooth classes 
/// without needing to worry about game speed
/// </summary>
public class SimulatedDeltaTime : MonoBehaviour
{

    public static float deltaTime;
    private float lastTime;

    void Start()
    {
        lastTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        deltaTime = Time.realtimeSinceStartup - lastTime;

        lastTime = Time.realtimeSinceStartup;
    }
}
