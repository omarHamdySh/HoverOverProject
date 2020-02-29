using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransfromResetable : MonoBehaviour {

	[System.Serializable]
	public class ResetSettings
	{
		public Transform parent;

		public bool resetPos = false;
		public bool resetRot = false;
		public bool resetScale = false;
	}

	public ResetSettings[] settings;

	public void ApplyReset(int index)
	{
		ResetSettings currentSettings = settings [index];

		transform.parent = currentSettings.parent;
		if (currentSettings.resetPos)
			transform.localPosition = Vector3.zero;
		if (currentSettings.resetRot)
			transform.localRotation = Quaternion.identity;
		if (currentSettings.resetScale)
			transform.localScale = Vector3.one;
	}
}
