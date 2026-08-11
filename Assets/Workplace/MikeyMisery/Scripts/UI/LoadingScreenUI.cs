using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text LoadingProgress;

    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(1.5f); // Wait for a moment before starting the loading

        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            LoadingProgress.text = Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }
    }
}
