using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

//	public BlocksHolder staticHolder;
//	public BlocksHolder dynamicHolder;
	public Timer gameTimer;
	public LayerMask hitLayer;
	private PlayerHeli player;
	private CameraHover cam;
	private Camera mainCam;

	[Header("Menus Camera")]
	public Canvas[] menusCam;

	[Header("Actions")]
	public UnityEvent onLevelCompletedEvent;
	public UnityEvent onGameOverEvent;
	public UnityEvent onReviveEvent;
	public UnityEvent onStartButton;

	private Car[] allCars;
	private bool isPlaying = false;
	private bool isGameComplete = false;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		HelisLoader.instance.LoadSelectedHeli ();
		cam = FindObjectOfType<CameraHover> ();
		mainCam = Camera.main;
		player = FindObjectOfType<PlayerHeli> ();

		SetupMenusCam ();
	}

	public void OnStartButtonAction()
	{
		onStartButton.Invoke ();
	}

	void SetupMenusCam()
	{
		for (int i = 0; i < menusCam.Length; i++) {
			menusCam [i].worldCamera = mainCam;
		}
	}

	public void PlayerHoverInCamera()
	{
		if (cam == null)
			return;
		
		cam.PlayHoverIn ();
	}

	public void PlayerHoverOutCamera()
	{
		if (cam == null)
			return;
		
		cam.PlayHoverOut ();
	}

	public void ReloadLevel()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void LoadScene(int i)
	{
		SceneManager.LoadScene (i);
	}

	public void OnLevelCompleted()
	{
		AudioManager.instance.PlaySoundByName ("Win");
		onLevelCompletedEvent.Invoke ();
		isGameComplete = true;
	}

	public void RevivePlayer()
	{
		isPlaying = false;
		onReviveEvent.Invoke ();
		player.gameObject.SetActive (true);
		player.Revive ();
	}

	public bool IsGameCompleted()
	{
		return isGameComplete;
	}

	public void GameOver()
	{
		onGameOverEvent.Invoke ();
		Stop ();
	}

	public void UpdateCompletedCars()
	{
		bool allCompleted = true;

		for (int i = 0; i < allCars.Length; i++) {
			if (!allCars [i].IsCompleted ())
				allCompleted = false;
		}

		if (allCompleted) {
			OnLevelCompleted ();
		}
	}

	public void Play()
	{
		if (isPlaying)
			return;


		GameObject holder = GameObject.FindGameObjectWithTag (Tags.STATIC_HOLDER);

		Point[] allPoints = holder.GetComponentsInChildren<Point> ();
		foreach (Point p in allPoints) {
			p.Connect (hitLayer.value);
		}


		isPlaying = true;

		allCars = FindObjectsOfType<Car> ();
		for (int i = 0; i < allCars.Length; i++) {
			if (allCars [i] == null)
				return;
			allCars [i].Move (true);
		}
	}

	public void Stop()
	{
		allCars = FindObjectsOfType<Car> ();
		for (int i = 0; i < allCars.Length; i++) {
			if (allCars [i] == null)
				return;
			allCars [i].Move (false);
		}
	}

	public void PushGame()
	{
		Time.timeScale = 0;
	}


	public void ResumeGame()
	{
		Time.timeScale = 1;
	}

	public bool IsGameOver()
	{
		if (player == null)
			return false;

		return !player.gameObject.activeSelf;
	}

	public bool IsGamePlaying()
	{
		return isPlaying;
	}
}
