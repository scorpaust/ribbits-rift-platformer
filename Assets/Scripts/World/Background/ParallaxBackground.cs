using UnityEngine;

/// <summary>
/// Manages parallax effect for background elements relative to the camera's position.
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
	// Cached reference to the main camera's transform.
	private Transform cameraTransform;

	[Tooltip("Reference to the sky element of the background.")]
	[SerializeField] private Transform theSky;

	[Tooltip("Reference to the treeline element of the background.")]
	[SerializeField] private Transform theTreeline;

	[Tooltip("The speed at which the treeline moves relative to the camera, creating a parallax effect.")]
	[SerializeField][Range(0f, 1f)] private float parallaxSpeed;

	/// <summary>
	/// Initialization of the script, caching the main camera's transform.
	/// </summary>
	private void Start()
	{
		cameraTransform = Camera.main.transform;
	}

	/// <summary>
	/// Updates the position of parallax elements each frame after all other updates have occurred.
	/// </summary>
	private void LateUpdate()
	{
		// The sky position matches the camera position on the x and y axes but maintains its original z position.
		theSky.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, theSky.position.z);

		// The treeline position is a fraction of the camera's position, determined by the parallax speed.
		// This creates the illusion of depth due to the different rates of movement between the foreground and background.
		theTreeline.position = new Vector3(cameraTransform.position.x * parallaxSpeed,
											cameraTransform.position.y * parallaxSpeed,
											theTreeline.position.z);
	}
}

