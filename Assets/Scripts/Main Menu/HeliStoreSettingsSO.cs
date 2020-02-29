using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heli Settings", menuName = "Heli/ Heli Ability")]
public class HeliStoreSettingsSO : ScriptableObject {

	[Range(0.1f,0.5f)] public float speed = 0.1f;
	[Range(0.2f,1)] public float weight = 0.2f;
	[Range(3,10)]  public float grabRange = 3f;

	public float GetSpeedPercentage()
	{
		return speed / 0.5f;
	}

	public float GetWeighPercentage()
	{
		return weight / 1;
	}

	public float GetGrabRangePercentage()
	{
		return grabRange / 10;
	}
}
