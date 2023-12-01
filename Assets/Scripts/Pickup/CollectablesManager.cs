using UnityEngine;

/// <summary>
/// Manages the collection of collectables in the game, and awards extra lives when certain thresholds are reached.
/// </summary>
public class CollectablesManager : MonoBehaviour
{
	// Singleton Instance
	public static CollectablesManager instance { get; private set; }

	[Header("Collectables Configuration")]
	[Tooltip("Current count of collectables collected.")]
	[SerializeField] private int collectableCount;
	[Tooltip("Number of collectables required to earn an extra life.")]
	[SerializeField] private int extraLifeThreshold;

	/// <summary>
	/// Initializes the singleton instance of the CollectablesManager.
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
	/// Processes the collection of a specified amount of collectables, updates the count, 
	/// and awards an extra life if the threshold is reached.
	/// </summary>
	/// <param name="amount">The amount of collectables collected.</param>
	public void GetCollectable(int amount)
	{
		collectableCount += amount;

		// Check if the threshold for an extra life is reached.
		if (collectableCount >= extraLifeThreshold)
		{
			collectableCount -= extraLifeThreshold;
			LifeController.instance?.AddLife();
		}

		UIController.Instance?.UpdateCollectables(collectableCount);
	}
}
