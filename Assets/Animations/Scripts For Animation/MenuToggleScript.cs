using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggleScript : MonoBehaviour
{
public GameObject MainMenuScreen;

public void MenuAnimation()
{
    if (MainMenuScreen != null)
        {
                Animator animator = MainMenuScreen.GetComponent<Animator>();
                if(animator != null)
                {
                    bool isMenuOpen = animator.GetBool("MenuOpen");

                    animator.SetBool("MenuOpen", !isMenuOpen);

                    
                } else {Debug.Log("Animator is null");}

        } else {Debug.Log("Menu screen is null");}

}

}
