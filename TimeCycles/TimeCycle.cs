using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TimeCycle : MonoBehaviour
{
    [SerializeField] private float dayLength = 15;//how long day time is
    [SerializeField] private float nightLength = 5;//how long night time is

    [SerializeField] private GameObject lightHolder;
    private bool isDaytime = false;//whether it is daytime or nightime
    private float timeRemaining;//amount of time of the current part of day that is left
    private int currentDayInTheWeek=1;//The day of the week
    private int currentWeek = 1;//The current week *shrugs*

    private int dayThreshold=4;

    private int nightThreshold=2;

    private bool gameOver = false;

    #region OnEvents
    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.DAYSPEEDUP, OnDaySpeedUp);
        EventManager.currentManager.Subscribe(EventType.GAMEOVER, OnGameOver);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.DAYSPEEDUP, OnDaySpeedUp);
        EventManager.currentManager.Unsubscribe(EventType.GAMEOVER, OnGameOver);
    }

    private void OnDaySpeedUp(EventData eventData)
    {
        if (eventData is SpeedUpDays)
        {
            Debug.Log("speeding up");
            //Cast the event so it can be used
            SpeedUpDays daySpeedUp = (SpeedUpDays)eventData;

            //reduce day time
            float dayTimeDecrease = dayLength * daySpeedUp.daySpeedIncreasePercentile;
            float nightTimeDecrease = nightLength * daySpeedUp.daySpeedIncreasePercentile;

            dayLength -= dayTimeDecrease;
            nightLength -= nightTimeDecrease;

            if (isDaytime)
            {
                timeRemaining = dayLength;
            }
            else
            {
                timeRemaining = nightLength;
            }


            //threshold to prevent time getting too fast
            if (dayLength < dayThreshold)
            {
                dayLength = dayThreshold;
            }
            if (nightLength < nightThreshold)
            {
                nightLength = nightThreshold;
            }
            //Debug.Log("day length is: " + dayLength + "and time Reaming of day is: " + timeRemaining);
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.DAYSPEEDUP was received but is not of class SpeedUpDays.");
        }
    }

    private void OnGameOver(EventData eventData)
    {
        if (eventData is GameOver)
        {
            //game is over
            gameOver = true;
        }
        else
        {
            throw new System.Exception("Error: EventData class with EventType.GAMEOVER was received but is not of class GameOver.");
        }
    }
    #endregion
    private void Start()
    {
        if (lightHolder == null)
        {
            DebugManager.DebugLog("No Light holder was added");
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameOver)
        {
            AlternateDaytime();
            ChangeLightBasedOnTime();
        }
    }

    private void AlternateDaytime()
    {
        //removes time passed from time remaining
        timeRemaining -= Time.deltaTime;
        //if there is no time remaining
        if (timeRemaining <= 0)
        {
            //swaps daytime
            isDaytime = !isDaytime;
            //If it is day
            if (isDaytime)
            {
                DebugManager.DebugLog("Day has started");
                //set the time remaining to the day length
                timeRemaining = dayLength;
            }
            //Else nightime
            else
            {
                DebugManager.DebugLog("Night has started");
                //set the time remaining to the night length
                timeRemaining = nightLength;
                //Send out event that Day has passed
                currentDayInTheWeek++;
                AlternateWeek();
                EventManager.currentManager.AddEvent(new DayHasPassed());
            }
        }
    }
    //Blends color based on the current time of day
    private void ChangeLightBasedOnTime()
    {
        //If it is day
        if (isDaytime)
        {
            RotateSun(0, dayLength);
        }
        //Else nightime
        else
        {
            RotateSun(180, nightLength);
        }
    }

    //Rotates the directional lights to create an effect of time passing
    private void RotateSun(int rotatonIncrease, float dayOrNightLength)
    {
        //convers into a fraction that is between 1-0
        float timePassed = timeRemaining / dayOrNightLength;

        //debug to check if something is wrong with time passing
        //Debug.Log("timepassed: " + timePassed + "=timeRemaining: " + timeRemaining + "/dayOrNightLength: " + dayOrNightLength);
        
        //with the time decreasing it causes an issue where the value can be more than 1, this is a simple workaround.
        if (timePassed > 1)
        {
            timePassed = 1;
        }

        float sunRotation = Mathf.Lerp(0+rotatonIncrease, 180 + rotatonIncrease, timePassed);
        if (lightHolder != null)
        {
            lightHolder.transform.rotation = Quaternion.Euler(sunRotation, -150.0f, 0);
        }
        else
        {
            Debug.LogWarning("No light holder in time cycle was selected, please do so.");
        }
    }

    private void AlternateWeek()
    {
        //If the full week has passed
        if (currentDayInTheWeek > 7)
        {
            //sets the day back to 1
            currentDayInTheWeek = 1;
            //increases the week
            currentWeek++;
            //Passes the week
            EventManager.currentManager.AddEvent(new WeekHasPassed());
            DebugManager.DebugLog("The current week has advanced to week " + currentWeek);
        }
    }
}
