using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {

    public float bounceForce = 8f;

    Rigidbody2D playerRigidbody;

    private void Start()
    {
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(playerRigidbody.velocity.y <= 0)
            {
                playerRigidbody.velocity = Vector3.up * bounceForce;
            }          
        }
    }
}
