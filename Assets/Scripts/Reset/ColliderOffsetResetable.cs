using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderOffsetResetable : MonoBehaviour {


	public BoxCollider collider;

	public Vector3[] offset;


	public void ApplyReset (int index) {
		collider.center = offset [index];
	}
}
