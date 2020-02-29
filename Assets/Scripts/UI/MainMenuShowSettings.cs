using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuShowSettings : MonoBehaviour {

	public Animator animator;

	public void ShowSettings(bool value)
	{
		animator.SetBool ("Show", value);
	}
		

}
