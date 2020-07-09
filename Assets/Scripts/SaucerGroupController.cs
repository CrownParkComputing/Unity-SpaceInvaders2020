using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerGroupController : MonoBehaviour
{
    public float minSaucerTime = 5.0f;
    public float maxSaucerTime = 15.0f;
	public Vector3 StartPosRight;
	public Vector3 StartPosLeft;
	GameManager game;

	private void OnEnable()
	{
		GameManager.SpawnSaucers += SpawnSaucers;
		game = GameManager.SharedInstance;
	}


	void OnDisable()
	{
		GameManager.SpawnSaucers -= SpawnSaucers;
	}


	void SpawnSaucers()
    {
		StartCoroutine("SpawnSaucer");
	}

	IEnumerator SpawnSaucer()
	{
		yield return new WaitForSeconds(Random.Range(minSaucerTime, maxSaucerTime));
		while (!game.GameOver)
		{
			GameObject saucer = ObjectPooler.SharedInstance.GetPooledObject("Saucer");
			int whichSaucer = Random.Range(1, 3);
			if (saucer != null)
			{
				if (whichSaucer == 1)
                { 
					saucer.transform.position = StartPosRight;
					saucer.tag  =  "SaucerRight";
				}
				else
				{ 
					saucer.transform.position = StartPosLeft;
					saucer.tag = "SaucerLeft";
				}
				
				saucer.transform.rotation = transform.rotation;
				saucer.SetActive(true);
			}
			yield return new WaitForSeconds(Random.Range(minSaucerTime, maxSaucerTime));
		}
	}
	
}