using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDudeEnemy : MonoBehaviour
{
    public float attackSpeed;
    float attackTimer;
    public float attackRadius;
    public float shotDamage;
    public float collisionDamage;
    public float flameDamage;
    public float knockbackForce;
    public float flameKnockbackForce;

    public bool shoot;

    public GameObject ShotPrefab;
    PlayerManager player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }
    private void Update()
    {
        CheckRadius();
        Shoot();
    }

    void Shoot()
    {
        if (shoot)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackSpeed)
            {
                Instantiate(ShotPrefab, transform.position, Quaternion.identity);
                attackTimer = 0;
            }
        }       
    }

    void CheckRadius()
    {
        if(Vector2.Distance(player.transform.position, transform.position) <= attackRadius)
        {
            shoot = true;
        }
        else
        {
            shoot = false;
        }
    }
}
