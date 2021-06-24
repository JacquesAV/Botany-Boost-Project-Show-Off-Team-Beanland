using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekCompletedSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;//source to play from
    [SerializeField] private AudioClip audioClip;//clip to play
    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.WEEKPASSED, OnWeekCompleted);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.WEEKPASSED, OnWeekCompleted);
    }

    private void OnWeekCompleted(EventData eventData)
    {
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogWarning("No sound was added for week completing sfx");
        }
    }
}
