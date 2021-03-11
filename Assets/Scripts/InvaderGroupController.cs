using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderGroupController : MonoBehaviour
{
    public float moveSpeedDefault = 0.5f;
    public float audioSpeedIncrease = 0.02f;
    public float moveAcceleration = 0.02f;
    public readonly int invadersPerRow = 12;
    public readonly int invaderRows = 6;
    public readonly float maxX = 6.5f;
    public readonly float minX = -6.5f;

    private Vector3 movementLeft = new Vector3(1.0f, 0.0f, 0.0f);
    private Vector3 movementRight = new Vector3(-1.0f, 0.0f, 0.0f);

    public delegate void NewWave();
    public static event NewWave StartNewWave;

    private bool moveLeft = true;
    private int totalInvaders;
    private float audwaitdef = 1.5f;
    public float audwait;
    public float moveSpeed;

    readonly string invaderTag = "Invader";
    private int whichSound = 1;
    public AudioClip sound1;
    public AudioClip sound2;

    private AudioSource invaderSound;

    GameManager game;

    void OnEnable()
    {
        GameManager.SpawnEnemyWaves += SpawnEnemyWaves;
        GameManager.RemoveEnemyWaves += RemoveEnemyWaves;

        invaderSound = gameObject.GetComponent<AudioSource>();
        audwait = audwaitdef - audioSpeedIncrease;
        moveSpeed = moveSpeedDefault;
    }

    void OnDisable()
    {
        GameManager.SpawnEnemyWaves -= SpawnEnemyWaves;
        GameManager.RemoveEnemyWaves -= RemoveEnemyWaves;
    }


    void Start()
    {
        game = GameManager.SharedInstance;
        
    }


    void Update()
    {

        if (game.GameOver)
            return;
        else
        {
           
            float maxSpeed = 10.0f;


            List<GameObject> invaderRow = ObjectPooler.SharedInstance.GetAllPooledObjectsByTag(invaderTag);

            if (invaderRow.Count <=0)
            {
                StartNewWave();
            }

            if (invaderRow.Count < totalInvaders && moveSpeed <= maxSpeed)
            { 
                moveSpeed += (moveAcceleration);
                totalInvaders = invaderRow.Count;
                audwait = audwaitdef - audioSpeedIncrease;
            }

            if (invaderRow.Count > 0)
            {    
                if (audwait <= 0f)
                {
                    
                    if (whichSound == 1)
                    {
                        if (!invaderSound.isPlaying)
                        {
                            invaderSound.clip = sound1;
                            invaderSound.Play();
                            whichSound += 1;
                        }

                    }
                    else
                    {
                        if (!invaderSound.isPlaying)
                        {
                            invaderSound.clip = sound2;
                            invaderSound.Play();
                            whichSound = 1;
                        }
                    }


                }
                audwait -= Time.deltaTime;


                foreach (GameObject item in invaderRow)
                {
                    if (item.transform.position.x >= maxX)
                    {
                        foreach (GameObject moveRowItem in invaderRow)
                        {
                            Vector3 newPosition = new Vector3(moveRowItem.transform.position.x - 0.25f, moveRowItem.transform.position.y - 0.25f, 0); 
                            moveRowItem.transform.position = newPosition;
                            moveLeft = false;
                        }

                    }

                    if (item.transform.position.x <= minX)
                    {
                        foreach (GameObject moveRowItem in invaderRow)
                        {
                            Vector3 newPosition = new Vector3(moveRowItem.transform.position.x + 0.25f, moveRowItem.transform.position.y - 0.25f, 0);
                            moveRowItem.transform.position = newPosition;
                            moveLeft = true;
                        }


                    }
                    
                    if (moveLeft == true)
                        item.transform.Translate(movementLeft * Time.deltaTime * moveSpeed);
                    else
                        item.transform.Translate(movementRight * Time.deltaTime * moveSpeed);


                }
            }
        }
    }

    void SpawnEnemyWaves()
    {
        float nextRowY = 8.0f;
        float nextSpawnX = -5.0f;
        moveSpeed = moveSpeedDefault;
        audwait = audwaitdef;
        totalInvaders = invaderRows * invadersPerRow;


        for (int i = 1; i <= invaderRows; i++)
        {

            for (int x = 1; x <= invadersPerRow; x++)
            {

                Vector3 spawnPosition = new Vector2(nextSpawnX, nextRowY);
                Quaternion spawnRotation = Quaternion.Euler(nextSpawnX, nextRowY, 0);
                string newInvader = invaderTag + i;
                GameObject invader = ObjectPooler.SharedInstance.GetPooledObject(newInvader);
                if (invader != null)
                {
                    invader.transform.position = spawnPosition;
                    invader.transform.rotation = spawnRotation;
                    invader.SetActive(true);
                }
                nextSpawnX -= 0.75f;
            }

            nextRowY += 0.55f;
            nextSpawnX = -5.0f;
        }
    }
    void RemoveEnemyWaves()
    {
        List<GameObject> invaderRow = ObjectPooler.SharedInstance.GetAllPooledObjectsByTag(invaderTag);

        foreach (GameObject myInvader in invaderRow)
        {
            myInvader.SetActive(false);
        }
    }


}
