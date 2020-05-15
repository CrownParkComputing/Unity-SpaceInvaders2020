using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderMovementController : MonoBehaviour
{
	public float moveSpeed = 0.2f;
	private Rigidbody2D objectRigidbody;

	private float maxX = 6.0f;
	private float minX = -6.0f;
	private float maxY = 6.0f;
	private float minY = 6.0f;

	void OnEnable()
	{
		objectRigidbody = transform.GetComponent<Rigidbody2D>();
		objectRigidbody.velocity = transform.right * moveSpeed;
	}

	void Update()
	{
		maxX = 6.0f;
		minX = -6.0f;
		maxY = 6.0f;
		minY = -6.0f;

		objectRigidbody = transform.GetComponent<Rigidbody2D>();
		Vector3 newPosition = new Vector3();

		if (objectRigidbody.position.x >= maxX)
		{ 
			newPosition = new Vector3(objectRigidbody.position.x - 0.1f, objectRigidbody.position.y - 0.5f, 0);
			transform.position = newPosition;
			moveSpeed = -moveSpeed;
		}
		if (objectRigidbody.position.x <= minX)
		{ 
			newPosition = new Vector3(objectRigidbody.position.x + 0.1f, objectRigidbody.position.y - 0.5f, 0);
			transform.position = newPosition;
			moveSpeed = -moveSpeed;
		}

		
		objectRigidbody.velocity = transform.right * moveSpeed;

	}

}
