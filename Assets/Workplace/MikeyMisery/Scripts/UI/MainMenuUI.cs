using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
}
