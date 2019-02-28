
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbingEnemy : MonoBehaviour
{
    public int jumpsToBreak;
    private int jumpCount;
    public float dragForce;
    private bool grab;
    private bool activated = true;

    public PlayerControllerWithDashAndSprint player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (activated)
            {
                grab = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            grab = false;
        }
    }

    private void Update()
    {
        if (grab)
        {
            Grab();
        }
    }

    void Grab()
    {
        player.GetComponent<Rigidbody2D>().AddForce((transform.position - player.transform.position) * dragForce * 100);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpCount++;

            if (jumpCount >= jumpsToBreak)
            {
                grab = false;
                StartCoroutine(GrabCooldown());
                jumpCount = 0;
            }
        }
    }

    IEnumerator GrabCooldown()
    {
        activated = false;
        yield return new WaitForSeconds(1);
        activated = true;
    }
}
