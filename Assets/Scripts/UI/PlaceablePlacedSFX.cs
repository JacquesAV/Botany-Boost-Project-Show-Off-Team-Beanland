using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceablePlacedSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;//source to play from
    [SerializeField] private AudioClip audioClip;//clip to play
    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.OBJECTBOUGHTSCORES, OnPlaceablePlaced);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.OBJECTBOUGHTSCORES, OnPlaceablePlaced);
    }

    private void OnPlaceablePlaced(EventData eventData)
    {
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogWarning("No sound was added for plant placing sfx");
        }
    }
}
