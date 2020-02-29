using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

	public Vector2 maxMotion = Vector2.one;
	public Vector2 motionSpeed = Vector2.one;
	public float smooth = 2f;

	private Vector3 originalPos;
	private float mX, mY;

	void Start()
	{
		originalPos = transform.localPosition;
	}

	void Update () {

		Vector3 pos = Vector3.zero;
		mX += Time.deltaTime * motionSpeed.x;
		mY += Time.deltaTime * motionSpeed.y;

		pos.x = Mathf.Sin (mX) * maxMotion.x;
		pos.y = Mathf.Sin (mY) * maxMotion.y;
		transform.localPosition = Vector3.Lerp (transform.localPosition, originalPos + pos, Time.deltaTime * smooth);
	}
}
