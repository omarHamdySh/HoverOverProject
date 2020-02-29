using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisingAwards : MonoBehaviour {

	public static AdvertisingAwards instance;

	public void Awake()
	{
		if (instance == null) {

			instance = this;

		} else {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	[System.Serializable]
	public struct AwardUI
	{
		public GameObject continer;
		public Text text;

		public void Setup(int value)
		{
			continer.SetActive (true);
			text.text = "+" + value.ToString();
		}

		public void Hide()
		{
			continer.SetActive (false);
		}
	}

	public GameObject background;
	public AwardUI coinsAwardUI;
	public AwardUI starsAwardUI;

	public void ShowAward()
	{
		background.SetActive (true);
		CoinAndStarsAward ();
		Invoke ("HideAll", 3f);
	}

	void CoinAndStarsAward()
	{
		int coins = Random.Range (2, 6);
		int stars = Random.Range (1, 4);
		coinsAwardUI.Setup(coins);
		starsAwardUI.Setup(stars);

		int newCoins = SaveLoad.LoadBagCoins () + coins;
		int newStars = SaveLoad.LoadBagStars () + stars;

		print (newCoins);
		print (newStars);

		SaveLoad.SaveBagCoins (newCoins);
		SaveLoad.SaveBagStars (newStars);

		print ("Get Award");
	}

	void HideAll()
	{
		coinsAwardUI.Hide ();
		starsAwardUI.Hide ();
		background.SetActive (false);
	}

}
