using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevelInfo : MonoBehaviour {

	[SerializeField]private bool isActive;
	[SerializeField]private int levelToLoad = 0;
	[SerializeField]private int collectedStars = 0;

	[Header("Colors")]
	public Color inactiveColor = Color.red;

	[Header("UI")]
	[SerializeField]private Button levelButton;
	[SerializeField]private Sprite starSprite;
	[SerializeField]private Sprite failStarSprite;
	[SerializeField]private Text levelText;
	[SerializeField]private Image[] stars;


	public void SetStars(int count)
	{
		collectedStars = count;
	}

	public void UpdateInfo (bool activeState, int level) {

		levelToLoad = level;

		isActive = activeState;
		if (!isActive) {
			levelButton.interactable = false;
			levelButton.GetComponent<Image> ().color = inactiveColor;		
		} else {
			levelButton.interactable = true;
			levelButton.GetComponent<Image> ().color = Color.white;		
		}

		if (isActive) {
			UpdateStars ();
		}
	}

	public void SetLevelText(string text)
	{
		levelText.text = text;
		UpdateStars ();
	}

	void UpdateStars () {

		collectedStars = Mathf.Clamp (collectedStars, 0, 3);

		for (int i = 0; i < collectedStars; i++) {

			stars [i].sprite = starSprite;
		}
	}

	public void ActionsToApply()
	{
		FadeController.Instance.onEndCurrentFade.AddListener (DDD);
	}

	void DDD()
	{
		FindObjectOfType<SceneLoader> ().LoadScene (levelToLoad);
	}
}
