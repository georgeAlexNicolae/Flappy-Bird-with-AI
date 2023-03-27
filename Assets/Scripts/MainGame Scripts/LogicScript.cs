using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
   public int playerScore;
    public Text scoreText;
    public GameObject gameOverScreen;

    [ContextMenu("Increase score")]
    public void addScore(int scoreToAdd)
    { 
        playerScore = playerScore + scoreToAdd;
        scoreText.text = playerScore.ToString();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void StartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    private void SaveHighScore(int newScore)
    {
        for (int i = 0; i < 10; i++)
        {
            int savedScore = PlayerPrefs.GetInt("HighScore" + i, 0);
            if (newScore > savedScore)
            {
                PlayerPrefs.SetInt("HighScore" + i, newScore);
                newScore = savedScore;
            }
        }
        PlayerPrefs.Save();
    }

    private void SaveHighScoreWithName(int newScore)
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Joe");
        for (int i = 0; i < 10; i++)
        {
            int savedScore = PlayerPrefs.GetInt("HighScore" + i, 0);
            string savedName = PlayerPrefs.GetString("HighScoreName" + i, "Joe");
            if (newScore > savedScore)
            {
                PlayerPrefs.SetInt("HighScore" + i, newScore);
                PlayerPrefs.SetString("HighScoreName" + i, playerName);
                newScore = savedScore;
                playerName = savedName;
            }
        }
        PlayerPrefs.Save();
    }


    public void gameOver()
    {
        gameOverScreen.SetActive(true);
        SaveHighScoreWithName(playerScore);
    }

}
