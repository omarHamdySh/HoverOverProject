using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuWheel : MonoBehaviour {

	[System.Serializable]
	public class WheelSettings
	{
		public string name;
		public float targetPos = 30;
		public CanvasGroup menuCanvasGroup;
		public UnityEvent onStartTranslation;
		public UnityEvent onEndTranslation;
	}

	public float transationSpeed = 10f;
	public WheelSettings[] settings;
	private WheelSettings currentTarget;

	void Start()
	{
		currentTarget = settings [0];
	}

	public void RotateTo(int translationIndex)
	{
		if (!enabled)
			return;

		StopAllCoroutines ();
		StartCoroutine (RotateToCoroutine (translationIndex));
	}

	IEnumerator RotateToCoroutine(int translationIndex)
	{
		currentTarget = settings [translationIndex];
		currentTarget.onStartTranslation.Invoke ();

		while (Mathf.Abs(transform.position.x - currentTarget.targetPos) > 0.1f) {
			transform.position = Vector3.Lerp (transform.position, new Vector3 (currentTarget.targetPos,transform.position.y , transform.position.z), transationSpeed * Time.deltaTime);

			yield return null;
		}

		currentTarget.onEndTranslation.Invoke ();
	}

}
