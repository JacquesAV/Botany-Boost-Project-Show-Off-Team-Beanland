using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButtonImageToggle : MonoBehaviour
{
    [SerializeField] private Sprite inactive, active;
    [SerializeField] private Image image = null;

    public void Start()
    {
        //Null check/getter
        if(image == null)
        {
            image = this.gameObject.GetComponent<Image>();
        }
    }
    public void ToggleActiveSprite(bool isActive)
    {
        //If active, update the sprites acordingly
        if(isActive)
        {
            image.sprite = active;
        }
        else
        {
            image.sprite = inactive;
        }
    }
}
