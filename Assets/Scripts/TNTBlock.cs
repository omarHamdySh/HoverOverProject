using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTBlock : BlockBase, IDestroyableObjects {

	public void Destruction()
	{
		GetComponentInChildren<TNTCollisionInfo> ().ApplyDestroyEffect ();
		Destroy (gameObject);
	}
}
