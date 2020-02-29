using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SelectLevelMenu : MonoBehaviour {

	[System.Serializable]
	public class LevelCardSettings
	{
		public string name;
		public string levelKey = "b_lv";
		public int levelOffset = 0;
		[Range(1,8)]public int availableLevels = 1;
		public bool autoActivateNext = true;

		public RectTransform card;
		public GameObject levelsContiner;
		public Button nextCardButton;
		public ButtonLevelInfo[] levelsButton;

		public void TryOpenNextCard()
		{
			if (nextCardButton == null)
				return;

			if (!autoActivateNext) {
				nextCardButton.interactable = true;
				nextCardButton.GetComponent<Image> ().color = Color.white;
				return;
			}
			
			if (availableLevels >= 8) {
				nextCardButton.interactable = true;
				nextCardButton.GetComponent<Image> ().color = Color.white;
			} else {
				nextCardButton.interactable = false;
				nextCardButton.GetComponent<Image> ().color = Color.red;
			}
		}
	}

	public float transationSpeed = 1f;

	public GameObject levelsParent;
	public GameObject backgroundLevels;
	public Animator levelsMenuAnimation;

	[Header("Scroll Settings")]
	public Scrollbar bar;
	public GameObject leftButtonScroll;
	public GameObject rightButtonScroll;

	public LevelCardSettings[] cards;

	[Header("Actions")]
	public UnityEvent onStartScrollEvent;
	public UnityEvent onEndScrollEvent;

	private float steps;
	private float currentValue;
	private int index = 0;
	private LevelCardSettings currentCard;

	void Start()
	{

		steps = 1f / (cards.Length - 1);
		currentValue = 0;
		print (PlayerPrefs.GetString ("i_lv" + 9 + LevelInfo.competedLevelKey));
		UpdateScrollValue ();
		UpdateCardsState ();
	}

	void UpdateCardsState()
	{
		for (int i = 0; i < cards.Length; i++)
			UpdateAvailableCards (i);

		for (int i = 0; i < cards.Length; i++) {
			cards [i].TryOpenNextCard ();
		}
	}

	void UpdateAvailableCards(int index)
	{
		LevelCardSettings selectedCard = cards [index];

		selectedCard.availableLevels = 1;
		print ("--------------------------------");
		for (int i = 0; i < selectedCard.levelsButton.Length; i++) {

			int levelNumber =	selectedCard.levelOffset + i + 1;
			string levelKey = selectedCard.levelKey + levelNumber + LevelInfo.competedLevelKey;

			if (PlayerPrefs.GetString (levelKey) == "Completed") {
				selectedCard.availableLevels += 1;
			}
		}

		selectedCard.availableLevels = Mathf.Clamp (selectedCard.availableLevels, 1, 8);
		print ("--------------------------------");
	}

	public void ScrollNext()
	{
		StopAllCoroutines ();
		currentValue += steps;
		UpdateIndex (1);
		UpdateScrollValue ();
		StartCoroutine (ScrollToCoroutine (currentValue));
	}

	public void ScrollBack()
	{
		StopAllCoroutines ();
		UpdateIndex (-1);
		currentValue -= steps;
		UpdateScrollValue ();
		StartCoroutine (ScrollToCoroutine (currentValue));
	}

	void UpdateScrollValue()
	{
		currentValue = Mathf.Clamp (currentValue, 0, 1);

		if (currentValue == 1) {
			rightButtonScroll.SetActive (false);
			leftButtonScroll.SetActive (true);
		} else if (currentValue == 0) {
			rightButtonScroll.SetActive (true);
			leftButtonScroll.SetActive (false);
		} else {
			rightButtonScroll.SetActive (true);
			leftButtonScroll.SetActive (true);
		}
	}

	void UpdateIndex(int dir = 1)
	{
		index += 1 * dir;
		index = Mathf.Clamp (index, 0, cards.Length - 1);
	}


	public void OnHideLevelAction()
	{
		if (currentCard != null) {
			currentCard.levelsContiner.SetActive (false);

			if (currentCard.levelsContiner != null)
				currentCard.levelsContiner.SetActive (false);
		}

		levelsParent.SetActive (false);
	}

	public void HideLevels()
	{
		levelsMenuAnimation.SetBool ("Select_Menu_Show", false);
		backgroundLevels.SetActive (false);
		Invoke ("OnHideLevelAction", 1);
	}

	public void ShowLevels(int index)
	{
		CancelInvoke ("OnHideLevelAction");

		LevelCardSettings selectedCard = cards [index];
		currentCard = selectedCard;

		selectedCard.availableLevels = Mathf.Clamp (selectedCard.availableLevels, 0, selectedCard.levelsButton.Length);
		levelsParent.SetActive (true);
		selectedCard.levelsContiner.SetActive (true);
		backgroundLevels.SetActive (true);

		for (int i = 0; i < cards.Length; i++) {
			UpdateAvailableCards (i);
		}

		int levelText = 0;
		for (int i = 0; i < selectedCard.levelsButton.Length; i++) {
			if (selectedCard.levelsButton [i] == null)
				continue;

			int level = (i + 1) + selectedCard.levelOffset;

			if (i < selectedCard.availableLevels) {

				int levelNumber =	selectedCard.levelOffset + i + 1;

				selectedCard.levelsButton [i].UpdateInfo (true,level);
				selectedCard.levelsButton [i].SetStars (PlayerPrefs.GetInt(selectedCard.levelKey + levelNumber + LevelInfo.threeStarsKey));

			} else {
				selectedCard.levelsButton [i].UpdateInfo (false, level);
			}

			levelText = i + 1;
			selectedCard.levelsButton [i].SetLevelText (levelText.ToString());
		}

		levelsMenuAnimation.SetBool ("Select_Menu_Show", true);
	}

	IEnumerator ScrollToCoroutine(float amount)
	{
		onStartScrollEvent.Invoke ();
		while (bar.value != amount) {
			bar.value = Mathf.MoveTowards (bar.value, amount, Time.deltaTime * transationSpeed);
			yield return null;
		}
		onEndScrollEvent.Invoke ();
	}
}
