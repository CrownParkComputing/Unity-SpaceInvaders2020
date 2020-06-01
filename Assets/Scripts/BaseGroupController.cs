using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGroupController : MonoBehaviour
{
    private readonly int totalBases = 5;

    readonly string baseTag = "Base";

    GameManager game;

    void OnEnable()
    {
        GameManager.SpawnBases += SpawnBases;
        GameManager.RemoveBases += RemoveBases;
    }



    void OnDisable()
    {
        GameManager.SpawnBases -= SpawnBases;
        GameManager.RemoveBases -= RemoveBases;
    }

    void Start()
    {
        game = GameManager.SharedInstance;
    }

    void SpawnBases()
    {

        float nextRowY = -4.5f;
        float nextSpawnX = -4.0f;

        for (int i = 1; i <= totalBases; i++)
        {
            Vector3 spawnPosition = new Vector2(nextSpawnX, nextRowY);
            Quaternion spawnRotation = Quaternion.Euler(nextSpawnX, nextRowY, 0);
            GameObject playerBase = ObjectPooler.SharedInstance.GetPooledObject(baseTag);
            if (playerBase != null)
            {
                playerBase.transform.position = spawnPosition;
                playerBase.transform.rotation = spawnRotation;
                playerBase.transform.localScale = new Vector3(2.0f, 2.0f, 0);
                playerBase.SetActive(true);
            }
            nextSpawnX += 2.0f;
        }

    }
    void RemoveBases()
    {
        List<GameObject> allBases = ObjectPooler.SharedInstance.GetAllPooledObjectsByTag(baseTag);

        foreach (GameObject myBase in allBases)
        {
            myBase.SetActive(false);
        }

    }
}
