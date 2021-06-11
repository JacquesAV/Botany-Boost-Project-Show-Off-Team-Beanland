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

    private string chosenUsername;

    public void SubmitScore()
    {
        //checks if there are any non acceptable usernames
        if (usernameText.textComponent.text != " " && usernameText.textComponent.text != "Type your name here​" && string.IsNullOrEmpty(usernameText.textComponent.text))
        {
            //if not change to resolution screen and upload new high score
            gameObject.SetActive(false);
            resolutionScreen.SetActive(true);
            chosenUsername = usernameText.textComponent.text;
            UploadeToLeaderboard();
        }
        else
        {
            //show user that name is wrong and reset text
            usernameText.textComponent.color = Color.red;
            usernameText.text = "Type your name here​";
        }
    }
    public void ChangeColor()
    {
        usernameText.textComponent.color = Color.black;
    }

    private void UploadeToLeaderboard()
    {
        //this is where leaderboard calculation happens
    }
}
