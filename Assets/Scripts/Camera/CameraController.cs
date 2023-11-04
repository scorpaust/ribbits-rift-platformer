using UnityEngine;

/// <summary>
/// CameraController follows a target Transform while optionally clamping its movement within defined boundaries
/// and allowing for freezing of horizontal or vertical movement.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	[Header("Camera Follow Attributes")]
	[Tooltip("The target object that the camera will follow.")]
	[SerializeField] private Transform target;

	[Tooltip("Should the vertical movement of the camera be frozen?")]
	[SerializeField] private bool freezeVertical;

	[Tooltip("Should the horizontal movement of the camera be frozen?")]
	[SerializeField] private bool freezeHorizontal;

	[Header("Camera Clamp Attributes")]
	[Tooltip("Should the camera's position be clamped within certain bounds?")]
	[SerializeField] private bool clampPosition;

	[Tooltip("The minimum boundary for camera position clamping.")]
	[SerializeField] private Transform clampMin;

	[Tooltip("The maximum boundary for camera position clamping.")]
	[SerializeField] private Transform clampMax;

	[Header("Camera Component Reference")]
	[Tooltip("Reference to the camera component.")]
	private Camera theCam;

	// Private variables to store runtime values
	private Vector3 positionStore;
	private float halfHeight;
	private float halfWidth;

	private void Start()
	{
		InitialSetup();
	}

	private void LateUpdate()
	{
		FollowTarget();
		ControlCameraMovementAxis();
		ClampCameraPosition();
		UpdateParallaxBackground();
	}

	/// <summary>
	/// Initializes necessary components and calculates initial values for camera position clamping.
	/// </summary>
	private void InitialSetup()
	{
		theCam = GetComponent<Camera>();
		positionStore = transform.position;

		// Ensure that the clamping boundaries are not parented to the camera
		// to avoid moving them with the camera.
		if (clampMin) clampMin.SetParent(null);
		if (clampMax) clampMax.SetParent(null);

		// Calculate half of the camera's viewport dimensions for clamping calculations.
		halfHeight = theCam.orthographicSize;
		halfWidth = theCam.aspect * halfHeight;
	}

	/// <summary>
	/// Follows the assigned target's position while respecting the freeze constraints.
	/// </summary>
	private void FollowTarget()
	{
		// Set the camera's position to follow the target, applying freezing constraints as necessary.
		float xPosition = freezeHorizontal ? positionStore.x : target.position.x;
		float yPosition = freezeVertical ? positionStore.y : target.position.y;
		transform.position = new Vector3(xPosition, yPosition, transform.position.z);
	}

	/// <summary>
	/// Freezes the camera's movement on the horizontal axis, vertical axis, or both.
	/// </summary>
	private void ControlCameraMovementAxis()
	{
		// If either axis is frozen, reset that axis to the stored position.
		if (freezeVertical || freezeHorizontal)
		{
			transform.position = new Vector3(
				freezeHorizontal ? positionStore.x : transform.position.x,
				freezeVertical ? positionStore.y : transform.position.y,
				transform.position.z);
		}
	}

	/// <summary>
	/// Clamps the camera's position within the specified minimum and maximum boundaries.
	/// </summary>
	private void ClampCameraPosition()
	{
		// If clamping is enabled, adjust the camera's position to stay within the defined boundaries.
		if (clampPosition && clampMin && clampMax)
		{
			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x, clampMin.position.x + halfWidth, clampMax.position.x - halfWidth),
				Mathf.Clamp(transform.position.y, clampMin.position.y + halfHeight, clampMax.position.y - halfHeight),
				transform.position.z);
		}
	}

	/// <summary>
	/// Visualizes the camera clamping boundaries within the Unity Editor.
	/// </summary>
	private void OnDrawGizmos()
	{
		if (clampPosition && clampMin && clampMax)
		{
			Gizmos.color = Color.cyan;
			// Draw lines connecting the clamping boundary corners.
			Gizmos.DrawLine(new Vector3(clampMin.position.x, clampMin.position.y, 0f), new Vector3(clampMin.position.x, clampMax.position.y, 0f));
			Gizmos.DrawLine(new Vector3(clampMin.position.x, clampMin.position.y, 0f), new Vector3(clampMax.position.x, clampMin.position.y, 0f));
			Gizmos.DrawLine(new Vector3(clampMax.position.x, clampMax.position.y, 0f), new Vector3(clampMin.position.x, clampMax.position.y, 0f));
			Gizmos.DrawLine(new Vector3(clampMax.position.x, clampMax.position.y, 0f), new Vector3(clampMax.position.x, clampMin.position.y, 0f));
		}
	}

	/// <summary>
	/// Updates the position of the parallax background elements based on the camera's movement.
	/// </summary>
	private void UpdateParallaxBackground()
	{
		// Ensures that the ParallaxBackground instance exists and calls its MoveBackground method.
		ParallaxBackground parallaxBackground = ParallaxBackground.Instance;
		if (parallaxBackground != null)
		{
			parallaxBackground.MoveBackground();
		}
	}
}
