using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderBulletController : MonoBehaviour
{
    private Rigidbody2D objectRigidbody;
    public float speed;
	GameManager game;

	private void Start()
	{
		game = GameManager.SharedInstance;
	}
	void Update()
    {
		if (game.GameOver)
			return;
		else
		{
			objectRigidbody = transform.GetComponent<Rigidbody2D>();
			objectRigidbody.velocity = transform.up * -speed;
		}
    }
	void OnTriggerEnter2D(Collider2D other)
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
