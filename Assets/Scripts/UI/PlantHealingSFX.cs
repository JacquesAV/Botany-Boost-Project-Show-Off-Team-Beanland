using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHealingSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;//source to play from
    [SerializeField] private AudioClip gassedClip;//clip to play when gassed
    [SerializeField] private AudioClip curedClip;//clip to play when cured
    [SerializeField] private AudioClip failedClip;//clip to play when failed
    private void OnEnable()
    {
        EventManager.currentManager.Subscribe(EventType.PLANTCUREGASREQUESTRESULT, OnPlantCuredGassed);
    }

    private void OnDisable()
    {
        EventManager.currentManager.Unsubscribe(EventType.OBJECTBOUGHTSCORES, OnPlantCuredGassed);
    }

    private void OnPlantCuredGassed(EventData eventData)
    {
        if (eventData is PlantCureGasRequestResult)
        {
            PlantCureGasRequestResult plantStatus = (PlantCureGasRequestResult)eventData;
            //plant successfuly cured
            if (plantStatus.wasCureApproved)
            {
                if (curedClip != null)
                {
                    audioSource.PlayOneShot(curedClip);
                }
                else
                {
                    Debug.LogWarning("No sound was added for cured success sfx");
                }
            }
            //plant successfuly gassed
            else if (plantStatus.wasGasApproved)
            {
                if (gassedClip != null)
                {
                    audioSource.PlayOneShot(gassedClip);
                }
                else
                {
                    Debug.LogWarning("No sound was added for gassed success sfx");
                }
            }
            //plant failed to be both gassed/cured
            else
            {
                if (failedClip != null)
                {
                    audioSource.PlayOneShot(failedClip);
                }
                else
                {
                    Debug.LogWarning("No sound was added for failed to cure/gass sfx");
                }
            }
        }
        else
        {
            Debug.LogWarning("Eventdata for plantcure is not of correct data type, must be PlantCureGasRequestResult");
        }
    }
}
