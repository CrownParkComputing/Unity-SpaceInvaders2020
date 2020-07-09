using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerController : MonoBehaviour
{
	public delegate void SaucerGotShot(string whatHit);
	public static event SaucerGotShot OnPlayerScores;
	
	public readonly float maxX = 6.5f;
    public readonly float minX = -6.5f;

	private Vector3 movementLeft = new Vector3(-1.0f, 0.0f, 0.0f);
	private Vector3 movementRight = new Vector3(1.0f, 0.0f, 0.0f);

	private Rigidbody2D objectRigidbody;
	public float moveSpeed;
	
	GameManager game;
	private AudioSource shootSound;


	private void OnEnable()
	{
		game = GameManager.SharedInstance;
		shootSound = gameObject.GetComponent<AudioSource>();
		shootSound.Play();
	}
	private void OnDisable()
	{
		game = null;
		shootSound.Stop();
		shootSound = null;
	}


	void Update()
	{
		if (game.GameOver)
			return;
		else
		{
			objectRigidbody = transform.GetComponent<Rigidbody2D>();
			if (objectRigidbody.tag == "SaucerLeft")
			{
				objectRigidbody.transform.Translate(movementRight * Time.deltaTime * moveSpeed);

				if (objectRigidbody.transform.position.x >= maxX)
				{
					gameObject.SetActive(false);
				}
			}
			else
			{

				objectRigidbody.transform.Translate(movementLeft * Time.deltaTime * moveSpeed);

				if (objectRigidbody.transform.position.x <= minX)
				{
					gameObject.SetActive(false);
				}
			}
		}








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

			OnPlayerScores("Saucer");
		}
	}

}
