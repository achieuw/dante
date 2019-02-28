using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellfire : MonoBehaviour
{
    public bool hellfire;
    public bool active;
    public float speed = 3f;
    public float playerPositionForHellfireStart;
	private float playerStartPosition;

	public LayerMask layer;

	private bool executed;

    BoxCollider2D col;
    SpriteRenderer sprite;

    public PlayerControllerWithDashAndSprint player;

    private void Start()
    {
		playerStartPosition = player.startPos.y;
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
		
    void Update ()
    {
		if (!executed) 
		{
			if(player.transform.position.y > playerStartPosition + playerPositionForHellfireStart) //if player reaches said height, hellfire starts
			{
				active = true;
				executed = true;
			}
		}

        Vector2 position = transform.position;

        if (active)
        {
			layer = LayerMask.NameToLayer ("Hellfire");
            position.y += speed * Time.deltaTime;
            sprite.color = Color.red;
        }
        else
        {
			layer = LayerMask.NameToLayer ("Ground");
            sprite.color = Color.cyan;
        }

        transform.position = position;
	}

    public void ToggleCollider()
    {
        col.enabled = !col.enabled;
    }
}
