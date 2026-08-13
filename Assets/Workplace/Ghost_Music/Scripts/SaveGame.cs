using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public int playerScore = 100;

    // Save function
    public void SaveScore()
    {
        PlayerPrefs.SetInt("PlayerScore", playerScore);
        PlayerPrefs.Save(); // Ensure data is written to disk
        Debug.Log("Score saved!");
    }

    // Load function
    public void LoadScore()
    {
        if (PlayerPrefs.HasKey("PlayerScore"))
        {
            playerScore = PlayerPrefs.GetInt("PlayerScore");
            Debug.Log("Score loaded: " + playerScore);
        }
        else
        {
            Debug.Log("No save found!");
        }
    }
}