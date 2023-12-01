using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the player's health, handling damage, invincibility frames, death, and health restoration.
/// </summary>
public class PlayerHealthController : MonoBehaviour
{
	// Singleton Instance
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

	private void Awake()
	{
		ManageSingletonInstance();
	}

	private void Start()
	{
		InitializeHealth();
	}

	private void Update()
	{
		CheckInvincibility();
	}

	public void DamagePlayer()
	{
		if (invincibilityCounter <= 0)
		{
			ProcessPlayerDamage();
		}
	}

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

	private void InitializeHealth()
	{
		CurrentHealth = maxHealth;
		UIController.Instance.UpdateHealthDisplay(CurrentHealth, MaxHealth);
	}

	private void CheckInvincibility()
	{
		if (invincibilityCounter > 0)
		{
			invincibilityCounter -= Time.deltaTime;
		}
	}

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
	}

	private IEnumerator HandleDeath()
	{
		DisablePlayerControlAndCollider();
		rb.velocity = new Vector2(rb.velocity.x, deathJumpForce);

		yield return new WaitForSeconds(0.5f);

		LifeController.instance.Respawn();
		InitializeHealth();
	}

	private void DisablePlayerControlAndCollider()
	{
		thePlayer.IsActive = false;
		CapsuleCollider2D collider = thePlayer.GetComponent<CapsuleCollider2D>();
		if (collider != null) collider.enabled = false;
	}

	private void UpdateHealthUI()
	{
		UIController.Instance.UpdateHealthDisplay(CurrentHealth, MaxHealth);
	}

	public void AddHealth(int amountToAdd)
	{
		CurrentHealth += amountToAdd;
		UpdateHealthUI();
	}
}
