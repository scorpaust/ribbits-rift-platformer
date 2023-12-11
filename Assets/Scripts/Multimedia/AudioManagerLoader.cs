using UnityEngine;

/// <summary>
/// Responsible for loading the AudioManager instance when needed.
/// Ensures that an instance of AudioManager exists in the scene.
/// </summary>
public class AudioManagerLoader : MonoBehaviour
{
	[Header("Audio Manager Prefab")]
	[Tooltip("Prefab of the AudioManager to instantiate if it's not already present in the scene.")]
	[SerializeField] private AudioManager audioManagerPrefab;

	/// <summary>
	/// Checks if an AudioManager instance exists and instantiates one if not.
	/// </summary>
	private void Awake()
	{
		if (AudioManager.Instance == null && audioManagerPrefab != null)
		{
			Instantiate(audioManagerPrefab).SetupAudioManager();
		}
	}
}
