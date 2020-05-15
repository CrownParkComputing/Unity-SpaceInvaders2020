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
    public float startWait = 1.0f;

    public float waveInterval = 2.0f;
 
    public GameObject enemyType1;
    public GameObject enemyType2;
    public GameObject enemyType3;
    public GameObject enemySaucer;

    private GameObject RenderAlien;

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
    void SpawnEnemyWaves()
    {
        int invadersPerRow = 14;
        int invaderRows = 6;
        float nextRowY = -1f;
        float nextSpawnX = -5.0f;
        int invaderType = 0;

        for (int i = 0; i <= invaderRows; i++)
        {
            nextRowY += 1.0f;
            for (int x = 0; x <= invadersPerRow; x++)
            {
                Vector3 topLeft = new Vector3(nextSpawnX, nextRowY);

                Vector3 spawnPosition = new Vector3(topLeft.x, topLeft.y, (10+x));
                Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
                string invaderTag = "Invader" + invaderType.ToString();
                GameObject invader = ObjectPooler.SharedInstance.GetPooledObject(invaderTag);
                if (invader != null)
                {
                    invader.transform.position = spawnPosition;
                    invader.transform.rotation = spawnRotation;
                    invader.SetActive(true);
                }
                nextSpawnX += 0.75f;
            }

            nextSpawnX = -5.0f;

            if (invaderType == 3)
            { invaderType = 0; }
            else
            { invaderType += 1; }

        }
    }

    void OnCountdownFinished()
    {
        SpawnEnemyWaves();
        SetPageState(PageState.None);
        OnGameStarted();
        currentScore = 0;
        gameOver = false;
    }
    void OnPlayerScored()
    {
        currentScore += 50;
        scoreText.text = "Score : " + currentScore.ToString();
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
        SetPageState(PageState.Start);
        currentScore = 0;
        scoreText.text = "Score : " + currentScore.ToString();
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
