using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public int playerLives = 3;
    public float moveSpeed = 1.0f;
    public float ShotTurretReloadTime = 2.0f;
    public Vector3 startPos;

    GameManager game;

    public GameObject playerMainGun;

    private List<GameObject> activePlayerTurrets;
    private int gameLives;
    new Rigidbody2D rigidbody = new Rigidbody2D();

    public delegate void PlayerDied();
    public static event PlayerDied OnPlayerDied;

    private AudioSource shootSound;

    private float maxX = 6.5f;
    private float minX = -6.5f;
    private float maxY = 6.0f;
    private float minY = -6.0f;
    
    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;

        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.simulated = false;
        rigidbody.velocity = Vector3.zero;

        shootSound = gameObject.GetComponent<AudioSource>();
        game = GameManager.SharedInstance;
        activePlayerTurrets = new List<GameObject>
        {
              playerMainGun
        };
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameStarted()
    {
        gameLives = playerLives;
        transform.localPosition = startPos;
        rigidbody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        rigidbody.simulated = false;
        transform.localPosition = startPos;
        gameLives = playerLives;

    }
    // Update is called once per frame
    void Update()
    {
        if (game.GameOver)
            return;
        if (Input.GetKeyDown("space"))
        {
            Shoot();
        }

        float xDir = Input.GetAxis("Horizontal");
        float yDir = rigidbody.position.y;
        rigidbody.velocity = new Vector2(xDir * moveSpeed, yDir * moveSpeed);
        rigidbody.position = new Vector2(
                Mathf.Clamp(rigidbody.position.x, minX, maxX),
                Mathf.Clamp(rigidbody.position.y, minY, maxY)
            );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyBomb"))
        {
            other.gameObject.SetActive(false);
            GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");

            for (int i = 0; i < 8; i++)
            {
                Vector3 randomOffset = new Vector3(transform.position.x + Random.Range(-0.6f, 0.6f), transform.position.y + Random.Range(-0.6f, 0.6f), 0.0f);
                explosion.transform.position = randomOffset;
                explosion.SetActive(true);
            }

            if (gameLives > 1)
            {
                gameLives -= 1;
            }
            else
            {
                OnPlayerDied();
            }
        }

        if (other.gameObject.tag.Contains("Invader"))
        {
            other.gameObject.SetActive(false);
            GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");

            for (int i = 0; i < 8; i++)
            {
                Vector3 randomOffset = new Vector3(transform.position.x + Random.Range(-0.6f, 0.6f), transform.position.y + Random.Range(-0.6f, 0.6f), 0.0f);
                explosion.transform.position = randomOffset;
                explosion.SetActive(true);
            }
            OnPlayerDied();
        }

    }

    void Shoot()
    {

        foreach (GameObject turret in activePlayerTurrets)
        {
            {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("PlayerShot");
                if (bullet != null)
                {
                    bullet.transform.position = turret.transform.position;
                    bullet.transform.rotation = turret.transform.rotation;
                    bullet.SetActive(true);
                    shootSound.Play();
                }
            }
        }

    }

}
