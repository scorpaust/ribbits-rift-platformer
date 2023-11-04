using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI elements related to player health, updating heart icons to reflect the current and maximum health.
/// </summary>
public class UIController : MonoBehaviour
{
	public static UIController Instance { get; private set; }

	[Header("UI Components")]
	[Tooltip("Array of Image components representing the heart icons on the UI.")]
	[SerializeField] private Image[] heartIcons;

	[Header("Heart Sprites")]
	[Tooltip("Sprite to represent a full heart.")]
	[SerializeField] private Sprite heartFull;
	[Tooltip("Sprite to represent an empty heart.")]
	[SerializeField] private Sprite heartEmpty;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// Initializes the singleton instance.
	/// </summary>
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject); // Prevent duplicate instances of the singleton.
		}
		else
		{
			Instance = this;
		}
	}

	/// <summary>
	/// Updates the heart icons to reflect the player's current health.
	/// </summary>
	/// <param name="health">The player's current health.</param>
	/// <param name="maxHealth">The player's maximum health.</param>
	public void UpdateHealthDisplay(int health, int maxHealth)
	{
		for (int i = 0; i < heartIcons.Length; i++)
		{
			// Ensure all heart icons are active to start with.
			heartIcons[i].enabled = true;

			// Determine the sprite to show for each heart based on current health.
			heartIcons[i].sprite = health > i ? heartFull : heartEmpty;

			// Disable heart icons that exceed the player's maximum health.
			if (maxHealth <= i)
			{
				heartIcons[i].enabled = false;
			}
		}
	}
}
