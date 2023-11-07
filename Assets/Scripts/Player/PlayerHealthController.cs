using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the player's health and related effects such as invincibility frames and death handling.
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

	/// <summary>
	/// Damages the player if they are not currently invincible, triggering knockback and potential death.
	/// </summary>
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

		if (CurrentHealth <= 0)
		{
			StartCoroutine(HandleDeath());
		}
		else
		{
			UIController.Instance.UpdateHealthDisplay(CurrentHealth, MaxHealth);
		}
	}

	/// <summary>
	/// Handles the player's death animation and subsequent level reset.
	/// </summary>
	private IEnumerator HandleDeath()
	{
		DisablePlayerControlAndCollider();
		rb.velocity = new Vector2(rb.velocity.x, deathJumpForce);
		yield return new WaitForSeconds(0.5f);
		yield return new WaitForSeconds(2f);
		RestartCurrentScene();
	}

	private void DisablePlayerControlAndCollider()
	{
		thePlayer.IsActive = false;
		CapsuleCollider2D collider = thePlayer.GetComponent<CapsuleCollider2D>();
		if (collider != null) collider.enabled = false;
	}

	private void RestartCurrentScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void AddHealth(int amountToAdd)
	{
		CurrentHealth += amountToAdd;

		if (CurrentHealth > MaxHealth)
		{
			CurrentHealth = MaxHealth;
		}

		UIController.Instance.UpdateHealthDisplay(CurrentHealth, MaxHealth);
	}
}
