using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HighScoresController : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public Button backButton;

    private List<KeyValuePair<string, int>> highScores = new List<KeyValuePair<string, int>>();


    private void Start()
    {
        backButton.onClick.AddListener(GoBack);
        LoadHighScores();
        DisplayHighScores();
    }

    private void LoadHighScores()
    {
        for (int i = 0; i < 10; i++)
        {
            int score = PlayerPrefs.GetInt("HighScore" + i, 0);
            string name = PlayerPrefs.GetString("HighScoreName" + i, "Joe");
            highScores.Add(new KeyValuePair<string, int>(name, score));
        }
    }


    private void DisplayHighScores()
    {
        highScoreText.text = "High Scores\n";
        for (int i = 0; i < highScores.Count; i++)
        {
            highScoreText.text += (i + 1) + ". " + highScores[i].Key + " - " + highScores[i].Value + "\n";
        }
    }


    private void GoBack()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
