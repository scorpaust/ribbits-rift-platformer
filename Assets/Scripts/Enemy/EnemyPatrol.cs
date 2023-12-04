using UnityEngine;

/// <summary>
/// Manages the patrol behavior of an enemy, allowing it to move between predefined points and pause at each.
/// </summary>
public class EnemyPatrol : MonoBehaviour
{
	[Header("Patrol Configuration")]
	[Tooltip("Array of waypoints for the patrol route.")]
	[SerializeField] private Transform[] patrolPoints;
	[Tooltip("Movement speed of the enemy.")]
	[SerializeField] private float moveSpeed;
	[Tooltip("Time spent waiting at each patrol point.")]
	[SerializeField] private float timeAtPoints;

	private float waitCounter;
	private int currentPoint;
	private Animator anim;

	private EnemyController ec;

	/// <summary>
	/// Start is called before the first frame update.
	/// Initializes patrol points and sets up the animator.
	/// </summary>
	private void Start()
	{
		ec = GetComponent<EnemyController>();
		InitializePatrolPoints();
		waitCounter = timeAtPoints;
		anim = GetComponent<Animator>();
		anim.SetBool("IsMoving", true);
	}

	/// <summary>
	/// Update is called once per frame.
	/// Manages the patrol movement and waiting behavior.
	/// </summary>
	private void Update()
	{
		// If the enemy is defeated disable its movement
		if (ec.IsDefeated)
		{
			return;
		}
		
		MoveTowardsNextPoint();

		if (IsAtPatrolPoint())
		{
			ManageWaitAtPoint();
		}
	}

	/// <summary>
	/// Detaches patrol points from the enemy's transform and initializes them.
	/// </summary>
	private void InitializePatrolPoints()
	{
		foreach (Transform point in patrolPoints)
		{
			point.SetParent(null);
		}
	}

	/// <summary>
	/// Moves the enemy towards the next patrol point.
	/// </summary>
	private void MoveTowardsNextPoint()
	{
		transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, moveSpeed * Time.deltaTime);
	}

	/// <summary>
	/// Checks if the enemy has reached a patrol point.
	/// </summary>
	/// <returns>True if the enemy is within a small distance of the patrol point.</returns>
	private bool IsAtPatrolPoint()
	{
		return Vector3.Distance(transform.position, patrolPoints[currentPoint].position) < 0.01f;
	}

	/// <summary>
	/// Manages the waiting behavior and transitions between patrol points.
	/// </summary>
	private void ManageWaitAtPoint()
	{
		waitCounter -= Time.deltaTime;
		anim.SetBool("IsMoving", false);

		if (waitCounter <= 0)
		{
			UpdatePatrolPoint();
			waitCounter = timeAtPoints;
			anim.SetBool("IsMoving", true);
			UpdateOrientation();
		}
	}

	/// <summary>
	/// Updates the current patrol point index, looping back if necessary.
	/// </summary>
	private void UpdatePatrolPoint()
	{
		currentPoint = (currentPoint + 1) % patrolPoints.Length;
	}

	/// <summary>
	/// Updates the enemy's orientation based on the direction to the next patrol point.
	/// </summary>
	private void UpdateOrientation()
	{
		if (transform.position.x < patrolPoints[currentPoint].position.x)
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			transform.localScale = Vector3.one;
		}
	}
}

