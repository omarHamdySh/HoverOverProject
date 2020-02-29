using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour {

	public static PlayerBag instance;

	public int coins { get; private set;}
	public int stars { get; private set;}
	public int gems { get; private set;}


	void OnValidate()
	{
		if (instance == null)
			instance = this;
	}

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		coins = PlayerPrefs.GetInt (GetCoinsID());
		stars = PlayerPrefs.GetInt (GetStarsID());

		print ("Collected Coins: " + PlayerPrefs.GetInt (GetCoinsID ()));
		print ("Collected Stars: " + PlayerPrefs.GetInt (GetStarsID ()));

		FindObjectOfType<PlayerBagUI> ().UpdateBagUI ();
	}

	public string GetCoinsID()
	{
		return LevelInfo.instance.GetID () + LevelInfo.levelCoinsKey;
	}

	public string GetStarsID()
	{
		return LevelInfo.instance.GetID () + LevelInfo.levelStarsKey;
	}

	public void DeleteData(){
		PlayerPrefs.DeleteKey (GetCoinsID ());
		PlayerPrefs.DeleteKey (GetStarsID ());
	}

	public void AddCoin()
	{
		coins++;
	}

	public void AddStar()
	{
		stars++;
	}

	public void AddGem()
	{
		gems++;
	}
}
