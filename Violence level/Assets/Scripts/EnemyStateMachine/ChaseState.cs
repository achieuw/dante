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
        enemy.sprite.color = Color.red;
        Chase();       
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

    public void Chase()
    {
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.chaseTarget.position, enemy.speed * Time.deltaTime);
        //play chase animation
    }
}
