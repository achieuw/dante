using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState {

    private readonly StatePatternEnemy enemy;

    public ChaseState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        enemy.sprite.color = Color.yellow;
        if (Vector2.Distance(enemy.transform.position, enemy.chaseTarget.transform.position) < enemy.col.radius * 1.2f)
        {
            Chase();
        }
        else
        {
            ToIdleState();
        }
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

    public void ToChaseState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToIdleState()
    {
        enemy.currentState = enemy.idleState;
    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }
    public void Chase()
    {
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.chaseTarget.position, Random.Range(enemy.speed / 2, enemy.speed * 2) * Time.deltaTime);

        enemy.attackPosition = enemy.chaseTarget.transform.position;
        enemy.attackStartPosition = enemy.transform.position;

        if (Vector2.Distance(enemy.transform.position, enemy.chaseTarget.transform.position) <= enemy.attackDistance)
        {
            ToAttackState();
        }
        //play chase animation
    }
}
