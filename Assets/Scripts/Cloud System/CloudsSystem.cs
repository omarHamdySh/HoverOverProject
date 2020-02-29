using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsSystem : MonoBehaviour {

	public Cloud[] clouds;

	[Header("General")]
	public MinMax randomTime = new MinMax(-5,5);

	[Header("Clouds Position")]
	public float randomRightPos = 4;
	public float randomUpPos = 4;
	public float randomForwardPos = 4;

	void Start()
	{
		StartCoroutine (GenerateClouds());
	}

	IEnumerator GenerateClouds()
	{
		while (IsRun()) {

			Vector3 radnomPos = transform.position + transform.right * Random.Range (-randomRightPos, randomRightPos) / 2 +
			transform.up * Random.Range (-randomUpPos, randomUpPos) / 2 + transform.forward * Random.Range (-randomForwardPos, randomForwardPos) / 2;

			Quaternion radnomRot = Quaternion.Euler (0, Random.Range (-180, 180), 0);

			Cloud c = Instantiate (clouds [Random.Range (0, clouds.Length)], radnomPos,radnomRot, this.transform) as Cloud;
			c.SetDirection (transform.forward);
			yield return new WaitForSeconds (randomTime.GetRandomValue());
		}
	}

	bool IsRun()
	{
		if (GameManager.instance != null)
			return !GameManager.instance.IsGameOver () || !GameManager.instance.IsGameCompleted ();

		return true;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube (transform.position, transform.right * randomRightPos + transform.up * randomUpPos+ transform.forward * randomForwardPos);
	}
}
