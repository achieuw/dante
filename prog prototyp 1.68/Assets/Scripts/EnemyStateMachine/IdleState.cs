using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private readonly StatePatternEnemy enemy;

    public IdleState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {        
        Idle();
        enemy.sprite.color = Color.black;
    }

    public void OnTrigger(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToChaseState();
        }
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
        Debug.Log("Can't transition to same state");
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }
    void Idle()
    {
        if(enemy.transform.position != enemy.startPosition)
        {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.startPosition, enemy.speed * Time.deltaTime);
        }
        //play idle animation
    }
}
