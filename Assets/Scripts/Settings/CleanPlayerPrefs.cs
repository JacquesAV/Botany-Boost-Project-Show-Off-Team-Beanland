using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanPlayerPrefs : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //If key pressed, clean player prefs
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Player prefs cleared!");
        }
    }
}
