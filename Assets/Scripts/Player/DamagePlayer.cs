using System.Collections;
using UnityEngine;


/// <summary>
/// This component damages the player upon collision with a trap or hazard.
/// It provides visual feedback through a blinking effect and applies a physical impulse
/// to represent the knockback from the damage taken.
/// </summary>
public class DamagePlayer : MonoBehaviour
{
	[Tooltip("The force applied to the player when taking damage.")]
	[SerializeField] private float knockbackForce = 10f;

	[Tooltip("Duration of the blink effect when the player takes damage.")]
	[SerializeField] private float blinkDuration = 0.5f;

	[Tooltip("Frequency of the blinks during the blink effect.")]
	[SerializeField] private float blinkFrequency = 0.1f;

	// Is player already blinking?
	private bool playerBlinking = false;


	/// <summary>
	/// Called when another collider enters the trigger collider attached to this object.
	/// If the player enters the trigger, it will apply a knockback force and initiate a blinking effect.
	/// </summary>
	/// <param name="other">The collider that entered the trigger.</param>
	private void OnTriggerEnter2D(Collider2D other)
	{
		// Check if the colliding object is tagged as "Player", if player is not already blinking and if player is active or not dead.
		if (other.CompareTag("Player") && !playerBlinking && PlayerController.Instance.IsActive)
		{
			// Applies damage to the player.
			PlayerHealthController.Instance.DamagePlayer();

			// Apply a knockback force to the player's Rigidbody2D.
			Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
			if (playerRb != null)
			{
				Vector2 damageDirection = other.transform.position - transform.position;
				damageDirection = damageDirection.normalized * knockbackForce;
				playerRb.AddForce(damageDirection, ForceMode2D.Impulse);
			}

			// Start the blinking effect on the player's SpriteRenderer.
			SpriteRenderer playerSprite = other.GetComponentInChildren<SpriteRenderer>();
			if (playerSprite != null)
			{
				StartCoroutine(BlinkSprite(playerSprite));
			}
		}
	}

	/// <summary>
	/// Coroutine to make the sprite blink in a specified color for a set duration and frequency.
	/// </summary>
	/// <param name="spriteRenderer">The SpriteRenderer component of the player.</param>
	/// <returns>IEnumerator for coroutine sequencing.</returns>
	private IEnumerator BlinkSprite(SpriteRenderer spriteRenderer)
	{
		    // Player starts to blink
			playerBlinking = true;

			// Store the original color of the sprite.
			Color originalColor = spriteRenderer.color;
			// The color to blink to (red by default).
			Color blinkColor = Color.red;

			blinkColor.a = 0.8f;
			
			// Calculate the time when the blinking will end.
			float blinkEndTime = Time.time + blinkDuration;

			// Continue blinking until the current time exceeds the end time and if not player is already blinking.
			while (Time.time < blinkEndTime)
			{
				// Toggle between the original color and the blink color.
				spriteRenderer.color = spriteRenderer.color == originalColor ? blinkColor : originalColor;

				// Wait for the specified frequency duration before the next toggle.
				yield return new WaitForSeconds(blinkFrequency);
			}

			// Ensure the color of the sprite is reset to the original after blinking ends.
			spriteRenderer.color = originalColor;

		    // Player stops blinking
			playerBlinking = false;
	}
}

