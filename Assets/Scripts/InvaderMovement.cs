using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvaderMovement : MonoBehaviour
{
    private Transform invaderHolder;
    public float speed;
    float fireRateTimer;

    public delegate void InvaderDelegate();
    public static event InvaderDelegate HitRockBottom;


    public GameObject shot;
    public float fireRate;

    GameManager game;

    // Start is called before the first frame update

    void Start()
    {
        game = GameManager.SharedInstance;
        invaderHolder = GetComponent<Transform>();
    }


    void Update()
    {
        if (game.GameOver) return;

        invaderHolder.position += Vector3.right * speed;
        foreach (Transform invader in invaderHolder)
        {

            if (invader.position.x < -10.5 || invader.position.x > 10.5)
            {
                speed = -speed;
                invaderHolder.position += Vector3.down * 0.5f;
                return;
            }
            
            fireRateTimer += Time.deltaTime;

            if (fireRateTimer > fireRate)
            { 
                Instantiate(shot, invader.position, invader.rotation);
                fireRateTimer = 0;

            }

            if (invader.position.y <= -4)
            {
                HitRockBottom();

            }
            
        }

        if (invaderHolder.childCount == 0)
        {
            HitRockBottom();
            //Next wave event sent to gamemamager
        }
    }

}
