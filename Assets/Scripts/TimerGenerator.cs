using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimerGenerator : MonoBehaviour {

	public Image imgFill;
	public float maxTime = 10f;
	public bool isStart = false;

	[Header("Genetare Settings")]
	public bool enableGenerate = true;
	public GameObject objPrefab;
	public Transform point;
	public Transform parent;

	[Header("Actions")]
	public UnityEvent onEndTimeEvent;

	private float currentTime = 0;
	private GameObject lastObject;

	void Start()
	{
		currentTime = 0;
		imgFill.fillAmount = currentTime / maxTime;
	}

	void Update () {

		if (!isStart)
			return;

		if (lastObject != null)
			return;

		currentTime += Time.deltaTime;
		currentTime = Mathf.Clamp (currentTime, 0, maxTime);

		imgFill.fillAmount = currentTime / maxTime;

		if (currentTime >= maxTime) {
			GenerateObject ();
			onEndTimeEvent.Invoke ();
		}
	}

	void GenerateObject()
	{
		if (!enableGenerate)
			return;
		if(lastObject != null)
			return;

		lastObject = Instantiate (objPrefab, point.position, point.rotation, parent);
		RestartTimer ();
		UpdateFill ();
	}

	void UpdateFill()
	{
		imgFill.fillAmount = currentTime / maxTime;
	}

	public void RestartTimer()
	{
		isStart = true;
		currentTime = 0;
	}
}
