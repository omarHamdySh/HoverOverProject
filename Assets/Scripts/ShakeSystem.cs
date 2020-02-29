using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeSystem : MonoBehaviour {

	public Vector2 maxShake = Vector2.one;
	public float shakeSpeed = 1f;

	private Vector3 originalPos;
	private float randomX;
	private float randomY;
	private float a = 0;

	void Start()
	{
		originalPos = transform.position;
	}

	public void ApplyShake()
	{
		//StopCoroutine (StarShakeCoroutine ());
		StartCoroutine (StarShakeCoroutine ());
	}

	IEnumerator StarShakeCoroutine()
	{
		a = 1;
		while (a > 0) {
			a -= Time.deltaTime * shakeSpeed;

			randomX = Random.Range (-maxShake.x, maxShake.x);
			randomY = Random.Range (-maxShake.y, maxShake.y);
			transform.position = originalPos + new Vector3 (randomX * a, randomY * a, 0);

			yield return null;
		}
	}
}
