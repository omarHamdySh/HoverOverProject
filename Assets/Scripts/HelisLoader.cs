using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelisLoader : MonoBehaviour {

	[Header("Scene Exceptions")]
	public string[] sceneNames;

	[Header("Heli Generator")]
	[SerializeField]private GameObject[] helisPrefab;

	public static HelisLoader instance;

	public void Awake()
	{
		if (instance == null) {

			instance = this;

		} else {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}
		
	public void LoadSelectedHeli()
	{
		if (CanLoadInThisScene ()) {

			int heliIndex = SaveLoad.LoadSelectedHeli ();

			if (!PlayerPrefs.HasKey ("SelectedHeli"))
				heliIndex = 2;
			
			print ("Heli(" + heliIndex + ") Generated");
			Transform location = GameObject.FindGameObjectWithTag ("Heli Location").transform;
			GameObject newHeli = Instantiate (helisPrefab [heliIndex], location.position, Quaternion.identity) as GameObject;
			newHeli.GetComponentInChildren<PlayerHeli> ().playerSpawnLocation = location;

		}
	}

	bool CanLoadInThisScene()
	{
		string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
		for (int i = 0; i < sceneNames.Length; i++) {
			if (currentSceneName == sceneNames [i])
				return false;
		}

		return true;
	}
}
