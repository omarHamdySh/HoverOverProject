using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour {

	public static SaveLoad instance;

	public void Awake()
	{
		if (instance == null) {

			instance = this;

		} else {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		//DeleteAllData ();
		Load ();
	}

	//public bool deleteAll = false;
	//public bool deleteSettingsData = false;

	private int maxCoins;
	private int maxStars;
	private int selectedHeliIndex = 0;

	public static void DeleteAllData()
	{
//		if (deleteAll) {
//			PlayerPrefs.DeleteAll ();
//			print ("All Progress has been deleted");
//		}
//		if (deleteSettingsData) {
//			DeleteSettingsData ();
//			print ("All Settings data has been deleted");
//		}

		PlayerPrefs.DeleteAll ();
	}

	public void Load()
	{
		maxCoins = PlayerPrefs.GetInt ("Coins");
		maxStars = PlayerPrefs.GetInt ("Stars");
		selectedHeliIndex = PlayerPrefs.GetInt ("SelectedHeli");
	}

	public static int LoadBagCoins()
	{
		return PlayerPrefs.GetInt ("Coins");
	}

	public static int LoadBagStars()
	{
		return PlayerPrefs.GetInt ("Stars");
	}

	public static void SaveBagCoins(int count)
	{
		PlayerPrefs.SetInt ("Coins", count);
	}

	public static void SaveBagStars(int count)
	{
		PlayerPrefs.SetInt ("Stars", count);
	}

	//-------------------------------------------------------------

	public static void SaveSoldStateHeli(int index)
	{
		PlayerPrefs.SetString (index.ToString(), "Sold");
	}

	public static bool IsHeliSold(int index)
	{
		string key = index.ToString ();

		if (PlayerPrefs.HasKey (key)) {
			if (PlayerPrefs.GetString (key) == "Sold")
				return true;
		}

		return false;
	}

	public static int LoadSelectedHeli()
	{
		return PlayerPrefs.GetInt ("SelectedHeli");
	}

	public static void SaveSelectedHeli(int index)
	{
		PlayerPrefs.SetInt ("SelectedHeli", index);
	}

	public static bool IsSeleted(int index)
	{
		if (PlayerPrefs.GetInt ("SelectedHeli") == index)
			return true;

		return false;
	}

	//-------------------------------------------------------------

	public static void SaveCollectedObject(string id)
	{
		PlayerPrefs.SetString (id, "Collected");
	}

	public static string LoadCollectedObject(string id)
	{
		return PlayerPrefs.GetString (id);
	}

	public static void SaveCollectedObjectInLevel(string objName, int count)
	{
		int loadedCount = LoadCollectedObjectInLevel (objName);
		if(count > loadedCount)
			PlayerPrefs.SetInt (objName, count);
	}

	public static int LoadCollectedObjectInLevel(string objName)
	{
		return PlayerPrefs.GetInt (objName);
	}

	//-------------------------------------------------------------

	public static void SaveSoundsSlider(float value)
	{
		PlayerPrefs.SetFloat ("Sounds", value);
	}

	public static void SaveMusicSlider(float value)
	{
		PlayerPrefs.SetFloat ("Music", value);
	}

	public static void SaveGraphicSlider(int value)
	{
		PlayerPrefs.SetInt ("Graphic", value);
	}

	public static float LoadSoundsSlider()
	{
		return PlayerPrefs.GetFloat ("Sounds");
	}

	public static float LoadMusicSlider()
	{
		return PlayerPrefs.GetFloat ("Music");
	}

	public static int LoadGraphicSlider()
	{
		return PlayerPrefs.GetInt ("Graphic");
	}
		
	public static void DeleteSettingsData()
	{
		PlayerPrefs.DeleteKey ("Sounds");
		PlayerPrefs.DeleteKey ("Music");
		PlayerPrefs.DeleteKey ("Graphic");
	}
}
