using UnityEngine;

/// <summary>
/// Represents a health pickup item in the game that the player can collect to restore health.
/// </summary>
public class HealthPickup : MonoBehaviour
{
	[Header("Health Pickup Properties")]
	[Tooltip("Amount of health to add to the player's current health when this pickup is collected.")]
	[SerializeField] private int healthToAdd;

	[Tooltip("Determines if this pickup should fully restore the player's health.")]
	[SerializeField] private bool giveFullHealth;

	[Header("Visual and Audio Effects")]
	[Tooltip("The particle system effect that spawns when the player grabs the health pickup.")]
	[SerializeField] private GameObject pickupEffect;

	/// <summary>
	/// Called when another object enters a trigger collider attached to this object.
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	private void OnTriggerEnter2D(Collider2D other)
	{
		// Early exit if the player's health is already full.
		if (PlayerHealthController.Instance.CurrentHealth == PlayerHealthController.Instance.MaxHealth) return;

		// Check if the colliding object is the player.
		if (other.CompareTag("Player"))
		{
			CollectHealth(other);
		}
	}

	/// <summary>
	/// Collects the health and applies the effect to the player.
	/// </summary>
	/// <param name="playerCollider">The Collider2D component of the player.</param>
	private void CollectHealth(Collider2D playerCollider)
	{
		// Restore health to the player.
		if (giveFullHealth)
		{
			PlayerHealthController.Instance.AddHealth(PlayerHealthController.Instance.MaxHealth);
		}
		else
		{
			PlayerHealthController.Instance.AddHealth(healthToAdd);
		}

		// Instantiate visual effect at the location of the pickup.
		Instantiate(pickupEffect, transform.position, transform.rotation);

		// Destroy the health pickup game object.
		Destroy(gameObject);

		// Play SFX
		AudioManager.Instance.PlaySFX(10, false);
	}
}
