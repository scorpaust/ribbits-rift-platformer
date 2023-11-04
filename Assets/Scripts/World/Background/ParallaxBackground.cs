using UnityEngine;

/// <summary>
/// Manages the parallax effect for background elements, making them move at different speeds relative to the camera's position
/// to give a sense of depth.
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
	public static ParallaxBackground Instance { get; private set; }

	[Header("Parallax Elements")]
	[Tooltip("Reference to the sky element of the background.")]
	[SerializeField] private Transform theSky;

	[Tooltip("Reference to the treeline element of the background.")]
	[SerializeField] private Transform theTreeline;

	[Header("Parallax Settings")]
	[Tooltip("The speed at which the treeline moves relative to the camera, creating a parallax effect.")]
	[SerializeField][Range(0f, 1f)] private float parallaxSpeed;

	// Cached reference to the main camera's transform for performance optimization.
	private Transform cameraTransform;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// Here we set up the singleton instance.
	/// </summary>
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject); // Enforce singleton property
		}
		else
		{
			Instance = this;
		}
	}

	/// <summary>
	/// Start is called before the first frame update.
	/// Caches the main camera's transform for later use in moving the background.
	/// </summary>
	private void Start()
	{
		cameraTransform = Camera.main.transform;
	}

	/// <summary>
	/// Moves the background elements at different speeds to create a parallax effect.
	/// The sky moves with the camera to maintain its position, while the treeline moves at a set parallax speed.
	/// </summary>
	public void MoveBackground()
	{
		if (theSky)
		{
			theSky.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, theSky.position.z);
		}

		if (theTreeline)
		{
			theTreeline.position = new Vector3(cameraTransform.position.x * parallaxSpeed,
											   cameraTransform.position.y * parallaxSpeed,
											   theTreeline.position.z);
		}
	}
}
