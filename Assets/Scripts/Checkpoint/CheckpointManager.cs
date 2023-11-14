using UnityEngine;

/// <summary>
/// Manages all checkpoints in the game, handling activation and deactivation of checkpoints.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
	[Header("Checkpoint Management")]
	[Tooltip("Array of all checkpoints in the scene.")]
	private Checkpoint[] allCheckpoints;

	[Tooltip("Currently active checkpoint.")]
	private Checkpoint activeCheckpoint;

	/// <summary>
	/// Start is called before the first frame update.
	/// Initializes the checkpoints and assigns this manager to each.
	/// </summary>
	private void Start()
	{
		InitializeCheckpoints();
	}

	/// <summary>
	/// Initializes all checkpoints in the scene and assigns this manager to them.
	/// </summary>
	private void InitializeCheckpoints()
	{
		allCheckpoints = FindObjectsOfType<Checkpoint>();

		foreach (Checkpoint checkpoint in allCheckpoints)
		{
			checkpoint.ChkpMan = this;
		}
	}

	/// <summary>
	/// Deactivates all checkpoints.
	/// </summary>
	public void DeactivateAllCheckpoints()
	{
		foreach (Checkpoint checkpoint in allCheckpoints)
		{
			checkpoint.DeactivateCheckpoint();
		}
	}

	/// <summary>
	/// Sets a new active checkpoint and deactivates all others.
	/// </summary>
	/// <param name="newActiveCheckpoint">The checkpoint to be set as active.</param>
	public void SetActiveCheckpoint(Checkpoint newActiveCheckpoint)
	{
		DeactivateAllCheckpoints();
		activeCheckpoint = newActiveCheckpoint;
	}
}
