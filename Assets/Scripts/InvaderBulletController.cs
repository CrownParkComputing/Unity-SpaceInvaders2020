using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderBulletController : MonoBehaviour
{
    public static InvaderBulletController Instance;
    private Transform bullet;
    public float speed;

    GameManager game;
    // Start is called before the first frame update

    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        game = GameManager.SharedInstance;
    }

    void Start()
    {
        bullet = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (game.GameOver) return;
        bullet.position += Vector3.up * -speed;

        if (bullet.position.y <= -10)
        { Destroy(bullet.gameObject); }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (game.GameOver) return;
        if (other.CompareTag("Player"))
        {
            foreach (Transform x in bullet)
            {
                Destroy(x.gameObject);
            }

        }
        else if (other.CompareTag("Base"))
        {
            GameObject playerBase = other.gameObject;
            BaseHealth baseHealth = playerBase.GetComponent<BaseHealth>();
            baseHealth.health -= 1;
            Destroy(gameObject);
        }
    }
}
