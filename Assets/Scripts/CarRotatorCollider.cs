using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRotatorCollider : MonoBehaviour {

	public float angle = -20f;

	public void UpdateCollider()
	{
		GetComponent<Collider> ().enabled = false;
		Invoke ("ActiveCollider", 5);
	}

	void ActiveCollider()
	{
		GetComponent<Collider> ().enabled = false;
	}
}
