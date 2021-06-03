using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    [SerializeField] AudioSource audioSourceComponent=null;
    [SerializeField] AudioClip buttonClickSound=null;
    [SerializeField] AudioClip buttonHoverSound=null;
    // Start is called before the first frame update
    public void HoverSound()
    {
        if (buttonHoverSound != null)
        {
            audioSourceComponent.PlayOneShot(buttonHoverSound);
        }
        else
        {
            Debug.LogWarning("No sound for buttonFX hover sound selected");
        }
    }
    public void ClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSourceComponent.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogWarning("No sound for buttonFX click sound selected");
        }
    }
}
