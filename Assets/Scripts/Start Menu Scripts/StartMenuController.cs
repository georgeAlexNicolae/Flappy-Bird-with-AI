using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public void StartGame()
    {
        PlayerPrefs.SetString("PlayerName", playerNameInput.text);
        SceneManager.LoadScene("MainGame");
    }

    public void ShowHighScores()
    {
        SceneManager.LoadScene("HighScores");
    }

}
