using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLook : MonoBehaviour {

	public Point target;

	void LateUpdate () {
		transform.LookAt (target.transform.position);
	}
}
