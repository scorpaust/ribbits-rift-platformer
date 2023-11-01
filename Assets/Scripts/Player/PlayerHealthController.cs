using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the player's health, providing methods to deal damage and heal.
/// </summary>
public class PlayerHealthController : MonoBehaviour
{
	// Singleton instance to allow easy global access to the PlayerHealthController.
	public static PlayerHealthController Instance { get; private set; }

	[Tooltip("The current health of the player.")]
	[SerializeField]
	private int currentHealth;
	public int CurrentHealth
	{
		get => currentHealth;
		private set
		{
			// Ensure the health never exceeds the maximum or falls below zero.
			currentHealth = Mathf.Clamp(value, 0, maxHealth);
		}
	}

	[Tooltip("The maximum health of the player.")]
	[SerializeField]
	private int maxHealth;

	// Parameters for the death jump
	[SerializeField] private float deathJumpForce = 10f;
	private Rigidbody2D rb;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// Initializes the singleton instance.
	/// </summary>
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			// If a duplicate exists, destroy it to enforce the singleton pattern.
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	/// <summary>
	/// Start is called before the first frame update.
	/// Initializes the current health to the maximum value at the start of the game.
	/// </summary>
	private void Start()
	{
		CurrentHealth = maxHealth;

		rb = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// Damages the player by reducing their current health by one.
	/// </summary>
	public void DamagePlayer()
	{
		CurrentHealth--;
		// Additional logic for when the player takes damage can be added here.
		// For example, triggering a damage animation or checking for player death.

		if (CurrentHealth <= 0f)
		{
			CurrentHealth = 0;

			StartCoroutine(HandleDeath());
		}
	}

	/// <summary>
	/// Handles the player's death, triggering the death animation and any additional logic required.
	/// </summary>
	private IEnumerator HandleDeath()
	{
		CapsuleCollider2D collider = PlayerController.Instance.GetComponent<CapsuleCollider2D>();
		// Apply an upward force to make the player 'jump' into the air
		rb.velocity = new Vector2(rb.velocity.x, deathJumpForce);

		yield return new WaitForSeconds(.5f);

		// Disable further movement and collision handling here if needed
        // Disable the player's ability to control the character
		PlayerController.Instance.IsActive = false;

		// Disable player's collider to make it fall
		if (collider != null)
			collider.enabled = false;
		
		yield return new WaitForSeconds(2f);

		// Restart current scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
