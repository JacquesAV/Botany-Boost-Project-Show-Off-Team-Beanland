using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UploadScoreToLeaderboard : MonoBehaviour
{
    [SerializeField] private GameObject confirmationScreen;
    [SerializeField] private GameObject resolutionScreen;
    //[SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TMP_InputField usernameText;
    [SerializeField] private GameOverDisplay gameOverDisplay; 

    private string chosenUsername="[blank]";

    public void SubmitScore()
    {
        if (usernameText != null)
        {
            //checks if there are any non acceptable usernames
            if (usernameText.textComponent.text != " " && usernameText.textComponent.text != "Type your name here​" && !string.IsNullOrEmpty(usernameText.textComponent.text))
            {
                //if not, change to resolution screen and upload new high score
                chosenUsername = usernameText.textComponent.text;
                UploadToLeaderboard();
                gameObject.SetActive(false);
                if (resolutionScreen != null)
                {
                    resolutionScreen.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("The resolutionScreen component was not set, please do so");
                }
            }
            else
            {
                //show user that name is wrong and reset text
                usernameText.textComponent.color = Color.red;
                usernameText.text = "Type your name here​";
            }
        }
        else
        {
            Debug.LogWarning("The usernameText component was not set, please do so");
        }
    }
    public void ChangeColor()
    {
        usernameText.textComponent.color = Color.black;
    }

    private void UploadToLeaderboard()
    {
        if (gameOverDisplay != null)
        {
            PlayfabManager.singleton.SetUsername(chosenUsername);
            PlayfabManager.singleton.SendLeaderboard(gameOverDisplay.GetScore());
        }
        else
        {
            Debug.LogWarning("The gameOverDisplay component was not set in the leaderboard screen, please do so");
        }
    }
}
