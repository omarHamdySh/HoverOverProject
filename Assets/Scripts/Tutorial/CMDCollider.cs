using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CMDCollider : MonoBehaviour {

	public bool hideAfterCollide = false;
	public bool applyEventOnCollide = true;

	public UnityEvent onTriggerEvent;

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag (Tags.PLAYER)) {
			if(applyEventOnCollide)
				ApplyEvent ();
			if (hideAfterCollide)
				gameObject.SetActive (false);
		}
	}

	public void ApplyEvent()
	{
		onTriggerEvent.Invoke ();
	}
}
