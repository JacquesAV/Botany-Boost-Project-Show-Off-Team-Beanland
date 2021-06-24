using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGivenScene : MonoBehaviour
{
    //Load scene 
    public virtual void LoadScene(string givenSceneName)
    {
        //Load the scene
        SceneManager.LoadScene(givenSceneName);
    }
}
