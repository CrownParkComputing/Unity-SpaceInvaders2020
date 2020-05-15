using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private AudioSource bigBang;

    private void OnEnable()
    {
        bigBang = gameObject.GetComponent<AudioSource>();
        bigBang.Play();
    }
    void DisableGameObject()
    {
        gameObject.SetActive(false);
    }

}
