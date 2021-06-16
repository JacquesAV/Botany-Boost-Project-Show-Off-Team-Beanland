using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{

    public static PlayfabManager singleton;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this; //Sets the active manager to this instance of it
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Login();
    }
    #region OnEvents
    private void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create");
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account");
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard send");
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
        }
    }

    private void OnLowestScoreGet(GetLeaderboardResult result)
    {
        int lowestScore = 0;
        //Debug.Log(result.Leaderboard[0].StatValue);
        foreach (var item in result.Leaderboard)
        {
            lowestScore=item.StatValue;
        }
        EventManager.currentManager.AddEvent(new LowestLeaderboardScore(lowestScore,result.Leaderboard.Count));
    }
    private void OnUsernameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Username updated to: " + result.DisplayName);
    }

    private void OnCharacterGrant(GrantCharacterToUserResult result)
    {
        Debug.Log("Character: " + result.CharacterId+" was created");
    }
    #endregion
    private void Login()
    {
        var request = new LoginWithCustomIDRequest {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }   

    public void SendLeaderboard(int score)
    {
        //Creates request for updating leaderboard
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            { 
                new StatisticUpdate
                { 
                        StatisticName="High Score",Value = score
                }
            }
        };
        //updates the leaderboard, if it fails, will send out error
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);

    }

    public void GetLeaderboard()
    {
        //creates request for getting the leaderboard
        var request = new GetLeaderboardRequest
        {
            StatisticName = "High Score",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        //gets the leaderboard, if it fails, will send out error
        PlayFabClientAPI.GetLeaderboard( request,OnLeaderboardGet, OnError);
    }

    public void GetLowestLeaderboardScore()
    {
        //creates request for getting the leaderboard
        var request = new GetLeaderboardRequest
        {
            StatisticName = "High Score",
            StartPosition = 0,
            MaxResultsCount = 9
        };
        //gets the leaderboard, if it fails, will send out error
        PlayFabClientAPI.GetLeaderboard(request, OnLowestScoreGet, OnError);
    }

    public void SetUsername(string givenID)
    {
        //Creates request for updating display name
        var nameRequest = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = givenID
        };
        //updates the display name, if it fails, will send out error
        PlayFabClientAPI.UpdateUserTitleDisplayName(nameRequest, OnUsernameUpdate, OnError);
    }

    public void CreateCharacter(string characterName)
    {
        //Creates request for updating display name
        var characterRequest = new GrantCharacterToUserRequest
        {
            CharacterName = characterName
        };
        //updates the display name, if it fails, will send out error
        PlayFabClientAPI.GrantCharacterToUser(characterRequest, OnCharacterGrant, OnError);
    }
}
