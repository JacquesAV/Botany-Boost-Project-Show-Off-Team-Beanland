using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TimeCycle : MonoBehaviour
{
    [SerializeField] private float dayLength = 15;//how long day time is
    [SerializeField] private float nightLength = 5;//how long night time is
    [SerializeField] private HDAdditionalLightData sceneLight;//The scene light

    private float upperLuxBound = 130000; //The maximum value for the lux emission intensity lighting
    private float middleLuxBound = 10000; //The middle bound that separates day and night
    private float lowerLuxBound = 0; //The minimum value for the lux emission intensity lighting

    public bool Daytime { get; private set; }//whether it is daytime or nightime
    private float timeRemaining;//amount of time of the current part of day that is left
    private int currentDayInTheWeek=1;//The day of the week
    private int currentWeek = 1;//The current week *shrugs*

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
        //Checks for a day change
        AlternateDaytime();

        //ChangeColorGradientBasedOnTime();
        ChangeLightingEmmisionIntensityBasedOnTime();
    }

    //Checks for when a day has passed
    private void AlternateDaytime()
    {
        //Removes time passed from time remaining
        timeRemaining -= Time.deltaTime;

        //If there is no time remaining
        if (timeRemaining <= 0)
        {
            //Swaps daytime
            Daytime = !Daytime;

            //If it is day
            if (Daytime)
            {
                //Debug
                DebugManager.DebugLog("Day has started");

                //Set the time remaining to the day length
                timeRemaining = dayLength;
            }
            //Else nightime
            else
            {
                //Debug
                DebugManager.DebugLog("Night has started");

                //Set the time remaining to the night length
                timeRemaining = nightLength;

                //Send out event that Day has passed
                EventManager.currentManager.AddEvent(new DayHasPassed());

                //Mark that a day has passed
                currentDayInTheWeek++;

                //Check for a week change
                AlternateWeek();
            }
        }
    }

    //Changes the emmision intensity of the HDRP lighting based on the expected time of day
    private void ChangeLightingEmmisionIntensityBasedOnTime()
    {
        //Sets the emission based on the current time of day
        if (Daytime)
        {
            //Calculate change rate for the intensity based on day length
            float changePerFrame = GetIntensityChangePerFrame(upperLuxBound - middleLuxBound, dayLength);

            //If early in day
            if (timeRemaining >= dayLength / 2)
            {
                //Move towards highest value
                sceneLight.SetIntensity(Mathf.MoveTowards(sceneLight.intensity, upperLuxBound, changePerFrame));
            }
            //If late in day
            else
            {
                //Move towards transition value
                sceneLight.SetIntensity(Mathf.MoveTowards(sceneLight.intensity, middleLuxBound, changePerFrame));
            }
        }
        else
        {
            //Calculate change rate for the intensity based on night length
            float changePerFrame = GetIntensityChangePerFrame(middleLuxBound, nightLength);

            //If early in night
            if (timeRemaining >= nightLength / 2)
            {
                //Move towards lowest value
                sceneLight.SetIntensity(Mathf.MoveTowards(sceneLight.intensity, lowerLuxBound, changePerFrame));
            }
            //If late in night
            else
            {
                //Move towards transition value
                sceneLight.SetIntensity(Mathf.MoveTowards(sceneLight.intensity, middleLuxBound, changePerFrame));
            }
        }
    }

    //Gets the intensity based on day/night length for half a day/night cycle
    private float GetIntensityChangePerFrame(float givenLuxBound, float dayOrNightLength)
    {
        return givenLuxBound / (dayOrNightLength / 2) * Time.deltaTime;
    }

    //Checks for when a week has passed
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
