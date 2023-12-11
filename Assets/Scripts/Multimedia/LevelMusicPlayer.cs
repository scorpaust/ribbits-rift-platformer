using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicPlayer : MonoBehaviour
{
    [Header("Track Info")]
    [Tooltip("Music to play on this specific level")]
    [SerializeField] private int trackToPlay;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayLevelMusic(trackToPlay);
    }
}
