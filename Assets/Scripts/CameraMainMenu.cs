using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMainMenu : MonoBehaviour {

	[System.Serializable]
	public class TranslationSettings
	{
		public string name;
		public Transform target;
		public float rotateSpeed = 10f;
		public float moveSpeed = 10f;
		public UnityEvent onStartTranslation;
		public UnityEvent onEndTranslation;
	}
		
	public bool enabled = true;
	[SerializeField]private TranslationSettings[] translations;

	public void MoveTo(int translationIndex)
	{
		if (!enabled)
			return;
		
		StopCoroutine (MoveToCoroutine (translationIndex));
		StartCoroutine (MoveToCoroutine (translationIndex));
	}

	IEnumerator MoveToCoroutine(int translationIndex)
	{
		TranslationSettings t = translations [translationIndex];
		t.onStartTranslation.Invoke ();

		while (transform.position != t.target.position || transform.rotation != t.target.rotation) {
			
			transform.position = Vector3.MoveTowards (transform.position, t.target.position, Time.deltaTime * t.moveSpeed);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, t.target.rotation, Time.deltaTime * t.rotateSpeed);

			yield return null;
		}
		t.onEndTranslation.Invoke ();
	}
}
