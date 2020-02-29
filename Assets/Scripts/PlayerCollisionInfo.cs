using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionInfo : MonoBehaviour {

	public Transform playerMesh;
	public ParticleSystem explosionEffect;
	public ParticleSystem smokeEffect;
	public AudioClip explosionClip;

	private PlayerHeli player;

	void Start () {

		player = GetComponent<PlayerHeli> ();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag (Tags.DESTROY) || col.gameObject.CompareTag (Tags.GROUND) || col.gameObject.CompareTag (Tags.CAR)) {

			if (!player.IsPlayerDestroied()) {
				ParticleSystem newEffect = Instantiate (explosionEffect, transform.position, Quaternion.identity) as ParticleSystem;
				AudioManager.instance.PlayExplosionClip (explosionClip);

				newEffect.Play ();
				smokeEffect.Play ();
				player.reverseAxis = -playerMesh.forward;

				GetComponent<PlayerHeli> ().PlayerExploded ();
				GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
				GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotationX;
				GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotationZ;	
				GameManager.instance.Stop ();
				Invoke ("ApplyFinalDestroy", 2f);
			}

		}
		else if (player.IsPlayerDestroied()) {
			if (col.gameObject.layer == LayerMask.NameToLayer ("Block")) {
				CancelInvoke ("ApplyFinalDestroy");
				ApplyFinalDestroy ();
			}
		}

		if (col.gameObject.CompareTag (Tags.STAR)) {
			
			PlayerBag.instance.AddStar ();
			PlayerBagUI.instance.InstantiateItem (PlayerBagUI.instance.items[0], col.transform.position);
			col.gameObject.GetComponent<CollectableObject> ().Collect ();

		}
	}

	void ApplyFinalDestroy()
	{
		GameManager.instance.GameOver ();
		gameObject.SetActive (false);
		ParticleSystem newEffect = Instantiate (explosionEffect, transform.position, Quaternion.identity) as ParticleSystem;
		AudioManager.instance.PlayExplosionClip (explosionClip);
		return;
	}
}
