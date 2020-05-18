using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager SharedInstance;

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static event GameDelegate SpawnEnemyWaves;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;
   
    int score = 0;
    bool gameOver = true;
    public bool GameOver { get { return gameOver; } }
    public int Score { get { return score; } }

    public GameObject leftBoundary;                   //
    public GameObject rightBoundary;                  // References to the screen bounds: Used to ensure the player
    public GameObject topBoundary;                    // is not able to leave the screen.
    public GameObject bottomBoundary;                 //

    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown,
    }

    enum InvaderType
    {
        Alien1,
        Alien2,
        Alien3,
        AlienSaucer,
    }

    void Awake()
    {
        SharedInstance = this;
    }

    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        InvaderController.OnPlayerScores += OnPlayerScored;
        PlayerController.OnPlayerDied += OnPlayerDied;
        SetPageState(PageState.Start);
    }

    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        InvaderController.OnPlayerScores -= OnPlayerScored;
        PlayerController.OnPlayerDied -= OnPlayerDied;
    }


    void OnCountdownFinished()
    {
        SpawnEnemyWaves();
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }
    void OnPlayerScored()
    {
        score += 50;
        scoreText.text = "Score : " + score.ToString();
    }
    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("Highscore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        SetPageState(PageState.GameOver);

    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);

                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);

                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);

                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);

                break;

        }
    }

    public void ConfirmGameOver()
    // activate when replay hit
    {
        OnGameOverConfirmed(); //event
        SetPageState(PageState.Start);
        score = 0;
        scoreText.text = "Score : " + score.ToString();
    }

    public void StartGame()
    //actgivated when play button hit
    {
        SetPageState(PageState.Countdown);
    }

    public void ExitGame()
    //actgivated when exit button hit
    {
        Application.Quit();
    }

}
