using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Point))]
public class PointExceptionEvents : MonoBehaviour {

	[SerializeField]private Point[] points;
	[Space(10)]
	[SerializeField]private UnityEvent onPointException;
	[SerializeField]private UnityEvent onPointNotException;

	void Start()
	{
		Setup ();
	}

	public void Setup()
	{
		GetComponent<Point> ().SetUpExceptionEvents (this);
	}

	public void ApplyEvent(Point p)
	{
		if (IsNotException (p)) {
			onPointNotException.Invoke ();
			print (gameObject.name);
		} else {
			onPointException.Invoke ();
		}
	}

	public bool IsNotException(Point p)
	{
		for (int i = 0; i < points.Length; i++) {
			if (points [i] != p)
				return true;
		}

		return false;
	}
}
