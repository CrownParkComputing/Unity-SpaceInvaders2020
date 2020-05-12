using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D objectRigidbody;
    public float speed;
    void OnEnable()
    {
        objectRigidbody = transform.GetComponent<Rigidbody2D>();
        objectRigidbody.velocity = transform.up * speed;
    }

}
