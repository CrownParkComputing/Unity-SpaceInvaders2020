using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour
{
	void OnTriggerExit2D(Collider2D other)
	{
        if (other.gameObject.CompareTag("Boundary"))
        {
            if (gameObject.tag.Contains("Shot"))
             {gameObject.SetActive(false);}
            else
            { Destroy(gameObject);}
        }
    }
}
