using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	[Header("Attributes")]

	[Tooltip("The target object that the camera will follow.")]
	[SerializeField] private Transform target;

	[Tooltip("Should the vertical movement of the camera be frozen?")]
	[SerializeField] private bool freezeVertical;

	[Tooltip("Should the horizontal movement of the camera be frozen?")]
	[SerializeField] private bool freezeHorizontal;

	[Tooltip("Stores the camera's initial position for freezing its movement along certain axes.")]
	private Vector3 positionStore;

	[Tooltip("Should the camera's position be clamped within certain bounds?")]
	[SerializeField] private bool clampPosition;

	[Tooltip("The minimum boundary for camera position clamping.")]
	[SerializeField] private Transform clampMin;

	[Tooltip("The maximum boundary for camera position clamping.")]
	[SerializeField] private Transform clampMax;

	[Tooltip("Half of the camera's viewport height.")]
	private float halfHeight;

	[Tooltip("Half of the camera's viewport width.")]
	private float halfWidth;

	[Header("References")]

	[Tooltip("Reference to the camera component.")]
	private Camera theCam;

	/// <summary>
	/// Called on the frame when a script is initialized.
	/// </summary>
	private void Start()
	{
		InitialSetup();
	}

	/// <summary>
	/// Called once per frame, after all Update functions have been called.
	/// </summary>
	private void LateUpdate()
	{
		FollowTarget();
		ControlCamMovementAxis();
		ClampPosition();
	}

	/// <summary>
	/// Initializes necessary components and sets up initial values.
	/// </summary>
	private void InitialSetup()
	{
		theCam = GetComponent<Camera>();
		positionStore = transform.position;

		// Detaching clamp positions from their parent to make sure they remain stationary.
		clampMin.SetParent(null);
		clampMax.SetParent(null);

		halfHeight = theCam.orthographicSize;
		halfWidth = theCam.orthographicSize * theCam.aspect;
	}

	/// <summary>
	/// Moves the camera to follow the target's position.
	/// </summary>
	private void FollowTarget()
	{
		transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
	}

	/// <summary>
	/// Freezes the camera movement along specified axes.
	/// </summary>
	private void ControlCamMovementAxis()
	{
		if (freezeVertical)
			transform.position = new Vector3(transform.position.x, positionStore.y, transform.position.z);

		if (freezeHorizontal)
			transform.position = new Vector3(positionStore.x, transform.position.y, transform.position.z);
	}

	/// <summary>
	/// Clamps the camera position within specified boundaries.
	/// </summary>
	private void ClampPosition()
	{
		if (clampPosition)
		{
			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x, clampMin.position.x + halfWidth, clampMax.position.x - halfWidth),
				Mathf.Clamp(transform.position.y, clampMin.position.y + halfHeight, clampMax.position.y - halfHeight),
				transform.position.z
			);
		}
	}

	/// <summary>
	/// Draws visual representations of clamp boundaries in the Unity Editor.
	/// </summary>
	private void OnDrawGizmos()
	{
		if (clampPosition)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(clampMin.position, new Vector3(clampMin.position.x, clampMax.position.y, 0f));
			Gizmos.DrawLine(clampMin.position, new Vector3(clampMax.position.x, clampMin.position.y, 0f));
			Gizmos.DrawLine(clampMax.position, new Vector3(clampMin.position.x, clampMax.position.y, 0f));
			Gizmos.DrawLine(clampMax.position, new Vector3(clampMax.position.x, clampMin.position.y, 0f));
		}
	}
}