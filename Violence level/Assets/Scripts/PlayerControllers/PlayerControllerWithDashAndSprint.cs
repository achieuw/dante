using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerWithDashAndSprint : MonoBehaviour
{
	private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprajt;

    public float doubleJumpForce;
    public float speed = 7f;
	public float movement2;
	public float jumpForce = 8f;
	public bool acceleration = false;
	public int extraJumps;
	float extraJumpPoints;
	[Space(10)]
	public float dashSpeed;
	public float dashTime;
	private bool dash;
	public float sprintSpeed;
	private bool sprint;
    public bool move = true;
	[Space(10)]
	public bool allowFly;
	public float flyMultiplier = 1f;
	[Space(10)]
	public LayerMask groundLayer;
	[HideInInspector]
	public Vector2 startPos;
	public bool allowCrossScreen = false;
	public CameraController cam;
	private float radius;

	public float jumpingTime;
	public float jumpingTimeCounter;

	public bool grounded;
	public bool stoppedJumping;

	public Transform groundCheck;
	public float groundCheckRadius;

	[Header("Jump")]
	public float fallMultiplier;
	public float lowJumpMultiplier;

    public AudioSource footStepSound;
    public AudioSource jumpNLand;
    public AudioClip[] daligIde;
    public AudioClip[] testaIgen;

    private void Start()
	{
		startPos = transform.position;
		rb = GetComponent<Rigidbody2D>();
        sprajt = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        radius = GetComponent<BoxCollider2D>().size.x / 2; //ändra boxcollider till den aktiva collidern på spelarobjektet  
		extraJumpPoints = extraJumps;
		jumpingTimeCounter = jumpingTime;
	}

	private void Update()
	{
        if (move)
        {
            Movement();
        }	
		Animation();
		HandleBounds(); 
    }

    void Movement()
    {
        if (!sprint)
            movement2 = Input.GetAxisRaw("Horizontal") * speed;
        else
            movement2 = Input.GetAxisRaw("Horizontal") * sprintSpeed;

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (rb.velocity.y >= 0 && extraJumpPoints < 0)
        {
            grounded = false;
        }

        if (grounded && rb.velocity.y == 0)
        {
            //the jumpcounter is whatever we set jumptime to in the editor.
            jumpingTimeCounter = jumpingTime;
            extraJumpPoints = extraJumps;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!grounded)
            {
                StartCoroutine(Dash());
                sprint = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprint = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprint = true;
        }

        

        stepSound();
        flipper();
    }
			
    void flipper()
    {
        
        if (Mathf.Sign(movement2) > 0)
        {
            sprajt.flipX = false;
        }
        else
        {
            sprajt.flipX = true;
        }
    }
    void stepSound()
    {
        if (!(movement2 == 0))
        {
            if (grounded && movement2 != 0 && !footStepSound.isPlaying)
            {
                footStepSound.clip = (daligIde[Random.Range(0, 5)]);
                footStepSound.PlayOneShot((footStepSound.clip = (daligIde[Random.Range(0, 5)])), 1);
            }
        }
    }

	void Animation()
	{
		animator.SetFloat("Speed", Mathf.Abs(movement2));
		animator.SetFloat("Jumping", rb.velocity.y);	
	}

    /*void Movement()
	{
		//Vector3 position = transform.position;       

		float moveX = Input.GetAxis ("Horizontal");
		float moveXraw = Input.GetAxisRaw ("Horizontal");

		if(!sprint)
		{
			if (acceleration)
			{
				transform.position += Vector3.right * moveX * speed * Time.deltaTime;
			}
			else 
			{
				transform.position += Vector3.right * moveXraw * speed * Time.deltaTime;
			}
		}
		else 
		{ //sprint			
			if (acceleration)
			{
				transform.position += Vector3.right * moveX * sprintSpeed * Time.deltaTime;
			}
			else 
			{
				transform.position += Vector3.right * moveXraw * sprintSpeed * Time.deltaTime;
			}
		}
			
		if (Input.GetButtonDown("Jump"))
		{

			if (!IsGrounded())
			{
				return;
			}
			else
			{
				if(rb.velocity.y <= 0)	
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
		}
		//slutgiltlig transformation
		//transform.position = position;
	}

	bool IsGrounded()
	{
        Vector2 position = transform.position;
        Vector2 direction1 = new Vector2(0.5f, -1);
		Vector2 direction2 = new Vector2(-0.5f, -1);
        Vector2 direction3 = Vector2.down;
        Vector2 position4 = new Vector2(position.x + radius, position.y);
        Vector2 direction4 = Vector2.down;
        Vector2 position5 = new Vector2(position.x - radius, position.y);
        Vector2 direction5 = Vector2.down;

        float distance = 1f;

		Debug.DrawRay(position, direction1, Color.green);
		Debug.DrawRay(position, direction2, Color.green);
		Debug.DrawRay(position, direction3, Color.green);
        Debug.DrawRay(position4, direction4, Color.green);
        Debug.DrawRay(position5, direction5, Color.green);
        //raycast under spelaren som kollar om spelaren är grounded eller inte grounded
        if (Physics2D.Raycast(position, direction1, distance, groundLayer) || 
            Physics2D.Raycast(position, direction2, distance, groundLayer) || 
            Physics2D.Raycast(position, direction3, distance, groundLayer) ||
            Physics2D.Raycast(position4, direction4, distance, groundLayer)||
            Physics2D.Raycast(position5, direction5, distance, groundLayer))
		{
			return true;
		}

		return false;
	}
*/
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
		}

		transform.localPosition = position;
	}

	void LateUpdate()
	{	
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			if(grounded)
			{
				rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
				stoppedJumping = false;
			}
		}


		    if(Input.GetKey(KeyCode.UpArrow) && !stoppedJumping)
		    {
			    //and your counter hasn't reached zero...
			    if(jumpingTimeCounter > 0)
			    {
				    //keep jumping!
				    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
				    jumpingTimeCounter -= Time.deltaTime;

                    if (jumpingTimeCounter <= 0)
                    {
                        stoppedJumping = true;
                    }
			    }
		    }

		if(Input.GetKeyUp(KeyCode.UpArrow))
		{
			//stop jumping and set your counter to zero.  The timer will reset once we touch the ground again in the update function
			jumpingTimeCounter = 0;
			stoppedJumping = true;			
		}

		if(Input.GetKeyDown(KeyCode.UpArrow) && stoppedJumping == true && extraJumpPoints > 0)
		{
				rb.velocity = new Vector2 (rb.velocity.x, doubleJumpForce);
				extraJumpPoints--;
		}

		Vector2 velocity = rb.velocity;

		velocity.x = movement2;

		rb.velocity = velocity;


        if (rb.velocity.y < 0)
            rb.velocity += new Vector2(0, Physics2D.gravity.y * fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += new Vector2(0, Physics2D.gravity.y * lowJumpMultiplier - 1) * Time.deltaTime;
    }
	IEnumerator Dash()	
	{	
		float x = speed;
		speed = dashSpeed;
		yield return new WaitForSeconds(dashTime);
		speed = x;
	}
}
