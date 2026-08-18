using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Loading")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingProgress;

    private bool settingsOpenFromPause = false;

    public void ShowMainMenu()
    {
        HideAllPanels();
        mainMenuPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        settingsOpenFromPause = false;
        HideAllPanels();
        settingsPanel.SetActive(true);
    }

    public void ShowSettingsFromPause()
    {
        settingsOpenFromPause = true;
        HideAllPanels();
        settingsPanel.SetActive(true);
    }

    public void BackFromSettings()
    {
        if (settingsOpenFromPause)
        {
            HideAllPanels();

        }
        else
        {
            ShowMainMenu();
        }
    }

    public void ShowCredits()
    {
        HideAllPanels();
        creditsPanel.SetActive(true);
    }

    public void ShowLoading()
    {
        HideAllPanels();
        loadingPanel.SetActive(true);
    }

    public void HideAllPanels()
    {
        mainMenuPanel.SetActive(false);
        loadingPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void StartGame()
    {
        ShowLoading();
        StartCoroutine(LoadGameAsync());
    }

    private IEnumerator LoadGameAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("main");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.98f);

            loadingBar.value = progress;
            loadingProgress.text = Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }
    }
}
