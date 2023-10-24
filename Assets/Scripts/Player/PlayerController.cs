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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        HandleJumping();
    }

    void Move()
    {
		activeSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

		// Set the horizontal velocity of the object based on player input, while preserving the current vertical velocity.
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * activeSpeed, rb.velocity.y);
    }

    void HandleJumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
			// Set the vertical velocity of the object to the jump force, while preserving the current horizontal velocity.
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
