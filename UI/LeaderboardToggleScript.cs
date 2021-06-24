using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardToggleScript : MonoBehaviour
{
public GameObject LeaderBoardScreen;

public void LeaderBoardAnimation()
{
    if (LeaderBoardScreen != null)
        {
                Animator animator = LeaderBoardScreen.GetComponent<Animator>();
                if(animator != null)
                {
                    bool isLeaderBoardOpen = animator.GetBool("LeaderBoardOpen");

                    animator.SetBool("LeaderBoardOpen", !isLeaderBoardOpen);

                    
                } else {Debug.Log("AnimatorLB is null");}

        } else {Debug.Log("Menu screen is null");}

}

}