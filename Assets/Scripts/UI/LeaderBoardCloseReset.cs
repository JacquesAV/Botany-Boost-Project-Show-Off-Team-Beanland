using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardCloseReset : MonoBehaviour
{

    Animator m_Animator;

    void Start(){

        m_Animator = gameObject.GetComponent<Animator>();
    }

    public void ResetLBClose()
    {
       

        m_Animator.ResetTrigger("LeaderBoardClose");

    }

}
