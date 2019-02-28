using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    public float movementSlowPercentage;
    public float jumpSlowPercentage;
    private PlayerControllerWithDashAndSprint player;
    private float playerSpeed;
    private float playerJumpForce;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerWithDashAndSprint>();
        playerSpeed = player.speed;
        playerJumpForce = player.jumpForce;
    }

    private void Update()
    {
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (movementSlowPercentage < 100)
            {
                player.speed = playerSpeed - (playerSpeed * movementSlowPercentage / 100);
                player.jumpForce = playerJumpForce - (playerJumpForce * jumpSlowPercentage / 100);
            }
            else
            {
                Debug.Log("Slow Percentage needs a value.");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.speed = playerSpeed;
            StartCoroutine(SlowJumpTimer()); //maybe set a slowgrounded bool in playercontroller to slow player jump
        }
    }

    IEnumerator SlowJumpTimer()
    {
        yield return new WaitForSeconds(player.jumpingTime);
        player.jumpForce = playerJumpForce;
    }
}