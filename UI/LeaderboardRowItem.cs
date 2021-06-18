using UnityEngine;
using TMPro;

public class LeaderboardRowItem : MonoBehaviour
{
    [SerializeField]
    //Text panels that are meant to visualize the leaderbaoard log
    private TextMeshProUGUI placementText = null, playerNameText = null, scoreText = null;

    //Default player name
    private string defaultPlayerName = "Blank";

    //Setter input for all text
    public void SetAllText(string givenPlacementText, string givenPlayerNameText, string givenScoreText)
    {
        SetPlacementText(givenPlacementText);
        SetPlayerNameText(givenPlayerNameText);
        SetScoreText(givenScoreText);
    }

    #region Text Setters
    public void SetPlacementText(string givenText)
    {
        //Set the text
        placementText.text = givenText;
    }
    public void SetPlayerNameText(string givenText)
    {
        //Check if the name is blank, if so then set to the default name
        if(givenText == null || givenText.Trim().Length == 0)
        {
            givenText = defaultPlayerName;
        }

        //Set the text
        playerNameText.text = givenText;
    }
    public void SetScoreText(string givenText)
    {
        //Set the text
        scoreText.text = givenText;
    }
    #endregion
}
