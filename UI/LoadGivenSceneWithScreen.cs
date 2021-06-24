using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadGivenSceneWithScreen : LoadGivenScene
{
    public GameObject loadingScreen = null; //Loading screen
    private Slider progressSlider = null; //Slider that fills
    private TextMeshProUGUI progressSliderText = null; //The text of the current progress
    [Range(0.1f, 2f)] public float fillSpeed = 2f; //Speed at which the progress bar is filled

    private void Start()
    {
        GetLoadingUI();
        HasValidLoadingUI();
    }

    //Finds the relevant UI
    private void GetLoadingUI()
    {
        //Get the loading screen if missin
        if (loadingScreen == null)
        {
            //Get the loading screen
            loadingScreen = GameObject.FindWithTag("LoadingScreen");
        }

        //Get the text and slider
        if (loadingScreen != null)
        {
            progressSlider = loadingScreen.GetComponentInChildren<Slider>();
            progressSliderText = progressSlider.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    //Checks if the UI is valid
    private bool HasValidLoadingUI()
    {
        //Get the loading screen if missin
        if (loadingScreen == null || progressSlider == null || progressSliderText == null)
        {
            Debug.LogWarning("Loading UI is missing or could not be found! Are you using the correct load script?");
            return false;
        }
        else
        {
            return true;
        }
    }

    //Loads a scene while the current one "pauses"
    override public void LoadScene(string givenSceneName)
    {
        //If valid UI exists, being the courotine
        if(HasValidLoadingUI())
        {
            StartCoroutine(LoadAsynchronously(givenSceneName));
        }
        //Else change the screen without this UI
        else
        {
            base.LoadScene(givenSceneName);
        }
    }

    //Courotine to load
    private IEnumerator LoadAsynchronously(string givenSceneName)
    {
        //Create an operator that runs while the current scene is present
        AsyncOperation operation = SceneManager.LoadSceneAsync(givenSceneName);

        //Set the loading screen to active
        loadingScreen.SetActive(true);

        //While the operation (loading) is not complete, debug the loading information
        while (!operation.isDone)
        {
            //Divided by 0.9f as this is the upper limit of asyncronous loading for the assets of a scene
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            //Debug the current progress
            Debug.Log("Loading "+ givenSceneName+"... "+ "Progress: " + progress);

            //Move towards the targetted progress point
            progressSlider.value = Mathf.MoveTowards(progressSlider.value, progress, fillSpeed * Time.unscaledTime);

            //Update the text (in percentage format)
            progressSliderText.text = progress * 100f + "%";

            //Return for the frame
            yield return null;
        }
    }
}
