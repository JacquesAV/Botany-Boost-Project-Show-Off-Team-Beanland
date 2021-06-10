using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TimeCycle : MonoBehaviour
{
    [SerializeField] private float dayLength = 15;//how long day time is
    [SerializeField] private float nightLength = 5;//how long night time is

    [SerializeField] private Color dayColor = new Color(1, 1, 1);//the global lights color during day
    [SerializeField] private Color nightColor = new Color(0.2196078f, 0.08627451f, 0.08627451f);//the global lights color during night

    [SerializeField] private HDAdditionalLightData sceneLight;//The scene light
    public bool Daytime { get; private set; }//whether it is daytime or nightime
    private float timeRemaining;//amount of time of the current part of day that is left
    private int currentDayInTheWeek=1;//The day of the week
    private int currentWeek = 1;//The current week *shrugs*

    private int dayThreshold=4;
    private int nightThreshold=2;

    private bool gameOver = false;

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

    private void Start()
    {
        if (sceneLight==null)
        {
            DebugManager.DebugLog("No Light Source was added");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameOver)
        {
            AlternateDaytime();
            ChangeColorGradientBasedOnTime();
        }
    }

    private void OnDaySpeedUp(EventData eventData)
    {
        if (eventData is SpeedUpDays)
        {
            //Cast the event so it can be used
            SpeedUpDays daySpeedUp = (SpeedUpDays)eventData;

            //reduce day time
            dayLength -= dayLength* daySpeedUp.daySpeedIncreasePercentile;
            nightLength -= nightLength* daySpeedUp.daySpeedIncreasePercentile;
            //threshold to prevent time getting too fast
            if (dayLength < dayThreshold)
            {
                dayLength = dayThreshold;
            }
            if (nightLength < nightThreshold)
            {
                nightLength = nightThreshold;
            }
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

        private void AlternateDaytime()
    {
        //removes time passed from time remaining
        timeRemaining -= Time.deltaTime;
        //if there is no time remaining
        if (timeRemaining <= 0)
        {
            //swaps daytime
            Daytime = !Daytime;
            //If it is day
            if (Daytime)
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
    private void ChangeColorGradientBasedOnTime()
    {
        //If it is day
        if (Daytime)
        {
            sceneLight.color=BlendColorValue(nightColor, dayColor,dayLength);
        }
        //Else nightime
        else
        {
            sceneLight.color=BlendColorValue(dayColor, nightColor, nightLength);
        }
    }
    //Blends the colors together for clean transition of the color
    private Color BlendColorValue(Color orinalColor, Color newColor,float dayOrNightLength)
    {
        //convers into a fraction that is between 1-0
        float timePassed=timeRemaining/dayOrNightLength;
        //linear average blending
        return (1-timePassed)*orinalColor+timePassed*newColor;
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
