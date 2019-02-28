using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeelingEnemy : MonoBehaviour
{
    public float attackSpeed;
    float attackTimer;
    public float gooDamage;
    public float collisionDamage;

    public GameObject GooPrefab;

    private void Start()
    {
        GooPrefab = Resources.Load("Goo") as GameObject;
    }
    private void Update()
    {
        Drip();
    }

    void Drip()
    {
        attackTimer += Time.deltaTime;

        if(attackTimer >= attackSpeed)
        {
            Instantiate(GooPrefab, new Vector2(transform.position.x, transform.position.y - .4f), Quaternion.identity);
            attackTimer = 0;
        }
    }
}
