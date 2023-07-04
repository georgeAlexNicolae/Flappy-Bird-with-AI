using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public void StartGameHeuristicOnly()
    {
        PlayerPrefs.SetString("PlayerName", playerNameInput.text);
        PlayerPrefs.SetString("AgentBehaviorType", "HeuristicOnly");
        SceneManager.LoadScene("MainGame");
    }

    public void StartGameInferenceOnly()
    {
        PlayerPrefs.SetString("PlayerName", playerNameInput.text);
        PlayerPrefs.SetString("AgentBehaviorType", "InferenceOnly");
        SceneManager.LoadScene("MainGame");
    }

    public void ShowHighScores()
    {
        SceneManager.LoadScene("HighScores");
    }

}
