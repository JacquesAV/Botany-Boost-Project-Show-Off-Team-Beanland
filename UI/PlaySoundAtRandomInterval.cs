using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAtRandomInterval : MonoBehaviour
{
    [SerializeField] private AudioClip[] adioClips;//audio clips that could be played
    [SerializeField] private AudioSource audioSource;//the audio source where sound is played from
    [SerializeField] private int minTimeUntilNextPlay = 6;//the minimum time until the sound could be played
    [SerializeField] private int maxTimeUntilNextPlay = 7;//the maximum time until the sound could be played
    private int randomTimeBetween=5;//the time until the sound will be played

    private readonly bool keepPlaying = true;

    void Start()
    {
        StartCoroutine(SoundOut());
    }

    IEnumerator SoundOut()
    {
        //select a time until bird sound is played
        randomTimeBetween = Random.Range(minTimeUntilNextPlay, maxTimeUntilNextPlay);
        //Will continously play while true
        while (keepPlaying)
        {
            //chose an audio clip to play
            int audioClipToPlay = Random.Range(0, adioClips.Length - 1);
            audioSource.PlayOneShot(adioClips[audioClipToPlay]);
            //wait until next sound can be played
            yield return new WaitForSeconds(randomTimeBetween);
            //chose a new time until bird sound is played
            randomTimeBetween = Random.Range(minTimeUntilNextPlay, maxTimeUntilNextPlay);
            //Debug.Log("time until next play: " + randomTimeBetween);
        }
    }

}
