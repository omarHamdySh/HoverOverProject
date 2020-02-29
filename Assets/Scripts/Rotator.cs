using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public Vector3 axis;

	void LateUpdate () {

		transform.Rotate (axis* Time.deltaTime);
	}
}
