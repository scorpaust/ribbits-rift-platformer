using UnityEngine;

/// <summary>
/// Controls the player character's movement, including walking, running, jumping, and handling interactions
/// with the environment, such as knockback from hazards.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance { get; private set; }

	[Header("Movement Attributes")]
	[Tooltip("Controls the movement speed of the player.")]
	[SerializeField] private float moveSpeed;
	[Tooltip("Speed multiplier for running.")]
	[SerializeField] private float runSpeed;

	[Header("Jumping Attributes")]
	[Tooltip("Upward force when jumping.")]
	[SerializeField] private float jumpForce;

	[Header("Ground Detection")]
	[Tooltip("Check point for ground contact.")]
	[SerializeField] private Transform groundCheckPoint;
	[Tooltip("Radius for checking ground contact.")]
	[SerializeField] private float groundCheckRadius;
	[Tooltip("Layers considered as ground.")]
	[SerializeField] private LayerMask whatIsGround;

	[Header("Platform Detection")]
	[Tooltip("Check point for left side platform contact.")]
	[SerializeField] private Transform leftPlatformsCheckPoint;
	[Tooltip("Check point for right side platform contact.")]
	[SerializeField] private Transform rightPlatformsCheckPoint;
	[Tooltip("Radius for checking platform contact.")]
	[SerializeField] private float platformCheckRadius;
	[Tooltip("Layers considered as platform.")]
	[SerializeField] private LayerMask whatIsPlatform;

	[Header("Knockback Settings")]
	[Tooltip("Duration of knockback effect.")]
	[SerializeField] private float knockbackLength = 1f;
	[Tooltip("Speed of knockback effect.")]
	[SerializeField] private float knockbackSpeed;

	private float activeSpeed;
	private bool isGrounded;
	private bool canDoubleJump;
	private float knockbackCounter;
	private bool isActive = true;

	private Rigidbody2D rb;
	private Animator anim;

	public bool IsActive
	{
		get => isActive;
		set => isActive = value;
	}


	/// <summary>
	/// Initializes the singleton instance, ensuring only one instance exists.
	/// </summary>
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			rb = GetComponent<Rigidbody2D>();
			anim = GetComponent<Animator>();
		}
	}

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	private void Start()
	{
		// Initialize the Rigidbody2D and Animator components.
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		activeSpeed = moveSpeed;
	}

	/// <summary>
	/// Handles player input for movement and jumping, and applies knockback when necessary.
	/// </summary>
	private void Update()
	{
		if (Time.timeScale <= 0f) return;

		if (!IsActive || knockbackCounter > 0)
		{
			HandleKnockback();
			return;
		}

		CheckGrounded();
		Move();
		HandleJumping();
		HandleFlippingSide();
		HandleAnimations();
	}

	/// <summary>
	/// Moves the player character based on user input and active speed.
	/// </summary>
	private void Move()
	{
		// Determine the active speed based on whether the player is walking or running.
		activeSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

		// Apply the determined active speed to the player's horizontal velocity.
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * activeSpeed, rb.velocity.y);
	}

	/// <summary>
	/// Handles the player's jumping behavior when the jump button is pressed.
	/// </summary>
	private void HandleJumping()
	{
		// If jumping from the ground, enable double jump. Otherwise, disable it.
		canDoubleJump = !isGrounded && IsBesidePlatform();

		// Check for jump input and if the player is either grounded or can double jump.
		if (Input.GetButtonDown("Jump"))
		{
			if (isGrounded && !canDoubleJump)
			{
				Jump(false);
			}
			else if (canDoubleJump) 
			{
				Jump(true);
			}
		}
	}

	/// <summary>
	/// Checks if the player is grounded by performing a Physics2D overlap circle check at the groundCheckPoint.
	/// </summary>
	private void CheckGrounded()
	{
		// Update the isGrounded state based on whether the groundCheckPoint overlaps with the ground or platform layers.
		isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround) || 
			Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsPlatform);
	}

	/// <summary>
	/// Executes the jump by applying an upward force to the Rigidbody2D component and manages the double jump state.
	/// </summary>
	public void Jump(bool doubleJump)
	{
		// Apply the jump force to the player's vertical velocity.
		rb.velocity = new Vector2(rb.velocity.x, jumpForce);
		
		// Inform the Animator component about the jump.
		anim.SetBool("isDoubleJumping", doubleJump);
		// anim.SetTrigger("doDoubleJump")
	}

	/// <summary>
	/// Determines if the player is beside a platform allowing for a double jump.
	/// </summary>
	/// <returns>True if the player is next to a platform within the check radius.</returns>
	private bool IsBesidePlatform()
	{
		// Check both left and right side platform points for overlap with the platform layer.
		return Physics2D.OverlapCircle(leftPlatformsCheckPoint.position, platformCheckRadius, whatIsPlatform) ||
			   Physics2D.OverlapCircle(rightPlatformsCheckPoint.position, platformCheckRadius, whatIsPlatform);
	}

	/// <summary>
	/// Flips the player's sprite to face the direction of movement.
	/// </summary>
	private void HandleFlippingSide()
	{
		// Flip the character's localScale.x to face the correct direction of movement.
		if (rb.velocity.x > 0)
			transform.localScale = Vector3.one;
		else if (rb.velocity.x < 0)
			transform.localScale = new Vector3(-1f, 1f, 1f);
	}

	/// <summary>
	/// Handles the player's animations based on movement and ground states.
	/// </summary>
	private void HandleAnimations()
	{
		// Update Animator parameters with the current speed and grounded state.
		anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
		anim.SetBool("isGrounded", isGrounded);
		anim.SetFloat("ySpeed", rb.velocity.y);
	}

	/// <summary>
	/// Applies knockback to the player and triggers knockback animation.
	/// </summary>
	public void KnockBack()
	{
		knockbackCounter = knockbackLength;
		rb.velocity = new Vector2(knockbackSpeed * -transform.localScale.x, rb.velocity.y);
		anim.SetTrigger("isKnockingBack");
	}

	/// <summary>
	/// Handles the knockback effect over time and resets player velocity after the effect.
	/// </summary>
	private void HandleKnockback()
	{
		knockbackCounter -= Time.deltaTime;
		if (knockbackCounter <= 0)
		{
			rb.velocity = new Vector2(0f, rb.velocity.y);
		}
	}

	
}
	