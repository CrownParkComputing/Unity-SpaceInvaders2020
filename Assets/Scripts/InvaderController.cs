using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InvaderController : MonoBehaviour
{
	public float minReloadTime = 5.0f;
	public float maxReloadTime = 10.0f;

	public delegate void InvaderGotShot(string whatHit);
	public static event InvaderGotShot OnPlayerScores;
	
	GameManager game;

    void Start()
	{
		game = GameManager.SharedInstance;
		StartCoroutine("Shoot");
	}

	void Update()
	{
		if (game.GameOver)
			StopCoroutine("Shoot");
	}

	IEnumerator Shoot()
	{
		yield return new WaitForSeconds(Random.Range(minReloadTime, maxReloadTime));
		while (!game.GameOver)
		{
			GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("EnemyBomb");
			if (bullet != null)
			{
				bullet.transform.position = transform.position;
				bullet.transform.rotation = transform.rotation;
				bullet.SetActive(true);
  			}
			yield return new WaitForSeconds(Random.Range(minReloadTime, maxReloadTime));
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

			OnPlayerScores("Invader");
		}
	}


}
