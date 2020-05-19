using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public float health = 3;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerShot"))
        {
            other.gameObject.SetActive(false);  
        }

        if (other.gameObject.CompareTag("EnemyBomb"))
        {
            if (health <= 0)
                Destroy(gameObject);
            else
                health -= 1;
            
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag.Contains("Invader"))
        {
           Destroy(gameObject);
           other.gameObject.SetActive(false);
        }
    }
}
