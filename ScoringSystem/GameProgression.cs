using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgression : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float daySpeedIncreasePercentilePerDay = 0.5f;
    [Range(0, 10)] [SerializeField] private int scoreIncreaseScalar = 6;
    [Range(0, 100)] [SerializeField] private int scoreIncreaseModifier = 20;

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
        EventManager.currentManager.AddEvent(new ScoreIncreasePerWeek(scoreIncreaseModifier,scoreIncreaseScalar));
    }
    #endregion

}
