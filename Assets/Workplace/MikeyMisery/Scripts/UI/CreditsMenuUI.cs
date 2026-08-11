using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenuUI : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
