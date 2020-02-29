using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class VisibleBlockObsticle : MonoBehaviour, IDestroyableObjects {

	public ParticleSystem demolitionEffect;
	public UnityEvent onDemolitionEvent;

	public void Destruction()
	{
		onDemolitionEvent.Invoke ();
		Instantiate (demolitionEffect, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
