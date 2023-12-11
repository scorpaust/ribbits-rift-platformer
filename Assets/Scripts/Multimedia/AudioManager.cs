using UnityEngine;

/// <summary>
/// Manages all audio aspects of the game, including music and sound effects (SFX).
/// </summary>
public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	[Header("Audio Sources")]
	[Tooltip("Audio source for menu music.")]
	[SerializeField] private AudioSource menuMusic;
	[Tooltip("Audio source for boss music.")]
	[SerializeField] private AudioSource bossMusic;
	[Tooltip("Audio source for level completion music.")]
	[SerializeField] private AudioSource levelCompleteMusic;
	[Tooltip("Array of audio sources for level tracks.")]
	[SerializeField] private AudioSource[] levelTracks;
	[Tooltip("Array of all sound effects.")]
	[SerializeField] private AudioSource[] allSFX;

	/// <summary>
	/// Ensures a single instance of the AudioManager and sets it to persist between scenes.
	/// </summary>
	private void Awake()
	{
		if (Instance == null)
		{
			SetupAudioManager();
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void SetupAudioManager()
	{
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	/// <summary>
	/// Stops all music currently playing.
	/// </summary>
	private void StopMusic()
	{
		menuMusic.Stop();
		bossMusic.Stop();
		levelCompleteMusic.Stop();

		foreach (AudioSource track in levelTracks)
		{
			track.Stop();
		}
	}

	public void PlayMenuMusic()
	{
		StopMusic();
		menuMusic.Play();
	}

	public void PlayBossMusic()
	{
		StopMusic();
		bossMusic.Play();
	}

	public void PlayLevelCompleteMusic()
	{
		StopMusic();
		levelCompleteMusic.Play();
	}

	/// <summary>
	/// Plays a specific level track.
	/// </summary>
	/// <param name="trackToPlay">Index of the track to play.</param>
	public void PlayLevelMusic(int trackToPlay)
	{
		StopMusic();
		if (trackToPlay >= 0 && trackToPlay < levelTracks.Length)
		{
			levelTracks[trackToPlay].Play();
		}
	}

	/// <summary>
	/// Plays a sound effect with optional pitch variation.
	/// </summary>
	/// <param name="sfxToPlay">Index of the sound effect to play.</param>
	/// <param name="pitched">Whether to apply pitch variation.</param>
	public void PlaySFX(int sfxToPlay, bool pitched)
	{
		if (sfxToPlay >= 0 && sfxToPlay < allSFX.Length)
		{
			var sfx = allSFX[sfxToPlay];
			sfx.Stop();

			if (pitched)
			{
				sfx.pitch = Random.Range(0.75f, 1.25f);
			}

			sfx.Play();
		}
	}
}
