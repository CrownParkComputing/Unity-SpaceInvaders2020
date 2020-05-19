using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderGroupController : MonoBehaviour
{
    public float startWait = 1.0f;
    public float moveSpeed = 0.2f;
    public float acceleration = 0.2f;
    public int invadersPerRow = 12;
    public int invaderRows = 6;
    public int totalInvaders;

    private Vector3 movementLeft = new Vector3(1.0f, 0.0f, 0.0f);
    private Vector3 movementRight = new Vector3(-1.0f, 0.0f, 0.0f);

    private bool moveLeft = true;

    GameManager game;

    private readonly float maxX = 6.5f;
    private readonly float minX = -6.5f;


    void OnEnable()
    {
        GameManager.SpawnEnemyWaves+= SpawnEnemyWaves;
    }

    void OnDisable()
    {
        GameManager.SpawnEnemyWaves -= SpawnEnemyWaves;
    }

    void Start()
    {

        game = GameManager.SharedInstance;
        totalInvaders = invaderRows * invadersPerRow;
    }


    void Update()
    {

        if (game.GameOver) return;
        else
        {
            
            string invaderTag = "Invader";
            float maxSpeed = 5.0f;
            List<GameObject> invaderRow = ObjectPooler.SharedInstance.GetAllPooledObjectsByTag(invaderTag);

            if (invaderRow.Count < totalInvaders && moveSpeed <= maxSpeed)
            {
                  moveSpeed += acceleration ;
                totalInvaders = invaderRow.Count;

            }


            if (invaderRow.Count > 0)
            {
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

        for (int i = 1; i <= invaderRows; i++)
        {
            
            for (int x = 1; x <= invadersPerRow; x++)
            {

                Vector3 spawnPosition = new Vector2(nextSpawnX, nextRowY);
                Quaternion spawnRotation = Quaternion.Euler(nextSpawnX, nextRowY, 0);
                string invaderTag = "Invader" + i;
                GameObject invader = ObjectPooler.SharedInstance.GetPooledObject(invaderTag);
                if (invader != null)
                {
                    invader.transform.position = spawnPosition;
                    invader.transform.rotation = spawnRotation;
                    invader.SetActive(true);
                }
                nextSpawnX -= 0.75f;
            }

            nextRowY = nextRowY + 0.55f;
            nextSpawnX = -5.0f;



        }



    }

}
