using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGivenScene : MonoBehaviour
{
    public string sceneName;

    //Load scene 
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
