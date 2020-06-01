using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletGroupController : MonoBehaviour
{
    readonly string bulletTag = "Bomb";
    GameManager game;

    void OnEnable()
    {
        GameManager.RemoveEnemyBullets += RemoveEnemyBullets;
    }

    void OnDisable()
    {
        GameManager.RemoveEnemyBullets -= RemoveEnemyBullets;
    }

    void RemoveEnemyBullets()
    {
        List<GameObject> allBullets = ObjectPooler.SharedInstance.GetAllPooledObjectsByTag(bulletTag);

        foreach (GameObject myBullet in allBullets)
        {
            myBullet.SetActive(false);
        }
    }
}
