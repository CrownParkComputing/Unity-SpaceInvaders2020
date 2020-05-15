using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderController : MonoBehaviour
{

    public float minReloadTime = 5.0f;
    public float maxReloadTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Shoot");
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds((Random.Range(minReloadTime, maxReloadTime)));
        while (true)
        {
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("InvaderShot");
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.SetActive(true);
            }
            yield return new WaitForSeconds((Random.Range(minReloadTime, maxReloadTime)));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerShot"))
        {
            GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
            if (explosion != null)
            {
                explosion.transform.position = transform.position;
                explosion.transform.rotation = transform.rotation;
                explosion.SetActive(true);
            }
            other.gameObject.SetActive(false);
        }   


    }
}
