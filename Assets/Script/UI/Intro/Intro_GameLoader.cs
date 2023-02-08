using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro_GameLoader : MonoBehaviour
{
    public void LoadGame()
    {
        LoadingSceneManager.LoadScene(1);
    }
}
