using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDudeShot : MonoBehaviour
{
    public Vector2 velocity;
    Vector2 moveDirection;
    Rigidbody2D rb;
    PlayerManager player;

    public GameObject flamePrefab;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        moveDirection = player.transform.position - transform.position;

        rb.velocity = new Vector2(moveDirection.normalized.x * velocity.x * Mathf.Clamp(Vector2.Distance(player.transform.position, transform.position), 2, 5),
        velocity.y * Mathf.Clamp(Vector2.Distance(player.transform.position, transform.position), 8, 12));

        flamePrefab = Resources.Load("Flame") as GameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        if (rb.velocity.y < 0)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                Instantiate(flamePrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }       
    }
}
