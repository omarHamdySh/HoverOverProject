using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour {

	[Header("Game Timer")]
	[SerializeField]private Text timerText;

	[SerializeField]private bool isSlowTime = false;
	public UnityEvent onEndGameTimer;
	private bool isStart = false;

	[Header("Ready Timer")]
	[SerializeField]private GameObject readyContiner;
	[SerializeField]private Text readyTimerText;
	[SerializeField]private float readyTime = 3;
	[SerializeField]private bool isReady = false;
	public UnityEvent onEndReadyTimer;

	private float levelTime;
	private float currentGameTime;
	private bool isStopTimer = false;
	private bool canStartTimer = false;

	public static Timer instance;

	void Awake()
	{
		instance = this;
	}
	
	void Update () {


		if (GameManager.instance.IsGameOver () || isStopTimer)
			return;

		UpdateReadyTimer ();
		UpdateTimer ();
	}

	public void CanStartTimer()
	{
		canStartTimer = true;
	}

	void UpdateReadyTimer()
	{
		if (isStart)
			return;
		
		if (Input.GetMouseButtonDown (0) && canStartTimer) {
			isReady = true;
		}

		if (isReady) {
			readyTime -= Time.deltaTime;
			int rt = (int)readyTime;
			readyTimerText.text = rt.ToString();

			if (readyTime <= 1) {
				isStart = true;
				isReady = false;
				readyTime = 0;
				onEndReadyTimer.Invoke ();
				readyContiner.SetActive (false);
			}
		}
	}

	void UpdateTimer()
	{
		if (!isStart)
			return;

		if(!isSlowTime)
			currentGameTime -= Time.deltaTime;
		else
			currentGameTime -= Time.deltaTime / 5;

		if (currentGameTime <= LevelInfo.instance.GetTargetTime())
			timerText.color = Color.red;
		else
			timerText.color = Color.white;

		currentGameTime = Mathf.Clamp (currentGameTime, 0, Mathf.Infinity);

		UpdateTimerText ();

		if (currentGameTime <= 0) {
			onEndGameTimer.Invoke ();
			GameManager.instance.GameOver ();
			isStart = false;
		}
	}

	void UpdateTimerText()
	{
		timerText.text = currentGameTime.ToString("0") + "<size=20>s</size> / " + LevelInfo.instance.GetTargetTime();
	}

	public float GetCurrentTime()
	{
		return currentGameTime;
	}

	public void StartTimer()
	{
		isStart = true;
	}

	public void SlowTime()
	{
		StartCoroutine (SlowTimeCoroutine (0.1f));
	}

	public void AddExtraTime()
	{
		currentGameTime += 10;
	}


	IEnumerator SlowTimeCoroutine(float time)
	{
		isSlowTime = true;
		yield return new WaitForSeconds (time);
		isSlowTime = false;
	}

	public void PushTimer()
	{
		isStart = false;
	}

	public void Stop()
	{
		isStopTimer = true; 
	}

	public bool IsReadyToPlay()
	{
		return isStart;
	}

	public void SetLevelTime(float time)
	{
		levelTime = time;
		currentGameTime = levelTime;
		UpdateTimerText ();
	}
}
