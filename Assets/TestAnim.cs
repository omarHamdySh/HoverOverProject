using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour {

	[System.Serializable]
	public class ObjectAnimation
	{
		public Transform obj;
		public float ySize = 1;
	}

	public AnimationCurve animCurve;
	public float speed = 1;

	public ObjectAnimation[] objects;

	void Start () {

		StartCoroutine (LevelAnimation ());
	}

	IEnumerator LevelAnimation()
	{

		for (int i = 0; i < objects.Length; i++) {
			objects[i].obj.localScale = new Vector3 (objects[i].obj.localScale.x, 0.01f, objects[i].obj.localScale.z);
		}

		float a = 0;

		while (a != 1) {
			a += Time.deltaTime * speed;
			a = Mathf.Clamp (a, 0, 1);

			for (int i = 0; i < objects.Length; i++) {
				float time = animCurve.Evaluate (a) * objects[i].ySize;
				objects[i].obj.localScale = new Vector3 (objects[i].obj.localScale.x, time, objects[i].obj.localScale.z);
			}

			yield return null;
		}
	}
}
