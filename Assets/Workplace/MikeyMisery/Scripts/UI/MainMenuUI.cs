using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public void NewGame()
    {
        SceneManager.LoadScene("LoadingScreen");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("CreditsMenu");
    }
}
