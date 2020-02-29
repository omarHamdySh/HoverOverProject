using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public bool lockChanges = false;

	void OnValidate()
	{
		for (int i = 0; i < GetPoints().Length; i++) {
			GetPoints () [i].SetChangesState (lockChanges);
		}
	}

	public ConnectionPoint[] GetPoints()
	{
		ConnectionPoint[] allChildPoints = GetComponentsInChildren<ConnectionPoint> ();
		return allChildPoints;
	}

	[ContextMenu("Connect all Sides")]
	public void ConnectPoints()
	{

		ConnectionPoint[] points = GetComponentsInChildren<ConnectionPoint> ();
		if (points == null) {
			print ("NO POINTS TO CONNECT!");
			return;
		}
		for (int i = 0; i < points.Length; i++) {
			points [i].ConnectWithMyPoints ();
		}

		print ("All sides Are Connected");
	}

	[ContextMenu("Reverse Connection")]
	public void ReverseConnections()
	{
		ConnectionPoint[] points = GetComponentsInChildren<ConnectionPoint> ();

		if (points == null) {
			print ("NO POINTS TO Correct!");
			return;
		}
		for (int i = 0; i < points.Length; i++) {
			points [i].ReverseConnectMyPoints ();
		}

		print ("All Connetions Were Corrected");
	}

	[ContextMenu("Reset Connection")]
	public void DisconnectPoints()
	{
		ConnectionPoint[] points = GetComponentsInChildren<ConnectionPoint> ();
		if (points == null) {
			print ("NO POINTS TO Correct!");
			return;
		}
		for (int i = 0; i < points.Length; i++) {
			points [i].DisconnectWithMyPoints ();
		}

		print ("All Connetions Were Corrected");
	}
}
