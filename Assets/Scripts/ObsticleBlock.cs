using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObsticleBlock : MonoBehaviour {

	[SerializeField]private bool isEnable = true;

	public void SetEnable(bool value)
	{
		isEnable = value;
		GetComponent<Collider> ().enabled = value;
	}

	public bool IsEnable()
	{
		return isEnable;
	}
}
