using UnityEngine;

public class LoadGivenScene : MonoBehaviour
{
    public string givenSceneName;

    //Load scene 
    public void LoadScene()
    {
        Application.LoadLevel(givenSceneName);
    }
}
