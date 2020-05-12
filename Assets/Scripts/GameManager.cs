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

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;

    public GameObject enemyType1;
    public GameObject enemyType2;


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

    private int currentScore = 0;
    bool gameOver = true;

    public bool GameOver { get { return gameOver; } }
    public int CurrentScore { get { return currentScore; } }

    void Awake()
    {
        SharedInstance = this;
    }

    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        SetPageState(PageState.Start);
    }

    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
    }


    void Start()
    {
        //StartCoroutine(SpawnEnemyWaves());
    }

    //IEnumerator SpawnEnemyWaves()
    //{
    //    while (true)
    //    {
    //        for (int i = 0; i < 5; i++)
    //        {
    //            Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight + 2, 0));
    //            Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight + 2, 0));
    //            Vector3 spawnPosition = new Vector3(Random.Range(topLeft.x, topRight.x), topLeft.y, 0);
    //            Quaternion spawnRotation = Quaternion.Euler(0, 0, 180);
    //            Instantiate(enemyType1, spawnPosition, spawnRotation);
    //        }
    //    }
    //}
    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        currentScore = 0;
        gameOver = false;
    }
    void OnPlayerScored()
    {
        currentScore++;
        scoreText.text = currentScore.ToString();
    }
    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("Highscore");
        if (currentScore > savedScore)
        {
            PlayerPrefs.SetInt("Highscore", currentScore);
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
        //scoreText.text = "Score : 0";
        SetPageState(PageState.Start);
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
