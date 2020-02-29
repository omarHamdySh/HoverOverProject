using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMenuGenerator : MonoBehaviour {

	public Transform carPrefab;
	public MinMax randomTime;
	public Transform[] points;

	// Use this for initialization
	void Start () {
		StartCoroutine (GenerateCars ());
	}
	
	IEnumerator GenerateCars()
	{
		while (true) {
			yield return new WaitForSeconds (randomTime.GetRandomValue ());
			Transform randPos = points [Random.Range (0, points.Length)];
			GameObject newCar = Instantiate (carPrefab.gameObject, randPos.position, randPos.rotation) as GameObject;
			Destroy (newCar, 10f);
			yield return null;
		}

	}
}
