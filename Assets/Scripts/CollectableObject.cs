using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour {

	public ParticleSystem collectEffect;
	public Vector3 offset;
	public bool canSaveLoad = false;

	[Header("Audio Settings")]
	public AudioClip clip;
	public float pitch = 1f;
	public float volume = 1f;

	void Start()
	{
		if (SaveLoad.LoadCollectedObject (GetID ()) == "Collected")
			Destroy (gameObject);
	}

	public string UID()
	{
		return transform.position.ToString ();
	}

	public string GetID()
	{
		return LevelInfo.instance.GetID() + LevelInfo.collectedObjectKey + UID();
	}

	public string GetCollectedStarsID()
	{
		return PlayerBag.instance.GetStarsID();
	}

	public string GetCollectedCoinsID()
	{
		return PlayerBag.instance.GetCoinsID();
	}

	public void DeleteProgress()
	{
		if (gameObject.tag == "Coin") {
		
			int coinsInBag = SaveLoad.LoadBagCoins () - 1;
			if (coinsInBag <= 0)
				coinsInBag = 0;
			
			SaveLoad.SaveBagCoins (coinsInBag);
		}
		if (gameObject.tag == "Star") {

			int starsInBag = SaveLoad.LoadBagStars () - 1;

			if (starsInBag <= 0)
				starsInBag = 0;
			
			SaveLoad.SaveBagStars (starsInBag);
		}

		PlayerPrefs.DeleteKey (GetCollectedCoinsID ());
		PlayerPrefs.DeleteKey (GetCollectedStarsID ());
		PlayerPrefs.DeleteKey (GetID ());
	}

	public void Collect()
	{
		if(collectEffect != null)
			Instantiate (collectEffect, transform.position + offset, Quaternion.identity);
		if (clip != null)
			AudioManager.instance.PlayCollectAudio (clip, pitch, volume);
		
		PlayerBagUI.instance.UpdateBagUI ();

		if (canSaveLoad) {
			
			UpdateSaveLoad ();
		}

		Destroy (gameObject);
	}

	void UpdateSaveLoad()
	{
		PlayerPrefs.SetString(GetID(), "Collected");
		print ("Collected Object Saved: " + name  +  UID());


		if (gameObject.tag == "Coin") {
			print ("Bag Coins Saved");

			int newCoin = PlayerPrefs.GetInt (GetCollectedCoinsID ()) + 1;
			PlayerPrefs.SetInt (GetCollectedCoinsID (), newCoin);

			int maxCoins = SaveLoad.LoadBagCoins () + 1;
			SaveLoad.SaveBagCoins (maxCoins);

		} else if (gameObject.tag == "Star") {
			print ("Bag Star Saved");

			int newStar = PlayerPrefs.GetInt (GetCollectedStarsID ()) + 1;
			PlayerPrefs.SetInt (GetCollectedStarsID (), newStar);

			int maxStars = SaveLoad.LoadBagStars () + 1;
			SaveLoad.SaveBagStars (maxStars);
		}
	}
}
