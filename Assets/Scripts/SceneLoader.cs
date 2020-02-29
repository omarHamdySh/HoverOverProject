using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {


	public void LoadScene(int index)
	{
		if (index < SceneManager.sceneCountInBuildSettings)
			SceneManager.LoadScene (index);
		else
			Debug.LogError ("This level is not exist!");
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void LoadNextScene()
	{
		int nextScene = SceneManager.GetActiveScene ().buildIndex + 1;
		if (nextScene < SceneManager.sceneCountInBuildSettings)
			SceneManager.LoadScene (nextScene);
		else
			SceneManager.LoadScene (0);
	}
}
