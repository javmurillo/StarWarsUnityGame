using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;         // For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;               // Condition for whether the player should jump.
	public bool jumpAttack = false;               // Condition for whether the player should jump.
	public bool attack = false;               // Condition for whether the player should jump.
	public bool attack2 = false;               // Condition for whether the player should jump.
	
	public float maxSpeed = 5f;             // The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;           // Array of clips for when the player jumps.
	public float jumpForce = 100f;         // Amount of force added when the player jumps.

	private Transform groundCheck;          // A position marking where to check if the player is grounded.
	private bool grounded = false;          // Whether or not the player is grounded.
	private Animator anim;                  // Reference to the player's animator component.
	
	private Rigidbody2D rigi;
	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float rocketspeed = 20f;				// The speed the rocket will fire at.

	private bool jumped = false;
	private float jumpTime, jumpDelay = .5f;
	
	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
		rigi = GetComponent<Rigidbody2D>();
	}
	
	
	void Update()
	{
		Debug.Log ("JA: " + jumpAttack);
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		
		// If the jump button is pressed and the player is grounded then the player should jump.
		if (Input.GetButtonDown ("Jump") && grounded) {
			jump = true;
		}
		if (Input.GetButtonDown ("Fire1") && grounded) {
			attack = true;

		}
		if (Input.GetButtonDown ("Fire1") && !grounded) {
			jumpAttack = true;
			
		}
		if (Input.GetButtonDown ("Fire2") && grounded) {
			attack2 = true;
			
		}
		
	}
	
	
	void FixedUpdate()
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		
		
		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));
		
		rigi.velocity = new Vector2(h * maxSpeed, rigi.velocity.y);
		
		
		/*
        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            // ... add a force to the player.
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            // ... set the player's velocity to the maxSpeed in the x axis.
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        */
		
		// If the input is moving the player right and the player is facing left...
		if (h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (h < 0 && facingRight)
			// ... flip the player.
			Flip();
		
		// If the player should jump...
		if (jump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");

			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
			jumpTime = jumpDelay;
			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
			jumped = true;

		}
		if (jumpAttack) {
			anim.SetTrigger("JumpAttack");
			jumpAttack = false;
		}
		
		jumpTime -= Time.deltaTime;

		if (jumpTime <= 0 && grounded && jumped) {
			anim.SetTrigger("Land");
			jumped = false;

		}

		if (attack) {
			anim.SetTrigger("Attack1");
			attack = false;
		}
		if (attack2) {
			anim.SetTrigger("Attack2");
			attack2 = false;
		}
		
		
	}
	
	
	void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	


	void Shoot() {
		// If the player is facing right...
		if(facingRight)
		{
			Vector3 rock = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
			// ... instantiate the rocket facing right and set it's velocity to the right. 
			Rigidbody2D bulletInstance = Instantiate(rocket, rock, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			bulletInstance.velocity = new Vector2(rocketspeed, 0);
		}
		else
		{
			Vector3 rock = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
			// Otherwise instantiate the rocket facing left and set it's velocity to the left.
			Rigidbody2D bulletInstance = Instantiate(rocket, rock, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
			bulletInstance.velocity = new Vector2(-rocketspeed, 0);
		}
	}
	
	void setFalse() {
		anim.SetBool ("Shoot", false);
	}
	
}
