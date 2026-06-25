using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Intro Cinematic");
    }

    public void QuitGame()
    {
        Application.Quit();
    }   
}

