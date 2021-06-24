using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCompletedSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;//source to play from
    [SerializeField] private AudioClip audioClip;//clip to play
    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.MISSIONCOMPLETED, OnMissionCompleted);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.MISSIONCOMPLETED, OnMissionCompleted);
    }

    private void OnMissionCompleted(EventData eventData)
    {
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogWarning("No sound was added for mission completing sfx");
        }
    }
}
