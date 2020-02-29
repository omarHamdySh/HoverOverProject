using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpot : MonoBehaviour {

	private PlayerHeli player;
	public Vector3 offset;

	public void StartFollow () {

		player = FindObjectOfType<PlayerHeli> ();
	}
	
	void FixedUpdate () {
		if (player == null)
			return;
		
		if (GameManager.instance.IsGameCompleted () || GameManager.instance.IsGameOver () || GameManager.instance.IsGamePlaying ()) {
			transform.GetChild (0).gameObject.SetActive (false);
			enabled = false;
		}

		transform.position = new Vector3 (player.transform.position.x, 0, player.transform.position.z) + offset;
	}

	public void Play()
	{
		enabled = true;
		transform.GetChild (0).gameObject.SetActive (true);
	}
}
