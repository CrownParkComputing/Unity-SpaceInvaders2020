using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager SharedInstance;

    public delegate void GameDelegate();
    public static event GameDelegate SpawnEnemyWaves;
    public static event GameDelegate RemoveEnemyWaves;
    public static event GameDelegate RemoveEnemyBullets;
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static event GameDelegate SpawnBases;
    public static event GameDelegate RemoveBases;
    public static event GameDelegate SpawnSaucers;
    public static event GameDelegate StartInvaderBombs;


    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public GameObject gameEnvironment;
    public Text currentScore;
    public Text highScore;

    private int score;
    private int high;

    private bool gameOver = true;

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
        SaucerController.OnPlayerScores += OnPlayerScored;
        PlayerController.OnPlayerDied += OnPlayerDied;
        InvaderGroupController.StartNewWave += StartNewWave;

        SetPageState(PageState.Start);
        high = PlayerPrefs.GetInt("Highscore");
        highScore.text = high.ToString("D6");
    }

    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        InvaderController.OnPlayerScores -= OnPlayerScored;
        PlayerController.OnPlayerDied -= OnPlayerDied;
        SaucerController.OnPlayerScores -= OnPlayerScored;
        InvaderGroupController.StartNewWave += StartNewWave;
    }


    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        SpawnBases();
        SpawnEnemyWaves();
        SpawnSaucers();
        OnGameStarted();
        score = 0;
        gameOver = false;
    }
    void OnPlayerScored(string whatHit)
    {
        switch (whatHit)
        {
            case "Saucer":
                score += 500;

                break;

            case "Invader":
                score += 200;

                break;
            case "Bomb":
                score += 50;

                break;
        }

        currentScore.text = score.ToString("D6");

        if (Score > high)
        { highScore.text = currentScore.text; }
    }
    void OnPlayerDied()
    {
        gameOver = true;

        if (score > high)
        {
            high = score;
            PlayerPrefs.SetInt("Highscore", high);
            highScore.text = high.ToString("D6");
        }
        OnGameOverConfirmed();
        RemoveEnemyBullets();
        RemoveEnemyWaves();
        RemoveBases();
        SetPageState(PageState.GameOver);
    }

    void StartNewWave()
    {
        RemoveEnemyBullets();
        RemoveEnemyWaves();
        SpawnEnemyWaves();
        StartInvaderBombs();
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                gameEnvironment.SetActive(true);

                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                gameEnvironment.SetActive(false);

                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                gameEnvironment.SetActive(false);

                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                gameEnvironment.SetActive(false);

                break;

        }
    }

    public void ConfirmGameOver()
    // activate when replay hit
    {
        SetPageState(PageState.Start);
        score = 0;
        currentScore.text = score.ToString("D6");
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
