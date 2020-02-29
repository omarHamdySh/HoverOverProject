using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

	private MeshRenderer mesh;
	private float a = 0;
	public float maxAlpha = 0.5f;
	private bool flip =false;
	private Vector3 direction;

	void Start()
	{
		mesh = GetComponent<MeshRenderer> ();
		mesh.material.color = new Color(1,1,1,a);
	}
		
	void LateUpdate() {

		transform.position += direction * Time.deltaTime; 

		if(!flip)
			a += Time.deltaTime / 15;
		else
			a -= Time.deltaTime / 15;

		a = Mathf.Clamp (a,0,maxAlpha);
		mesh.material.color = new Color(1,1,1,a);

		if (a >= maxAlpha)
			flip = true;
		if (a <= 0 && flip)
			Destroy (gameObject);
	}

	public void SetDirection(Vector3 dir)
	{
		direction = dir;
	}
}
