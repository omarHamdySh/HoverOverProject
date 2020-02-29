using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAd : MonoBehaviour {

	void Start () {
		if (Unityad.instance == null) {
			print ("Unityad Script is missing");
			return;
		}

		Button button = GetComponent<Button> ();
		button.onClick.AddListener (() => Unityad.instance.ShowRewardVedio());
	}
}
