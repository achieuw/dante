using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodsplash : MonoBehaviour
{
    public ParticleSystem bloodSplash;

    private void Start()
    {
        bloodSplash = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SeelingGoo"))
        {
            bloodSplash.Play();
        }
    }
}
