using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float ShotTurretReloadTime = 2.0f;
    
    public GameObject playerShot;
    public GameObject explosion;
    public GameObject playerMainGun;

    private List<GameObject> activePlayerTurrets;

    public Vector3 startPos;

    private BoxCollider2D playerCollider;
    new Rigidbody2D rigidbody = new Rigidbody2D();


    private GameObject leftBoundary;
    private GameObject rightBoundary;
    private GameObject topBoundary;
    private GameObject bottomBoundary;

    private AudioSource shootSound;
    private float nextFire;

    void Start()
    {
        leftBoundary = GameManager.SharedInstance.leftBoundary;
        rightBoundary = GameManager.SharedInstance.rightBoundary;
        topBoundary = GameManager.SharedInstance.topBoundary;
        bottomBoundary = GameManager.SharedInstance.bottomBoundary;
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
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Shoot();
        }

        //if (game.GameOver) return;
        float xDir = Input.GetAxis("Horizontal");
        float yDir = rigidbody.position.y;
        rigidbody.velocity = new Vector2(xDir * moveSpeed, yDir * moveSpeed);
        rigidbody.position = new Vector2(
                Mathf.Clamp(rigidbody.position.x, leftBoundary.transform.position.x, rightBoundary.transform.position.x),
                Mathf.Clamp(rigidbody.position.y, bottomBoundary.transform.position.y, topBoundary.transform.position.y)
            );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Base"))
        {
            if (gameObject.tag == "PlayerShot")
            { gameObject.SetActive(false);}
            else
            {Destroy(gameObject);}
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
                }
            }
        }
        //shootSound.Play();
    }

}
