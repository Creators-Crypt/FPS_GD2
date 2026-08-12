using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Continue() {

    }

    public void NewGame() {

    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); 
#endif
    }

    public void Resume() {
        GameManager.instance.Unpause();
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.Unpause();
    }
}