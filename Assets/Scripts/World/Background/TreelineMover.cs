using UnityEngine;

/// <summary>
/// Moves the treeline in relation to the camera's position to create a looping background effect.
/// </summary>
public class TreelineMover : MonoBehaviour
{
	[Tooltip("Maximum distance from the camera at which the treeline will reset to create a continuous loop.")]
	[SerializeField] private float maxDistance = 22f;

	// Cached transform of the main camera to avoid repeated calls to Camera.main.
	private Transform cameraTransform;

	/// <summary>
	/// Initialization of the script, caching the main camera's transform.
	/// </summary>
	private void Start()
	{
		// Cache the camera transform at start to optimize performance.
		cameraTransform = Camera.main.transform;
	}

	/// <summary>
	/// Moves the treeline to create a looping background effect as the camera moves.
	/// </summary>
	private void Update()
	{
		// Calculate the distance between the treeline and the camera.
		float distance = transform.position.x - cameraTransform.position.x;

		// If the treeline is too far to the right, loop it to the left.
		if (distance > maxDistance)
		{
			LoopTreeline(-maxDistance * 2f);
		}
		// If the treeline is too far to the left, loop it to the right.
		else if (distance < -maxDistance)
		{
			LoopTreeline(maxDistance * 2f);
		}
	}

	/// <summary>
	/// Moves the treeline by a given offset to loop it around the camera's position.
	/// </summary>
	/// <param name="offset">The amount to move the treeline by, which is double the maxDistance.</param>
	private void LoopTreeline(float offset)
	{
		transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
	}
}
