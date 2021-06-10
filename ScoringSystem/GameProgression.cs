using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgression : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float daySpeedIncreasePercentilePerDay = 0.5f;
    [Range(0, 1)] [SerializeField] private float scoreIncreasePercentilePerWeek = 0.35f;

    #region OnEvents
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
        EventManager.currentManager.AddEvent(new SpeedUpDays(daySpeedIncreasePercentilePerDay));
    }

    private void OnWeekPassed(EventData eventData)
    {
        EventManager.currentManager.AddEvent(new ScoreIncreasePerWeek(scoreIncreasePercentilePerWeek));
    }
    #endregion

}
