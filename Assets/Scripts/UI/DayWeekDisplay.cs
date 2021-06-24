using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;

public class DayWeekDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;//Text for display
    private int currentDay = 1;
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
        UpdateLocalizedString();
    }

    private void OnWeekPassed(EventData eventData)
    {
        currentDay = 0;
        currentWeek++;
        UpdateLocalizedString();
    }

    public int GetDay()
    {
        return currentDay;
    }

    public int GetWeek()
    {
        return currentWeek;
    }

    private void UpdateLocalizedString()
    {
        //Error handling
        if(TryGetComponent(out LocalizeStringEvent stringEvent))
        {
            stringEvent.RefreshString();
        }
        else
        {
            Debug.LogWarning("No localized string event attached correctly, are you sure you set it up?");
        }
    }
}
