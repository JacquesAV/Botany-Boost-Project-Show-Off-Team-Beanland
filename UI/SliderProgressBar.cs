using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderProgressBar : MonoBehaviour
{
    //Reference to the slider object
    private Slider sliderComponent;

    //Represents the targetted progress value
    private float targetProgress = 0;

    //Speed at which the bar fills up
    [Range(0.1f,2f)] public float fillSpeed = 1f;

    //Bar background and fill image 
    [SerializeField] private Image background, barFill;

    private void Awake()
    {
        //Get the slider component
        sliderComponent = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        //Move towards the targetted slider point
        sliderComponent.value = Mathf.MoveTowards(sliderComponent.value, targetProgress, fillSpeed * Time.deltaTime);
    }

    //Set the target progress
    public void SetTargetProgress(float givenTarget)
    {
        targetProgress = givenTarget;
    }

    //Set the target progress
    public void SetBarColor(Color givenColor)
    {
        barFill.color = givenColor;
    }

    //Set the target progress
    public void SetBackgroundColor(Color givenColor)
    {
        background.color = givenColor;
    }
}
