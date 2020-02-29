using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBlock : BlockBase {

	[Header("Settings")]
	[SerializeField]private float angle = 0;
	public Transform anchorPoint;

	public void SetAngle(float _angle)
	{
		angle = _angle;

		if ((int)angle == 0 || (int)angle == 180)
			angle = 90;
		else if ((int)angle == 90)
			angle = -90;
	}

	public float GetAngle()
	{
		return (int)angle;
	}
}
