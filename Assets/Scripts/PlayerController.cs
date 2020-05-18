using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public int NumberofLives = 3;
    public float moveSpeed = 1.0f;
    public float ShotTurretReloadTime = 2.0f;

    public GameObject playerMainGun;

    private List<GameObject> activePlayerTurrets;

    public Vector3 startPos;

    private BoxCollider2D playerCollider;
    new Rigidbody2D rigidbody = new Rigidbody2D();

    public delegate void PlayerDied();
    public static event PlayerDied OnPlayerDied;

    private AudioSource shootSound;

    private float maxX = 6.5f;
    private float minX = -6.5f;
    private float maxY = 6.0f;
    private float minY = -6.0f;

    void Start()
    {
        activePlayerTurrets = new List<GameObject>();
        activePlayerTurrets.Add(playerMainGun);
        playerCollider = gameObject.GetComponent<BoxCollider2D>();

        shootSound = gameObject.GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.simulated = false;
    }
    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
            gameObject.SetActive(false);
            GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
            explosion.SetActive(true);
            other.gameObject.SetActive(false);

            if (NumberofLives > 1)
            {
                gameObject.SetActive(true);
                NumberofLives -= 1;

            }
            else
            {
                OnPlayerDied();
            }
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
