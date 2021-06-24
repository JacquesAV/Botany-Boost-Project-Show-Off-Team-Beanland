using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLeaderboard : MonoBehaviour
{
    public void RequestLeaderboardUpdate()
    {
        //Update the current leaderboard
        PlayfabManager.singleton.GetLeaderboard();
    }
}
