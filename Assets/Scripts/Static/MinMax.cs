using UnityEngine;

[System.Serializable]
public struct MinMax {


	public float min;
	public float max;

	public MinMax (float minimum, float maximum)
	{
		min = minimum;
		max = maximum;
	}

	public float GetRandomValue()
	{
		return Random.Range (min, max);
	}
}
