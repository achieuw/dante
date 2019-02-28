using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 7f;
    public float jumpForce = 8f;
	public bool acceleration = false;
    [Space(10)]
    public bool allowFly;
    public float flyMultiplier = 1f;
    [Space(10)]
    public bool ignorePlatformCollision;
    public LayerMask groundLayer;
    private bool move_left, move_right, move_up, jump;

    public bool allowCrossScreen = false;
    public CameraController cam;
    private float radius;
  
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        radius = GetComponent<BoxCollider2D>().size.x / 2; //ändra boxcollider till den aktiva collidern på spelarobjektet          
    }

    private void Update()
    {
        PlayerInput();
        Movement();
        CrossScreen();
    }
    void Movement()
    {
		Vector3 position = transform.position;       

		float moveX = Input.GetAxis ("Horizontal");
		float moveXraw = Input.GetAxisRaw ("Horizontal");

		if (acceleration)
		{
			position.x += moveX * speed * Time.deltaTime;
		}
		else 
		{
			position.x += moveXraw * speed * Time.deltaTime;
		}

        if (jump)
        {
            if (!IsGrounded())
            {
                return;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        if (allowFly)
        {
            if (move_up)
            {
                rb.isKinematic = true; //ta bort fysiken från rigidbody
                position.y += speed * flyMultiplier * Time.deltaTime;             
            }   
        }
        if (!allowFly)
        {
            rb.isKinematic = false;   
        }
        //slutgiltlig transformation
        transform.localPosition = position;
    }

    void PlayerInput()
    {
        bool input_up = Input.GetKey(KeyCode.W);
        bool input_jump = Input.GetKeyDown(KeyCode.Space);

        move_up = input_up;
        jump = input_jump;
    }

    bool IsGrounded()
    {
        Vector3 position = transform.position;
        Vector3 direction1 = new Vector3(0.5f, -1f, 0f);
        Vector3 direction2 = new Vector3(-0.5f, -1f, 0f);
        float distance = .8f;

        Debug.DrawRay(position, direction1, Color.green);
        Debug.DrawRay(position, direction2, Color.green);
        //raycast under spelaren som kollar om spelaren är grounded eller inte grounded
        if (Physics2D.Raycast(position, direction1, distance, groundLayer) || Physics2D.Raycast(position, direction2, distance, groundLayer))
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignorePlatformCollision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<BoxCollider2D>(), true);   
            }
        } 
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ignorePlatformCollision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<BoxCollider2D>(), false);
            }
        }
    }

    void CrossScreen()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //Skickar spelaren till andra sidan skärmen om han hamnar utanför
        if (allowCrossScreen)
        {
            if (position.x > cam.width / 2)
            {
                position.x = -cam.width / 2;
            }
            if (position.x < -cam.width / 2)
            {
                position.x = cam.width / 2;
            }
        }
        else
        {
            if (position.x >= cam.width / 2 - radius)
            {
                position.x = cam.width / 2 - radius;
            }
            if (position.x <= -cam.width / 2 + radius)
            {
                position.x = -cam.width / 2 + radius;
            }
        } 

        transform.localPosition = position;
    }
}
