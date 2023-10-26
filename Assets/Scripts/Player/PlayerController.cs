using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
	[Header("Attributes")]

	// Player's normal movement speed.
	[Tooltip("Controls the movement speed of the player.")]
	[SerializeField] private float moveSpeed;

	// Speed multiplier for when the player is running.
	[Tooltip("Speed multiplier for running.")]
	[SerializeField] private float runSpeed;

	// The speed currently being used, based on player state (walking or running).
	private float activeSpeed;

	// Reference to the Rigidbody2D component for physics interactions.
	[Tooltip("Rigidbody2D component for 2D physics.")]
	private Rigidbody2D rb;

	// Reference to the Animator component to handle character animations.
	[Tooltip("Animator component to handle character animations.")]
	private Animator anim;

	// Force applied upward when the player jumps.
	[Tooltip("Upward force when jumping.")]
	[SerializeField] private float jumpForce;

	[Header("State")]

	// Is the player currently standing on the ground?
	[Tooltip("Is the player touching the ground?")]
	private bool isGrounded;

	// Is the player currently able to perform a double jump?
	[Tooltip("Is the player able to double jump?")]
	private bool canDoubleJump;

	// Position from which a check is made to see if the player is on the ground.
	[Tooltip("Check point for ground contact.")]
	[SerializeField] private Transform groundCheckPoint;

	// The distance around groundCheckPoint where the check for ground is made.
	[Tooltip("Radius for checking ground contact.")]
	[SerializeField] private float groundCheckRadius;

	// The distance around platform check points to determine if player is near a platform.
	[Tooltip("Radius for checking platform contact.")]
	[SerializeField] private float platformCheckRadius;

	// Position from which a check is made to see if the player is near a platform on the left.
	[Tooltip("Check point for left-side platform contact.")]
	[SerializeField] private Transform leftPlatformsCheckPoint;

	// Position from which a check is made to see if the player is near a platform on the right.
	[Tooltip("Check point for right-side platform contact.")]
	[SerializeField] private Transform rightPlatformsCheckPoint;

	// Specifies which layers are considered 'ground'.
	[Tooltip("Layers considered as ground.")]
	[SerializeField] private LayerMask whatIsGround;

	// Specifies which layers are considered 'platform'.
	[Tooltip("Layers considered as platform.")]
	[SerializeField] private LayerMask whatIsPlatform;

	// Initialization of component references.
	void Start()
	{
		InitRequiredComponents();
	}

	// Regularly update player states and handle inputs.
	void Update()
	{
		CheckGrounded();
		CheckIsOnPlatform();
		Move();
		HandleJumping();
		HandleAnimations();
		HandleFlippingSide();
	}

	// Fetch the necessary components from the game object.
	void InitRequiredComponents()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	// Handle player's horizontal movement.
	void Move()
	{
		// Set movement speed based on whether player is walking or running.
		activeSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

		// Apply movement.
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * activeSpeed, rb.velocity.y);
	}

	// Determine jump behavior based on player's current state.
	void HandleJumping()
	{
		if (Input.GetButtonDown("Jump"))
		{
			if (isGrounded)
			{
				Jump();
				canDoubleJump = true;
				anim.SetBool("isDoubleJumping", false);
			}
			else if (canDoubleJump && ASidePlatform())
			{
				Jump();
				anim.SetBool("isDoubleJumping", true);
				canDoubleJump = false;
			}
		}
	}

	// Check if player is grounded.
	void CheckGrounded()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
	}

	// Check if player is on or near a platform.
	void CheckIsOnPlatform()
	{
		isGrounded = isGrounded || Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsPlatform);
	}

	// Flip player sprite based on movement direction.
	void HandleFlippingSide()
	{
		if (rb.velocity.x > 0)
			transform.localScale = Vector3.one;
		else if (rb.velocity.x < 0)
			transform.localScale = new Vector3(-1f, 1f, 1f);
	}

	// Adjust animations based on player's state and movement.
	void HandleAnimations()
	{
		anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
		anim.SetBool("isGrounded", isGrounded);
		anim.SetFloat("ySpeed", rb.velocity.y);
	}

	// Apply vertical force to make the player jump.
	void Jump()
	{
		rb.velocity = new Vector2(rb.velocity.x, jumpForce);
	}

	// Check if player is near a platform, either on the left or the right.
	bool ASidePlatform()
	{
		return Physics2D.OverlapCircle(leftPlatformsCheckPoint.position, platformCheckRadius, whatIsPlatform) ||
			   Physics2D.OverlapCircle(rightPlatformsCheckPoint.position, platformCheckRadius, whatIsPlatform);
	}
}
