using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the player's lives, handling respawning and game over scenarios.
/// </summary>
public class LifeController : MonoBehaviour
{
	// Singleton Instance
	public static LifeController instance { get; private set; }

	[Header("Player and Checkpoint")]
	private PlayerController thePlayer;

	[Header("Respawn Settings")]
	[Tooltip("Delay before respawning the player.")]
	[SerializeField] private float respawnDelay = 5f;

	[Header("Player Lives")]
	[Tooltip("Current number of lives the player has.")]
	[SerializeField] private int currentLives = 3;

	[Header("Effects")]
	[Tooltip("Effect to instantiate when the player respawns.")]
	[SerializeField] private GameObject respawnEffect;
	[Tooltip("Effect to instantiate when the player dies.")]
	[SerializeField] private GameObject deathEffect;

	/// <summary>
	/// Initializes the singleton instance of the LifeController.
	/// </summary>
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}

	/// <summary>
	/// Start is called before the first frame update.
	/// Initializes player references.
	/// </summary>
	private void Start()
	{
		thePlayer = FindObjectOfType<PlayerController>();
	}

	/// <summary>
	/// Handles the player's respawn process or triggers game over if out of lives.
	/// </summary>
	public void Respawn()
	{
		currentLives--;
		Instantiate(deathEffect, thePlayer.transform.position, thePlayer.transform.rotation);
		UpdateDisplay();

		if (currentLives > 0)
		{
			StartCoroutine(RespawnCo());
		}
		else
		{
			currentLives = 0;
			StartCoroutine(GameOverCo());
		}
	}

	/// <summary>
	/// Coroutine to manage the respawn delay and process.
	/// </summary>
	private IEnumerator RespawnCo()
	{
		yield return new WaitForSeconds(2f);

		// Reset player's velocity and position.
		var playerRb = thePlayer.GetComponent<Rigidbody2D>();
		playerRb.velocity = new Vector2(playerRb.velocity.x, 20f);

		yield return new WaitForSeconds(respawnDelay);

		CheckpointManager checkpointManager = FindObjectOfType<CheckpointManager>();
		thePlayer.transform.position = checkpointManager.respawnPosition;

		Instantiate(respawnEffect, thePlayer.transform.position, thePlayer.transform.rotation);

		AudioManager.Instance.PlaySFX(11, false);
	}

	/// <summary>
	/// Coroutine to handle game over delay and process.
	/// </summary>
	private IEnumerator GameOverCo()
	{
		yield return new WaitForSeconds(respawnDelay);
		UIController.Instance?.ShowGameOver();
	}

	/// <summary>
	/// Adds a life to the player's current lives and updates the display.
	/// </summary>
	public void AddLife()
	{
		currentLives++;
		UpdateDisplay();
		AudioManager.Instance.PlaySFX(8, false);
	}

	/// <summary>
	/// Updates the lives display on the UI.
	/// </summary>
	private void UpdateDisplay()
	{
		UIController.Instance?.UpdateLivesDisplay(currentLives);
	}
}
