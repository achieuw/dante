using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternEnemy : MonoBehaviour
{   
    public float speed = 2f;
    public float attackDistance;
    public float attackSpeed;

    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public CircleCollider2D col;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public Vector3 attackPosition;
    [HideInInspector] public Vector3 attackStartPosition;
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public NavMeshAgent navMeshAgent;


    void Awake ()
    {
        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
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
