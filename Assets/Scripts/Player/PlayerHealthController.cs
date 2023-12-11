using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the player's health, handling damage, invincibility frames, death, and health restoration.
/// </summary>
public class PlayerHealthController : MonoBehaviour
{
	public static PlayerHealthController Instance { get; private set; }

	[Header("Health Configuration")]
	[Tooltip("The current health of the player.")]
	[SerializeField] private int currentHealth;
	[Tooltip("The maximum health of the player.")]
	[SerializeField] private int maxHealth;

	[Header("Death and Invincibility")]
	[Tooltip("The force applied when the player dies to create a 'jump' effect.")]
	[SerializeField] private float deathJumpForce = 10f;
	[Tooltip("The duration for which the player is invincible after taking damage.")]
	[SerializeField] private float invincibilityLength = 0.5f;

	private float invincibilityCounter;
	private Rigidbody2D rb;
	private PlayerController thePlayer;

	public int CurrentHealth
	{
		get => currentHealth;
		private set => currentHealth = Mathf.Clamp(value, 0, maxHealth);
	}

	public int MaxHealth => maxHealth;

	/// <summary>
	/// Initializes the singleton instance and component references.
	/// </summary>
	private void Awake()
	{
		ManageSingletonInstance();
	}

	/// <summary>
	/// Sets initial health and updates the health display on the UI.
	/// </summary>
	private void Start()
	{
		InitializeHealth();
	}

	/// <summary>
	/// Updates the invincibility counter on each frame.
	/// </summary>
	private void Update()
	{
		CheckInvincibility();
	}

	/// <summary>
	/// Damages the player if they are not currently invincible.
	/// </summary>
	public void DamagePlayer()
	{
		if (invincibilityCounter <= 0)
		{
			ProcessPlayerDamage();
		}
	}

	/// <summary>
	/// Manages the singleton instance and initializes components.
	/// </summary>
	private void ManageSingletonInstance()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			rb = GetComponent<Rigidbody2D>();
			thePlayer = GetComponent<PlayerController>();
		}
	}

	/// <summary>
	/// Sets the player's current health to maximum and updates the UI.
	/// </summary>
	private void InitializeHealth()
	{
		CurrentHealth = maxHealth;
		UpdateHealthUI();
	}

	/// <summary>
	/// Checks and updates the invincibility counter.
	/// </summary>
	private void CheckInvincibility()
	{
		if (invincibilityCounter > 0)
		{
			invincibilityCounter -= Time.deltaTime;
		}
	}

	/// <summary>
	/// Processes the player taking damage, including updating health, handling knockback, and death.
	/// </summary>
	private void ProcessPlayerDamage()
	{
		invincibilityCounter = invincibilityLength;
		CurrentHealth--;
		thePlayer.KnockBack();
		UpdateHealthUI();

		if (CurrentHealth <= 0)
		{
			StartCoroutine(HandleDeath());
		}
		else
		{
			AudioManager.Instance.PlaySFX(13, false);
		}
	}

	/// <summary>
	/// Handles the player's death process including disabling control, applying a jump force, and respawning.
	/// </summary>
	private IEnumerator HandleDeath()
	{
		PlayerControlAndCollider(false);
		rb.velocity = new Vector2(rb.velocity.x, deathJumpForce);

		yield return new WaitForSeconds(0.5f);

		LifeController.instance.Respawn();
		InitializeHealth();
		PlayerControlAndCollider(true);
	}

	/// <summary>
	/// Enables or disables player control and collider.
	/// </summary>
	/// <param name="enabled">Whether to enable or disable control and collider.</param>
	private void PlayerControlAndCollider(bool enabled)
	{
		thePlayer.IsActive = enabled;
		CapsuleCollider2D collider = thePlayer.GetComponent<CapsuleCollider2D>();
		collider.enabled = enabled;
	}

	/// <summary>
	/// Updates the health display on the UI.
	/// </summary>
	private void UpdateHealthUI()
	{
		UIController.Instance.UpdateHealthDisplay(CurrentHealth, MaxHealth);
	}

	/// <summary>
	/// Adds a specified amount of health to the player and updates the UI.
	/// </summary>
	/// <param name="amountToAdd">The amount of health to add.</param>
	public void AddHealth(int amountToAdd)
	{
		CurrentHealth += amountToAdd;
		UpdateHealthUI();
	}
}
