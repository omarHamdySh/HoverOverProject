using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThreeStarsContiner : MonoBehaviour {

	[System.Serializable]
	public struct StarInfo
	{
		public bool enable;
		public Transform star;
		public Transform failStar;
		public ParticleSystem effect;
	}


	public float speed = 100f;
	public float time = 1f;
	public StarInfo[] stars;

	[Header("Actions")]
	public UnityEvent onStarHit;

	void Update () {
	
		for (int i = 0; i < stars.Length; i++) {
			if (!stars [i].enable)
				continue;
			stars [i].star.position = Vector3.MoveTowards (stars [i].star.position, stars [i].failStar.position, Time.deltaTime * speed);

			if (Vector3.Distance (stars [i].star.position, stars [i].failStar.position) <= 0) {
				stars [i].effect.Play();
				stars [i].enable = false;
				onStarHit.Invoke ();
			}
		}
	}

	public void ShowStars(int count)
	{
		StartCoroutine (ShowStarsCoroutine (count));
	}

	IEnumerator ShowStarsCoroutine(int count)
	{
		for (int i = 0; i < count; i++) {
			stars [i].enable = true;
			yield return new WaitForSeconds (time);
		}
	}
}
