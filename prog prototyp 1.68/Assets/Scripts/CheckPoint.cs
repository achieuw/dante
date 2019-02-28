using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public SpriteRenderer flagSprite;
    private bool activated;
    [HideInInspector]
    public float deltaCheckpointPosition;

    public CheckPointManager cpManager;
    public PlayerManager player;

    private void Start()
    {
        cpManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CheckPointManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (flagSprite)
        {
            if (!activated)
            {
                flagSprite.color = Color.blue;
            }
            else
            {
                flagSprite.color = Color.green;
            }
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            activated = true;
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            cpManager.lastCheckpointPosition = position;
            player.startPos = transform.position;
        }
    }
}
