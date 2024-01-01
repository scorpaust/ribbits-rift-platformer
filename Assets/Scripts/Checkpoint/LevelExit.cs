using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private Animator anim;

	private bool isEnding;

	[SerializeField]
	private float waitToEndLevel = 4f;

	[SerializeField]
	private string nextLevel;

	[SerializeField]
	private GameObject blocker;

	[SerializeField]
	private float fadeTime = 1.5f;

	private void Start()
	{
		anim = GetComponentInChildren<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!isEnding)
		{
			if (other.CompareTag("Player"))
			{
				isEnding = true;

				anim.SetTrigger("Ended");

				blocker.SetActive(true);

				AudioManager.Instance.PlayLevelCompleteMusic();

				StartCoroutine(EndLevelCo());
			}
		}		
	}

	private IEnumerator EndLevelCo()
	{
		yield return new WaitForSeconds(waitToEndLevel - fadeTime);

		UIController.Instance.FadeToBlack();

		yield return new WaitForSeconds(fadeTime);

		SceneManager.LoadScene(nextLevel);
	}
}
