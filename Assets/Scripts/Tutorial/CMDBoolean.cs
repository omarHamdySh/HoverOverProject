using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CMDBoolean : MonoBehaviour {

	public bool[] activates;

	[Header("Actions")]
	public UnityEvent onActivateAllEvent;

	public void ApplyAction()
	{
		bool isAll = true;

		for (int i = 0; i < activates.Length; i++) {
			if (!activates [i])
				isAll = false;
		}

		if (isAll) {
			onActivateAllEvent.Invoke ();
		}
	}

	public void ActiveIndex(int i)
	{
		activates [i] = true;
		ApplyAction ();
	}
}
