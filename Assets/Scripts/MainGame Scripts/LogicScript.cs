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
    public BirdAgent birdAgent;
    public float countdownTime = 0.1f;
    private float countdownRemaining = 0.1f;
    private bool hasGameStarted = false;


    [ContextMenu("Increase score")]
    public void addScore(int scoreToAdd)
    {
        playerScore = playerScore + scoreToAdd;
        scoreText.text = playerScore.ToString();
        DidPassPipe = true;
    }

    void Start()
    {

        StartCountdown();
    }

    void Update()
    {
        if (!hasGameStarted)
        {
            Debug.Log("Countdown Remaining: " + countdownRemaining);
            countdownRemaining -= Time.deltaTime;

            if (countdownRemaining <= 0)
            {
                hasGameStarted = true;
                ResetFlags();
            }
        }
        else
        {
            // The game has started, so reset the flags
            ResetFlags();
        }
    }

    public void StartCountdown()
    {
        hasGameStarted = false;
        countdownRemaining = countdownTime;
        StartCoroutine(CountdownCoroutine());
    }
    private IEnumerator CountdownCoroutine()
    {
        Time.timeScale = 0; // Pause the game
        float startTime = Time.realtimeSinceStartup;
        float elapsedTime = 0f;

        while (elapsedTime < countdownTime)
        {
            elapsedTime = Time.realtimeSinceStartup - startTime;
            countdownRemaining = countdownTime - elapsedTime;
            Debug.Log("Countdown Remaining: " + countdownRemaining);
            yield return null;
        }

        hasGameStarted = true;
        StartGame(); // Resume the game
        ResetFlags();
    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        Time.timeScale = 1;
        StartCountdown();
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
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void StartGame()
    {
        birdAgent.enabled = true; // Enable the BirdAgent component to control the bird
        Time.timeScale = 1f; // Resume the game time scale
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
        birdAgent.isGameOver = true;
        Time.timeScale = 0;
        //restartGame();
    }


    // Call this method in every frame of your game loop
    public void ResetFlags()
    {
        DidPassPipe = false;
        DidHitPipe = false;
        DidMissPipe = false;
    }
}
