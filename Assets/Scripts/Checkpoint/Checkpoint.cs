using UnityEngine;

/// <summary>
/// Represents a checkpoint in the game that the player can activate by colliding with it.
/// Once activated, it communicates with the CheckpointManager to update the game's state.
/// </summary>
public class Checkpoint : MonoBehaviour
{
	[Header("Checkpoint State")]
	[Tooltip("Indicates if the checkpoint is currently active.")]
	private bool isActive;

	[Header("Components")]
	[Tooltip("Animator component for handling checkpoint animations.")]
	private Animator anim;

	[Header("Checkpoint Management")]
	[Tooltip("Reference to the CheckpointManager that manages all checkpoints.")]
	private CheckpointManager chkpMan;

	/// <summary>
	/// Property for getting and setting the CheckpointManager.
	/// </summary>
	public CheckpointManager ChkpMan
	{
		get => chkpMan;
		set => chkpMan = value;
	}

	/// <summary>
	/// Start is called before the first frame update.
	/// Initializes the checkpoint's components.
	/// </summary>
	private void Start()
	{
		anim = GetComponentInChildren<Animator>();
	}

	/// <summary>
	/// Called when another object enters a trigger collider attached to this object.
	/// Activates the checkpoint if collided by the player and if it's not already active.
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !isActive)
		{
			ActivateCheckpoint();
		}
	}

	/// <summary>
	/// Activates this checkpoint, updating its animation state and notifying the CheckpointManager.
	/// </summary>
	private void ActivateCheckpoint()
	{
		ChkpMan.SetActiveCheckpoint(this);
		anim.SetBool("flagActive", true);
		isActive = true;
		AudioManager.Instance.PlaySFX(3, false);
	}

	/// <summary>
	/// Deactivates this checkpoint, resetting its animation state.
	/// </summary>
	public void DeactivateCheckpoint()
	{
		anim.SetBool("flagActive", false);
		isActive = false;
	}
}
