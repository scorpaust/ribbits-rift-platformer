using UnityEngine;

/// <summary>
/// Manages collectable pickups in the game. When the player collides with a collectable, it updates the collectable count and triggers an effect.
/// </summary>
public class CollectablePickup : MonoBehaviour
{
	[Header("Collectable Configuration")]
	[Tooltip("The amount of collectable value this pickup represents.")]
	[SerializeField] private int amount = 1;

	[Header("Visual Effects")]
	[Tooltip("Effect to instantiate when the collectable is picked up.")]
	[SerializeField] private GameObject pickupEffect;

	/// <summary>
	/// Called when another object enters a trigger collider attached to this object.
	/// If the object is the player, it processes the collectable pickup.
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			ProcessCollectablePickup();
		}
	}

	/// <summary>
	/// Processes the pickup of the collectable, updating the collectable count and spawning an effect.
	/// </summary>
	private void ProcessCollectablePickup()
	{
		if (CollectablesManager.instance != null)
		{
			CollectablesManager.instance.GetCollectable(amount);
			InstantiatePickupEffect();
			Destroy(gameObject);
			AudioManager.Instance.PlaySFX(9, true);
		}
	}

	/// <summary>
	/// Instantiates the pickup effect at the location of the collectable.
	/// </summary>
	private void InstantiatePickupEffect()
	{
		Instantiate(pickupEffect, transform.position, Quaternion.identity);
	}
}
