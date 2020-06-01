using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    private Rigidbody2D objectRigidbody;
    public float speed;

    private void Update()
    {
		objectRigidbody = transform.GetComponent<Rigidbody2D>();
		objectRigidbody.velocity = transform.up * -speed;
    }
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("PlayerShot"))
		{
			GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
			explosion.transform.position = transform.position;
			explosion.transform.rotation = transform.rotation;
			explosion.SetActive(true);
			gameObject.SetActive(false);
			other.gameObject.SetActive(false);
		}
	}
}
