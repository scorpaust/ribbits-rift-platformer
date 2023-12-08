using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the UI elements in the game, including health display, lives count, game over screen, and collectables count.
/// </summary>
public class UIController : MonoBehaviour
{
	// Singleton declaration
	public static UIController Instance { get; private set; }

	[Header("UI Components")]
	[Tooltip("Array of Image components representing the heart icons on the UI.")]
	[SerializeField] private Image[] heartIcons;
	[Tooltip("Pause screen")]
	[SerializeField] private GameObject pauseScreen;

	[Header("Heart Sprites")]
	[Tooltip("Sprite to represent a full heart.")]
	[SerializeField] private Sprite heartFull;
	[Tooltip("Sprite to represent an empty heart.")]
	[SerializeField] private Sprite heartEmpty;

	[Header("Text Displays")]
	[Tooltip("Text display for the number of lives.")]
	[SerializeField] private TMP_Text livesText;
	[Tooltip("Game over screen GameObject.")]
	[SerializeField] private GameObject gameOverScreen;
	[Tooltip("Text display for the number of collectables.")]
	[SerializeField] private TMP_Text collectablesText;

	/// <summary>
	/// Initializes the singleton instance of the UIController.
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
		}
	}

	private void Update()
	{
		PauseUnpause();
	}

	/// <summary>
	/// Updates the heart icons on the UI to reflect the player's current and maximum health.
	/// </summary>
	/// <param name="health">The player's current health.</param>
	/// <param name="maxHealth">The player's maximum health.</param>
	public void UpdateHealthDisplay(int health, int maxHealth)
	{
		for (int i = 0; i < heartIcons.Length; i++)
		{
			heartIcons[i].enabled = i < maxHealth;
			heartIcons[i].sprite = i < health ? heartFull : heartEmpty;
		}
	}

	/// <summary>
	/// Updates the displayed number of lives on the UI.
	/// </summary>
	/// <param name="currentLives">The current number of lives.</param>
	public void UpdateLivesDisplay(int currentLives)
	{
		livesText.text = "x " + currentLives;
	}

	/// <summary>
	/// Shows the game over screen.
	/// </summary>
	public void ShowGameOver()
	{
		gameOverScreen.SetActive(true);
	}

	/// <summary>
	/// Restarts the current game scene.
	/// </summary>
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 1f;
	}

	/// <summary>
	/// Updates the displayed count of collectables on the UI.
	/// </summary>
	/// <param name="amount">The number of collectables collected.</param>
	public void UpdateCollectables(int amount)
	{
		collectablesText.text = amount.ToString();
	}

	public void PauseUnpause()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!pauseScreen.activeInHierarchy)
			{
				pauseScreen.SetActive(true);
				Time.timeScale = 0f;
			}
			else
			{
				pauseScreen.SetActive(false);
				Time.timeScale = 1f;
			}
		}
	}

	/// <summary>
	/// Returns player to the Main Menu screen.
	/// </summary>
	public void ReturnToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1f;
	}

	/// <summary>
	/// Closes the game. In the Unity Editor, it stops the play mode. In a production build, it quits the application.
	/// </summary>
	public void QuitGame()
	{
		#if UNITY_EDITOR
				// Stop the game if running in the Unity Editor
				EditorApplication.isPlaying = false;
		#else
						// Quit the game if running in a production build
						Application.Quit();
		#endif
	}
}


