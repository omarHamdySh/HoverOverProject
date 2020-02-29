using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinMenu : MonoBehaviour {

	public Animator anim;

	[Header("Actions")]
	public UnityEvent onHideMenu;
	public UnityEvent onShowMenu;

	public void ShowMenu(bool state)
	{
		anim.SetBool ("Show_MinMenu", state);
	}

	public void OnHideMenu()
	{
		onHideMenu.Invoke ();
		GameManager.instance.ResumeGame ();
	}

	public void OnShowMenu()
	{
		GameManager.instance.PushGame ();
		onShowMenu.Invoke();

	}
}
