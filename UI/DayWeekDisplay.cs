using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayWeekDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;//Text for display
    private int currentDay=1;
    private int currentWeek = 1;

    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.DAYPASSED, OnDayPassed);
        EventManager.currentManager.Subscribe(EventType.WEEKPASSED, OnWeekPassed);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.DAYPASSED, OnDayPassed);
        EventManager.currentManager.Unsubscribe(EventType.WEEKPASSED, OnWeekPassed);
    }

    private void OnDayPassed(EventData eventData)
    {
        currentDay++;
        UpdateTimeStamp();
    }
    private void OnWeekPassed(EventData eventData)
    {
        currentDay = 0;
        currentWeek++;
        UpdateTimeStamp();
    }

    private void UpdateTimeStamp()
    {
        text.text = "Day: " + currentDay + " Week: " + currentWeek;
    }
}
