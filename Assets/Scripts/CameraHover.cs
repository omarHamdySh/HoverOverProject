using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraHover : MonoBehaviour {

	[SerializeField]private float hoverfrom = 5;
	[SerializeField]private float hoverTo = 100;
	[SerializeField]private float hoverSpeed = 1;
	public bool playOnAwake = true;

	[Header("Actions")]
	public UnityEvent onEndHoverIn;
	public UnityEvent onEndHoverOut;
	private Camera cam;
	private bool isHover = false;

	void Awake()
	{
		cam = Camera.main;
		cam.nearClipPlane = hoverfrom;

		PlayHoverIn ();
	}

	public void PlayHoverIn()
	{
		if (isHover)
			return;
		StopAllCoroutines ();

		if(playOnAwake)
			StartCoroutine (HoverIn ());
	}

	public void PlayHoverOut()
	{
		if (isHover)
			return;
		
		StopAllCoroutines ();

		StartCoroutine (HoverOut ());
	}

	IEnumerator HoverOut()
	{
		isHover = true;
		cam.nearClipPlane = hoverTo;	
		while (cam.nearClipPlane != hoverfrom) {
			cam.nearClipPlane = Mathf.MoveTowards(cam.nearClipPlane, hoverfrom, Time.deltaTime * hoverSpeed);
			yield return null;
		}
		onEndHoverOut.Invoke ();
		isHover = false;
		yield break;
	}

	IEnumerator HoverIn()
	{
		isHover = true;
		cam.nearClipPlane = hoverfrom;	
		while (cam.nearClipPlane != hoverTo) {
			cam.nearClipPlane = Mathf.MoveTowards(cam.nearClipPlane, hoverTo, Time.deltaTime * hoverSpeed);
			yield return null;
		}
		onEndHoverIn.Invoke ();

		GameObject.Find ("Canvas Standard").GetComponent<Canvas> ().enabled = true;
		GameObject.Find ("Run").SetActive (true);

		isHover = false;
		yield break;
	}
}
