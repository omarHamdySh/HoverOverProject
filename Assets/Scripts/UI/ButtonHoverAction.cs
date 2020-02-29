using UnityEngine;
using UnityEngine.Events;

public class ButtonHoverAction : MonoBehaviour {

	[SerializeField]private UnityEvent onEndCameraHoverEvent;


	void Start()
	{
		GetComponent<UnityEngine.UI.Button> ().onClick.AddListener (ApplyOnEndCameraHoverEvent);
	}

	public void ApplyOnEndCameraHoverEvent()
	{
		CameraHover camHover = FindObjectOfType<CameraHover> ();
		if (camHover == null)
			return;
		
		camHover.onEndHoverOut.AddListener (onEndCameraHoverEvent.Invoke);
	}
}
