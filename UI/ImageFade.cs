using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class ImageFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    private bool fadeAway;
    private float colorAlpha = 1;

    private void FixedUpdate()
    {
        //changed the color alpha between 1 or 0
        if (fadeAway)
        {
            colorAlpha =Mathf.MoveTowards(colorAlpha, 0, Time.unscaledDeltaTime);
        }
        else
        {
            colorAlpha = Mathf.MoveTowards(colorAlpha, 1, Time.unscaledDeltaTime);
        }

        //if it reaches 1 or 0 swap to moving toward the other number
        if (colorAlpha == 1)
        {
            fadeAway = true;
        }
        else if (colorAlpha == 0)
        {
            fadeAway = false;
        }

        //change the alpha
        fadeImage.color = new Color(1, 1, 1, colorAlpha);

    }
}
