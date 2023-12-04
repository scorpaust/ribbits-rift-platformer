using UnityEngine;

/// <summary>
/// Controls enemy behavior, including interactions with the player and handling defeat animations.
/// </summary>
public class EnemyController : MonoBehaviour
{
	[Header("Enemy Configuration")]
	[Tooltip("Time to wait before destroying the enemy after being defeated.")]
	[SerializeField] private float waitToDestroy = 0.2f;

	private Animator anim;
	private bool isDefeated = false;

	/// <summary>
	/// Gets a value indicating whether the enemy is defeated.
	/// </summary>
	public bool IsDefeated => isDefeated;

	/// <summary>
	/// Start is called before the first frame update.
	/// Initializes enemy components.
	/// </summary>
	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	/// <summary>
	/// Update is called once per frame.
	/// Manages the destruction of the enemy after a delay if defeated.
	/// </summary>
	private void Update()
	{
		if (isDefeated)
		{
			HandleDefeatedState();
		}
	}

	/// <summary>
	/// Called when this enemy collides with another object.
	/// Damages the player if the enemy is not defeated.
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player") && !isDefeated)
		{
			PlayerHealthController.Instance.DamagePlayer();
		}
	}

	/// <summary>
	/// Called when another object enters a trigger collider attached to this enemy.
	/// Triggers the defeat animation and logic if collided by the player.
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			TriggerDefeat(other);
		}
	}

	/// <summary>
	/// Handles the defeated state of the enemy, counting down to its destruction.
	/// </summary>
	private void HandleDefeatedState()
	{
		waitToDestroy -= Time.deltaTime;

		if (waitToDestroy <= 0f)
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Triggers the defeat sequence of the enemy.
	/// </summary>
	/// <param name="playerCollider">Collider of the player who defeated the enemy.</param>
	private void TriggerDefeat(Collider2D playerCollider)
	{
		playerCollider.GetComponent<PlayerController>().Jump(false);
		anim.SetTrigger("Defeated");
		isDefeated = true;
	}
}
