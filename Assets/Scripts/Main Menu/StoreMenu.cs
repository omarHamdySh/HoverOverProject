using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour {
	
	[System.Serializable]
	public class HeliStore
	{
		public HeliStoreSettingsSO heliSettings;
		public GameObject objectToDisplay;
		public int coinCost;
		public int starsCost;
		public bool isFree = false;
		public bool isAvailable = true;
		public bool isSold = false;
		public bool isSelected = false;
	}

	[System.Serializable]
	public class StoreSlider
	{
		public Slider slider;
		public Image sliderImgColor;
		public Text percentageText;
	}

	[System.Serializable]
	public struct BuyButton
	{
		public Button button;
		public Text buttonText;
		public Image imgState;
		public Image buttonImg;
		public Sprite unlockImg;
		public Sprite lockImg;
		public Color lockColor;
	}

	[System.Serializable]
	public struct StoreText
	{
		public Text coinsRequiredText;
		public Text starsRequiredText;
		public Text soldText;
		public Image coinImg;
		public Image starImg;
	}

	[Header("General Setting")]
	public int defualtHeliDisplay = 0; 
	public float sliderTranslationSpeed = 2f;
	public Color maxSliderColor = Color.green;
	public Color minSliderColor = Color.red;

	[Header("Bag Setting")]
	[SerializeField]private Text coinsText;
	[SerializeField]private Text starsText;
	[SerializeField]private int coins;
	[SerializeField]private int stars;

	public StoreText storeText;
	public BuyButton buybutton;
	public StoreSlider speedSlider;
	public StoreSlider weightSlider;
	public StoreSlider grabRangeSlider;


	[Header("Helis")]
	public HeliStore[] helis;

	private HeliStore currentHeliStore;
	private int currentHeliIndex = 0;

	void Start()
	{

		defualtHeliDisplay = Mathf.Clamp (defualtHeliDisplay, 0, helis.Length - 1);
		currentHeliIndex = defualtHeliDisplay;
		UpdateSoldFreeHeli ();
		HideHeli (currentHeliStore);
		currentHeliStore = helis [defualtHeliDisplay];
		DisplayHeli (currentHeliStore);
		UpdateBuyButtonUI (currentHeliStore);
		UpdateStoreText (currentHeliStore);
		UpdatePlayerBag ();

		if (!PlayerPrefs.HasKey ("SelectedHeli"))
			SaveLoad.SaveSelectedHeli (defualtHeliDisplay);
		
		print ("Selected Heli: " + PlayerPrefs.GetInt ("SelectedHeli"));
	}

	void OnEnable()
	{
		if(SaveLoad.instance != null)
			SaveLoad.instance.Load ();

		UpdatePlayerBag ();
	}

	public void UpdatePlayerBag()
	{
		coins = SaveLoad.LoadBagCoins ();
		stars = SaveLoad.LoadBagStars ();

		if (coins <= 0)
			coins = 0;
		if (stars <= 0)
			stars = 0;

		coinsText.text = coins.ToString();
		starsText.text = stars.ToString();
	}

	public void DisplayHeli(HeliStore heli)
	{
		if (heli == null)
			return;

		heli.objectToDisplay.SetActive (true);

		float speedP = heli.heliSettings.GetSpeedPercentage ();
		float weightP = heli.heliSettings.GetWeighPercentage ();
		float grabRangeP = heli.heliSettings.GetGrabRangePercentage ();
		StopAllCoroutines();

		//speedSlider.slider.value = speedP;
		speedSlider.percentageText.text = (speedP * 100).ToString() + "%";
		UpdateSliderValue (speedSlider.slider, speedP);
		UpdateSliderColor (speedSlider, speedP);

		//weightSlider.slider.value = weightP;
		weightSlider.percentageText.text = (weightP * 100).ToString() + "%";
		UpdateSliderValue (weightSlider.slider, weightP);
		UpdateSliderColor (weightSlider, weightP);

		//grabRangeSlider.slider.value = grabRangeP;
		grabRangeSlider.percentageText.text = (grabRangeP * 100).ToString() + "%";
		UpdateSliderValue (grabRangeSlider.slider, grabRangeP);
		UpdateSliderColor (grabRangeSlider, grabRangeP);

	}

	public void UpdateSliderValue(Slider s, float value)
	{
		StartCoroutine( UpdateSliderCoroutine (s, value));
	}

	IEnumerator UpdateSliderCoroutine(Slider s, float value)
	{
		while (s.value != value) {
			s.value = Mathf.MoveTowards (s.value, value, Time.deltaTime * sliderTranslationSpeed);
			yield return null;
		}
		yield break;
	}

	public void UpdateSliderColor(StoreSlider slider, float amount)
	{
		slider.sliderImgColor.color = Color.Lerp (minSliderColor, maxSliderColor, amount);
	}

	public void HideHeli(HeliStore heli)
	{
		if (heli == null)
			return;
		
		heli.objectToDisplay.SetActive (false);
		currentHeliStore = null;
	}

	void UpdateBuyButtonUI(HeliStore heli)
	{
		if (heli == null)
			return;

		if (heli.isFree) {
			buybutton.button.interactable = true;
			buybutton.buttonImg.color = Color.white;
			buybutton.imgState.sprite = buybutton.unlockImg;
			storeText.soldText.text = "Free";


		} else {

			if (!heli.isSold) {

				buybutton.imgState.gameObject.SetActive (true);
				buybutton.buttonText.text = "BUY";

				if (heli.isAvailable) {

					buybutton.button.interactable = true;
					buybutton.buttonImg.color = Color.white;
					buybutton.imgState.sprite = buybutton.unlockImg;
				} else {
					buybutton.button.interactable = false;
					buybutton.buttonImg.color = buybutton.lockColor;
					buybutton.imgState.sprite = buybutton.lockImg;
				}
			} else {
				storeText.soldText.text = "SOLD";
			}
		}

		if (heli.isSelected && (heli.isSold || heli.isFree)) {
			buybutton.imgState.gameObject.SetActive (false);
			buybutton.buttonText.text = "Selected";
		}
		else
		{
			if (!heli.isSold && heli.isAvailable && !heli.isFree) {
				buybutton.imgState.gameObject.SetActive (true);
				buybutton.buttonText.text = "Buy";

			} else if (heli.isFree || heli.isSold) {
				buybutton.imgState.gameObject.SetActive (true);
				buybutton.buttonText.text = "Select";
			}
			else if (!heli.isAvailable && !heli.isFree) {
				buybutton.imgState.gameObject.SetActive (true);
				buybutton.buttonText.text = "Locked";
			}
		}
	}

	void OnBuyHeli(HeliStore heli)
	{
		if (heli.isFree) {
		
			SaveLoad.SaveSelectedHeli (currentHeliIndex);

			return;
		} 

		else if (heli.isSold) {

			SaveLoad.SaveSelectedHeli (currentHeliIndex);
			return;
		}

		if (heli.isAvailable) {
			
			if (coins >= heli.coinCost && stars >= heli.starsCost) {

				print ("This Heli is Sold");
				coins -= heli.coinCost;
				stars -= heli.starsCost;
				heli.isSold = true;
				SaveLoad.SaveSelectedHeli (currentHeliIndex);
				SaveLoad.SaveSoldStateHeli (currentHeliIndex);
				SaveLoad.SaveBagCoins (coins);
				SaveLoad.SaveBagStars (stars);
				UpdatePlayerBag ();
				return;

			} else {
				print ("Not enough coins and stars");
			}
		}
	}

	void UpdateSoldFreeHeli()
	{
		bool foundSelected = false;

		for (int i = 0; i < helis.Length; i++) {

			if (helis [i].isFree) {
			
				if (SaveLoad.IsSeleted (i)) {
					helis [i].isSelected = true;
					foundSelected = true;
				}

				continue;
			}

			if (!helis [i].isAvailable) {
				
				continue;
			}

			if (SaveLoad.IsHeliSold (i)) {
				helis [i].isAvailable = true;
				helis [i].isSold = true;

				if (SaveLoad.IsSeleted (i))
					helis [i].isSelected = true;
			}
		}

		if (!foundSelected) {
			helis [0].isSelected = false;
		}
	}

	void UpdateSoldFreeSeletedHeli(HeliStore heli)
	{
		if (heli.isSold || heli.isFree) {
			for (int i = 0; i < helis.Length; i++) {

				if (helis [i] == heli) {

					heli.isSelected = true;
					buybutton.buttonText.text = "Selected";
					buybutton.imgState.gameObject.SetActive (false);

				} else {
					helis [i].isSelected = false;
				}
			}
		}
	}


	public void UpdateStoreText(HeliStore heli)
	{
		if (heli == null)
			return;

		if (heli.isFree || heli.isSold) {
		
			storeText.coinsRequiredText.enabled = false;
			storeText.starsRequiredText.enabled = false;
			storeText.coinImg.enabled = false;
			storeText.starImg.enabled = false;

		} else {

			storeText.coinsRequiredText.enabled = true;
			storeText.starsRequiredText.enabled = true;
			storeText.coinImg.enabled = true;
			storeText.starImg.enabled = true;

			storeText.coinsRequiredText.text = heli.coinCost.ToString ();
			storeText.starsRequiredText.text = heli.starsCost.ToString ();

		}

		if (heli.isSold || heli.isFree) {
			storeText.soldText.gameObject.SetActive (true);
		} else {
			storeText.soldText.gameObject.SetActive (false);
		}

	}

	public void MoveNext()
	{
		HideHeli (currentHeliStore);
		currentHeliIndex++;

		if (currentHeliIndex >= helis.Length)
			currentHeliIndex = 0;

		currentHeliStore = helis [currentHeliIndex];
		DisplayHeli (currentHeliStore);
		UpdateBuyButtonUI (currentHeliStore);
		UpdateStoreText (currentHeliStore);
	}

	public void MoveBack()
	{
		HideHeli (currentHeliStore);
		currentHeliIndex--;

		if (currentHeliIndex < 0)
			currentHeliIndex = helis.Length - 1;

		currentHeliStore = helis [currentHeliIndex];
		DisplayHeli (currentHeliStore);
		UpdateBuyButtonUI (currentHeliStore);
		UpdateStoreText (currentHeliStore);
	}

	public void Buy()
	{
		OnBuyHeli (currentHeliStore);
		UpdateBuyButtonUI (currentHeliStore);
		UpdateStoreText (currentHeliStore);
		UpdatePlayerBag ();
		UpdateSoldFreeSeletedHeli (currentHeliStore);
	}
}
