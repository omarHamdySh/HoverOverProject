using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMenuMovement : MonoBehaviour {

	public float speed = 10f;

	void LateUpdate () {

		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
