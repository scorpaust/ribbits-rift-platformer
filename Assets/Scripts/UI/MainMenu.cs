using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Properties")]
    [Tooltip("The first level to be loaded when we start the game")]
    [SerializeField]
    private string firstLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	/// <summary>
	/// Start the game, loading the first level.
	/// </summary>
	public void StartGame()
    {
		SceneManager.LoadScene(firstLevel);
	}

	/// <summary>
	/// Closes the game. In the Unity Editor, it stops the play mode. In a production build, it quits the application.
	/// </summary>
	public void QuitGame()
	{
        #if UNITY_EDITOR
		        // Stop the game if running in the Unity Editor
		        EditorApplication.isPlaying = false;
        #else
                // Quit the game if running in a production build
                Application.Quit();
        #endif
	}
}
