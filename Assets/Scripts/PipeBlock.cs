using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBlock : BlockBase {

	[Header("Pipe Settings")]
	public Vector3 offsetPos = Vector3.zero;
	public Vector3 scale = Vector3.one;

	public void ChangePos()
	{
		//transform.position += offsetPos;
		//transform.localScale = scale;
	}

	public void ResetScale()
	{
		transform.localScale = scale;
	}
}
