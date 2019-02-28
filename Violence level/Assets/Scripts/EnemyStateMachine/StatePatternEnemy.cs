using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternEnemy : MonoBehaviour
{   
    public float speed = 2f;

    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public Vector2 startPosition;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public NavMeshAgent navMeshAgent;

	void Awake ()
    {
        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
	}

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        currentState = idleState;
        chaseTarget = GameObject.FindWithTag("Player").transform; //target the player
        startPosition = transform.position;
    }

    void Update ()
    {
        currentState.UpdateState();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTrigger(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollision(collision);
    }
    void Destroy(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
