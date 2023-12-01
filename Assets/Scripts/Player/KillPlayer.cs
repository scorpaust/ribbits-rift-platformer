using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the logic for killing the player character when colliding with a hazardous object.
/// </summary>
public class KillPlayer : MonoBehaviour
{
	/// <summary>
	/// Called when another object enters a trigger collider attached to this object.
	/// Initiates the player's death process if the colliding object is the player.
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			StartCoroutine(HandleDeath());
		}
	}

	/// <summary>
	/// Coroutine to manage the delay before respawning the player.
	/// </summary>
	private IEnumerator HandleDeath()
	{
		// Wait for a specified time before respawning the player.
		yield return new WaitForSeconds(2f);
		LifeController.instance.Respawn();
	}
}

