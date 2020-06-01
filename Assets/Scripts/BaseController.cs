using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseController : MonoBehaviour
{
    public float health = 8;
    new Rigidbody2D rigidbody = new Rigidbody2D();


    void OnEnable()
    {

        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.simulated = true;
        rigidbody.velocity = Vector3.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerShot"))
        {
            other.gameObject.SetActive(false);  
        }

        if (other.gameObject.CompareTag("EnemyBomb"))
        {
            if (health <= 0)
                gameObject.SetActive(false);
            else
                health -= 1;
            
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag.Contains("Invader"))
        {
           gameObject.SetActive(false);
           other.gameObject.SetActive(false);
        }
    }
}
