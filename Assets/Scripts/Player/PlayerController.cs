using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    [Header("Attributes")]

	[Tooltip("Controls the movement speed of the player. Higher values make the player move faster.")]
    [SerializeField] private float moveSpeed;

    [Tooltip("Speed multiplier applied when the character is running.")]
	[SerializeField] private float runSpeed;

    [Tooltip("Determines the current speed of the character, dynamically adjusted based on movement state.")]
    private float activeSpeed;

	[Tooltip("Controls the physics behavior of a 2D object, enabling realistic motion, collisions, and forces.")]
    private Rigidbody2D rb;

    [Tooltip("Determines the upward force applied when the character jumps.")]
    [SerializeField] private float jumpForce;

    [Tooltip("Indicates whether the player has the ability to perform a second jump while airborne.")]
    private bool canDoubleJump;

    [Header("State")]

    [Tooltip("Indicates whether the player is currently in contact with the ground.")]
    private bool isGrounded;

	[Tooltip("Reference point from which to check if the player is in contact with the ground.")]
	[SerializeField] private Transform groundCheckPoint;

	[Tooltip("Radius around the groundCheckPoint within which the game checks for ground.")]
	[SerializeField] private float groundCheckRadius;

	[Tooltip("Radius around the leftPlatformsCheckPoint or rightPlatformCheckpoints within which the game checks for platforms.")]
	[SerializeField] private float platformCheckRadius;

	[Tooltip("Reference point from which to check if the player is in contact with a left platform.")]
	[SerializeField] private Transform leftPlatformsCheckPoint;

	[Tooltip("Reference point from which to check if the player is in contact with a right platform.")]
	[SerializeField] private Transform rightPlatformsCheckPoint;

	[Tooltip("Layer(s) considered as 'ground'. Used to detect if the player is on the ground.")]
	[SerializeField] private LayerMask whatIsGround;

	[Tooltip("Layer(s) considered as 'plaatform'. Used to detect if the player is on the side of some platform to double jump.")]
	[SerializeField] private LayerMask whatIsPlatform;

	// Start is called before the first frame update
	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		CheckGrounded();

		Move();
		
		HandleJumping();
	}

    void Update()
    {
		HandleInputs();
	}

    void Move()
    {
		// Sets the active speed based on whether the Left Shift key is pressed: 'runSpeed' if pressed, otherwise 'moveSpeed'.
		activeSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

		// Set the horizontal velocity of the object based on player input, while preserving the current vertical velocity.
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * activeSpeed, rb.velocity.y);
    }

    void HandleJumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded == true)
            {
                Jump();
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
				if (CanDoubleJumpOffPlatform())
				{
					Jump();
					canDoubleJump = false;
				}
				else
				{
					canDoubleJump = false;
				}
			}
			else
			{
				canDoubleJump = false;
			}
		}
    }

	void CheckGrounded()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
	}

	void HandleInputs()
	{
		activeSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
	}

	void Jump()
    {
		// Set the vertical velocity of the object to the jump force, while preserving the current horizontal velocity.
		rb.velocity = new Vector2(rb.velocity.x, jumpForce);
	}

	bool CanDoubleJumpOffPlatform()
	{
		return Physics2D.OverlapCircle(leftPlatformsCheckPoint.position, platformCheckRadius, whatIsPlatform) ||
			   Physics2D.OverlapCircle(rightPlatformsCheckPoint.position, platformCheckRadius, whatIsPlatform);
	}
}
