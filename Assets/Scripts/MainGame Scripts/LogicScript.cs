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
    public bool DidPassPipe = false;
    public bool DidHitPipe = false;
    public bool DidMissPipe = false;
    public DataLogger logger;

    [ContextMenu("Increase score")]
    public void addScore(int scoreToAdd)
    {
        playerScore = playerScore + scoreToAdd;
        scoreText.text = playerScore.ToString();
        DidPassPipe = true;
    }
    void Update()
    {
        ResetFlags();
    }

    void Start()
    {
        logger = GameObject.FindObjectOfType<DataLogger>();
    }

    public void hitPipe()
    {
        DidHitPipe = true;
    }

    public void missedPipe()
    {
        DidMissPipe = true;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        logger.OnGameRestart();
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
        restartGame();
    }


    // Call this method in every frame of your game loop
    public void ResetFlags()
    {
        DidPassPipe = false;
        DidHitPipe = false;
        DidMissPipe = false;
    }
}
