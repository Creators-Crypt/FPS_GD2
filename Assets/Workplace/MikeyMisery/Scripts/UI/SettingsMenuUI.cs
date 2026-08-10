using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private GameObject interfacePanel;
    [SerializeField] private GameObject accessibilityPanel;

    private void Start()
    {
        ShowAudio(); // Show the audio panel by default
    }

    public void ShowAudio()
    {
        HideAll();
        audioPanel.SetActive(true);
    }

    public void ShowVideo()
    {
        HideAll();
        videoPanel.SetActive(true);
    }

    public void ShowInterface()
    {
        HideAll();
        interfacePanel.SetActive(true);
    }

    public void ShowAccessibility()
    {
        HideAll();
        accessibilityPanel.SetActive(true);
    }

    public void HideAll()
    {
        audioPanel.SetActive(false);
        videoPanel.SetActive(false);
        interfacePanel.SetActive(false);
        accessibilityPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
