using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    private readonly StatePatternEnemy enemy;

    public AttackState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Attack();
        enemy.sprite.color = Color.red;
    }

    public void OnTrigger(Collider2D other)
    {

    }

    public void OnCollision(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy.transform.position = enemy.startPosition;
        }
    }

    public void ToIdleState()
    {
        enemy.currentState = enemy.idleState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToAttackState()
    {
        Debug.Log("Can't transition to same state");
    }

    void Attack()
    {       
        bool attack = true;
        Debug.Log(attack);
        if (attack)
        {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.attackPosition, enemy.attackSpeed * Time.deltaTime);

            if (enemy.transform.position == enemy.attackPosition)
            {
                attack = false;
            }
        }
        else
        {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.attackStartPosition, enemy.attackSpeed * Time.deltaTime);
        }
    }
}
