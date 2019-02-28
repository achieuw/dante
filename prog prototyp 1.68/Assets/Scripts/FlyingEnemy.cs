using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed;
    public float collisionDamage;
    public float knockbackForce;
    public float alertDistance;
    public float attackDistance;
    public float dashCooldown;
    public float dashForce;

    float radius;
    Vector3 startPos;
    bool chase;
    bool keepChasing;
    float dashTimer;

    PlayerManager player;
    [HideInInspector]
    public Rigidbody2D rb;
    SpriteRenderer sprite;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        startPos = transform.position;
    }

    void Update ()
    {
        Chase();
        CheckDistance();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void CheckDistance()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= alertDistance)
        {
            chase = true;
        }
    }

    void Chase()
    {
        if (chase)
        {
            if(Vector2.Distance(transform.position, player.transform.position) <= alertDistance * 1.1f)
            {
                keepChasing = true;
            }
            else
            {
                chase = false;
                keepChasing = false;
            }
        }

        if (keepChasing)
        {
            sprite.color = Color.red;

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            if (dashTimer <= Time.time)
            {
                StartCoroutine(Dash());
                dashTimer = Time.time + dashCooldown;
            }
        }
        else
        {
            sprite.color = Color.black;
            transform.position = Vector2.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
        }
    }

    IEnumerator Dash()
    {
        if(transform.position.x < player.transform.position.x)
        {
            rb.velocity = new Vector2(Random.Range(-dashForce / 2, dashForce), Random.Range(-dashForce, dashForce));
        }
        else
        {
            rb.velocity = new Vector2(Random.Range(-dashForce, dashForce), Random.Range(-dashForce, dashForce));
        }
        
        yield return new WaitForSeconds(1);
        rb.velocity = new Vector2(0, 0);
    }
}
