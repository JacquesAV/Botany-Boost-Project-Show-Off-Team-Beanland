using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpTime : MonoBehaviour
{
    public void SpeedUpByValue(int speedUpTime)
    {
        Time.timeScale = speedUpTime;
    }

    public void NormalSpeed()
    {
        Time.timeScale = 1;
    }
}
