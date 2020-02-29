using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour {

	[SerializeField]private string levelID = "b_lv";
	[SerializeField]private int targetCoins;
	[SerializeField]private int targetStars;
	[SerializeField]private float targetTime = 60f;
	[SerializeField]private float levelTime = 60f;
	[SerializeField]private bool deleteLevelProgress = false;

	[Header("Player Limits")]
	public Vector2 position;
	public Vector2 limitSize = Vector2.one * 100f;
	public static LevelInfo instance;

	public const string threeStarsKey = "Three Stars";
	public const string competedLevelKey = "Completed Level";
	public const string collectedObjectKey = "CollectedObject";
	public const string levelCoinsKey = "Coins";
	public const string levelStarsKey = "Stars";

	void OnValidate()
	{
		if (instance == null)
			instance = this;
	}

	void Awake()
	{
		instance = this;
	}

	void Start () {

		Timer.instance.SetLevelTime (levelTime);
		if (deleteLevelProgress) {

			DeleteAllProgress ();
		}

		targetCoins = GameObject.FindGameObjectsWithTag ("Coin").Length;
		targetStars = GameObject.FindGameObjectsWithTag ("Star").Length;

		GameManager.instance.onLevelCompletedEvent.AddListener (OnCompletedLevel);

		PrintLevelInfo ();
	}

	public void PrintLevelInfo()
	{
		print("Level State: " + PlayerPrefs.GetString(GetLevelCompleteID()));
		print("Got Stars : " + PlayerPrefs.GetInt(Get3StarsID()));
		print ("Bag Coins " + SaveLoad.LoadBagCoins ());
		print ("Bag Stars " + SaveLoad.LoadBagStars ());
		print("---------------------------------------------------");
	}

	public void DeleteAllProgress()
	{
		GameObject[] allCoinsObj = GameObject.FindGameObjectsWithTag ("Coin");
		GameObject[] allStarsObj = GameObject.FindGameObjectsWithTag ("Star");

		for (int i = 0; i < allCoinsObj.Length; i++) {
			allCoinsObj [i].GetComponent<CollectableObject> ().DeleteProgress ();
		}

		for (int i = 0; i < allStarsObj.Length; i++) {
			allStarsObj [i].GetComponent<CollectableObject> ().DeleteProgress ();
		}

		PlayerPrefs.DeleteKey (Get3StarsID());
		PlayerPrefs.DeleteKey (GetLevelCompleteID());
	}

	public string GetID()
	{
		return levelID + UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex.ToString();
	}

	string GetLevelCompleteID()
	{
		return GetID () + competedLevelKey;
	}

	string Get3StarsID()
	{
		return GetID () + threeStarsKey;
	}

	void OnCompletedLevel()
	{
		PlayerPrefs.SetString (GetLevelCompleteID (), "Completed");

		int maxStars = Calculate3Stars ();
		int loadedStars = PlayerPrefs.GetInt (Get3StarsID());

		if(maxStars > loadedStars)
			PlayerPrefs.SetInt (Get3StarsID(), Calculate3Stars ());
	}

	int Calculate3Stars()
	{
		if (PlayerBag.instance.coins >= targetCoins && PlayerBag.instance.stars >= targetStars && Timer.instance.GetCurrentTime () >= targetTime)
			return 3;
		else if (PlayerBag.instance.coins >= targetCoins && PlayerBag.instance.stars >= targetStars)
			return 2;
		else if (PlayerBag.instance.coins >= targetCoins && Timer.instance.GetCurrentTime () >= targetTime)
			return 2;
		else if (PlayerBag.instance.stars >= targetStars && Timer.instance.GetCurrentTime () >= targetTime)
			return 2;
		
		return 1;
	}

	public int Get3Stars()
	{
		return PlayerPrefs.GetInt (Get3StarsID ());
	}

	public Vector2 GetMinLimit()
	{
		return position - (limitSize / 2);
	}

	public Vector2 GetMaxLimit()
	{
		return position + (limitSize / 2);
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (new Vector3(position.x, transform.position.y, position.y), new Vector3(limitSize.x, 3, limitSize.y));
		Vector3 pos = new Vector3 (GetMaxLimit ().x, 3, GetMaxLimit ().y);
		Gizmos.DrawSphere (pos, 1);
	}

	public float GetTargetTime()
	{
		return targetTime;
	}
}
