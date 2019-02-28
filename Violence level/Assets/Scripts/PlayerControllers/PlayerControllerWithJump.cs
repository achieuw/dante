using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerWithJump : MonoBehaviour
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

    public bool allowCrossScreen = false;
    public CameraController cam;
    private float radius;

    [Header("Jump")]
    public float fallMultiplier;
    public float lowJumpMultiplier;




    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        radius = GetComponent<BoxCollider2D>().size.x / 2; //ändra boxcollider till den aktiva collidern på spelarobjektet  
    }

    private void Update()
    {

        Movement();
        HandleBounds();
    }
    void Movement()
    {
		//Vector3 position = transform.position;       

		float moveX = Input.GetAxis ("Horizontal");
		float moveXraw = Input.GetAxisRaw ("Horizontal");

		if (acceleration)
		{
			transform.position += Vector3.right * moveX * speed * Time.deltaTime;
		}
		else 
		{
			transform.position += Vector3.right * moveXraw * speed * Time.deltaTime;
		}

        if (Input.GetButtonDown("Jump"))
        {

            if (!IsGrounded())
            {
                return;
            }
            else
            {
                rb.velocity += new Vector2(0, jumpForce);
            }
        }

        if (allowFly)
        {
            if (Input.GetButton("Jump"))
            {
                rb.isKinematic = true; //ta bort fysiken från rigidbody
                transform.position += Vector3.up * speed * flyMultiplier * Time.deltaTime;             
            }   
        }
        if (!allowFly)
        {
            rb.isKinematic = false;   
        }

        if (rb.velocity.y < 0)
            rb.velocity += new Vector2(0, Physics2D.gravity.y * fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += new Vector2(0, Physics2D.gravity.y * lowJumpMultiplier - 1) * Time.deltaTime;

        //slutgiltlig transformation
        //transform.position = position;
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

        if (collision.gameObject.CompareTag("Hellfire"))
        {
            //GAME OVER
            SceneManager.LoadScene("start");
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

    void HandleBounds()
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

        if (transform.position.y < cam.boundary)
        {
            //GAME OVER
            //SceneManager.LoadScene("main");
        }

        transform.localPosition = position;
    }
}
