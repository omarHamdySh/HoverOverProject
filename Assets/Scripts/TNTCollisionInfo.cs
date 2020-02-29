using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TNTCollisionInfo : MonoBehaviour {

	public ParticleSystem explosionEffect;
	public float destroyRange = 3f;

	private Rigidbody rb;
	private Vector3 pos;

	void Start () {

		rb = transform.parent.GetComponent<Rigidbody> ();
		pos = transform.localPosition;
	}

	void Update()
	{
		transform.localPosition = pos;
		transform.localRotation = Quaternion.identity;
	}

	void OnCollisionEnter(Collision col)
	{
		if (rb == null)
			return;
		if (rb.velocity.magnitude > 3f) {
			DestroyObjectsInsideRange ();
			ApplyDestroyEffect ();
		}
	}

	public void ApplyDestroyEffect()
	{
		ParticleSystem newEffect = Instantiate (explosionEffect, GetPosition(), Quaternion.identity) as ParticleSystem;
		newEffect.Play ();
		Destroy (transform.parent.gameObject);
	}

	void DestroyObjectsInsideRange()
	{
		Collider[] cols = Physics.OverlapSphere (GetPosition (), destroyRange);
		for (int i = 0; i < cols.Length; i++) {
			if (cols [i].gameObject == transform.parent.gameObject)
				continue;
			
			if (cols [i].GetComponent<IDestroyableObjects> () != null)
				cols [i].GetComponent<IDestroyableObjects> ().Destruction ();
		}
	}

	Vector3 GetPosition()
	{
		return transform.position + Vector3.up * 0.5f;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (GetPosition (), destroyRange);
	}
}
