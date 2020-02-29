using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WinLoseMenu : MonoBehaviour {

	public Animator anim;
	public CanvasGroup canvasGroup;

	public Text timeText;
	public float fadeSpeed = 2f;

	[Header("3 Stars Settings")]
	public ThreeStarsContiner starWin;
	[Range(1,3), SerializeField]public int starsCount = 3;

	[Header("Actions")]
	public UnityEvent onShowWinMenuEvent;
	public UnityEvent onHideWinMenuEvent;

	public void Start()
	{
		//starsCount = 1;
		ShowMenu (true);
	}

	void Update()
	{
		canvasGroup.alpha = Mathf.MoveTowards (canvasGroup.alpha, 1, Time.deltaTime * fadeSpeed);
	}

	public void HideMenu()
	{
		canvasGroup.alpha = 0;
		anim.SetBool ("Win_Menu_Show", false);
	}

	public void ShowMenu(bool state)
	{
		anim.SetBool ("Win_Menu_Show", state);

		if (state) {
			onShowWinMenuEvent.Invoke ();
			timeText.text = "TIME: " + GameManager.instance.gameTimer.GetCurrentTime ().ToString("0.0");
		}
		else
			onHideWinMenuEvent.Invoke ();
	}

	public void ShowStars()
	{
		if(starWin != null)
			starWin.ShowStars (LevelInfo.instance.Get3Stars());
	}
}
