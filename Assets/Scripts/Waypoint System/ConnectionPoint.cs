using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ConnectionPoint : MonoBehaviour {

	//public ConnectionPointSO connection;
	[SerializeField]public bool lockChanges;
	public bool isWalkable = true;

	public List<ConnectionPoint> connectedPoints;

	void Start () {

		if (!Application.isPlaying) {
//			if (connection.connected)
//				return;
			if (lockChanges)
				return;

			ConnectWithClosestPoint ();
		} 
//		else {
//			connection.connected = true;
//		}
	}

	public void SetChangesState(bool state)
	{
		lockChanges = state;
	}

	void ConnectWithClosestPoint()
	{
		if (connectedPoints != null)
			connectedPoints.Clear ();

		ConnectionPoint[] allPoints = FindObjectsOfType<ConnectionPoint> ();

		if (allPoints == null || allPoints.Length == 0)
			return;

		for (int i = 0; i < allPoints.Length; i++) {

			if (allPoints [i] == this)
				continue;

			float distance = Vector3.Distance (allPoints [i].transform.position, transform.position);
			if (distance < 0.01f) {

				if (!connectedPoints.Contains (allPoints [i])) {
					connectedPoints.Add (allPoints [i]);
					break;
				}
			}
		}
	}

	public void ConnectWithMyPoints()
	{
		if (connectedPoints == null || connectedPoints.Count == 0)
			return;

		for (int i = 0; i < connectedPoints.Count; i++) {
			if (!connectedPoints [i].connectedPoints.Contains (this)) {
				connectedPoints [i].connectedPoints.Add (this);
			}
		}
	}

	public void ReverseConnectMyPoints()
	{
		if (connectedPoints == null || connectedPoints.Count == 0)
			return;

		for (int i = 0; i < connectedPoints.Count; i++) {
			connectedPoints [i].connectedPoints.Add (this);
			connectedPoints.Remove (connectedPoints [i]);
		}
	}

	public void DisconnectWithMyPoints()
	{
		if (connectedPoints == null || connectedPoints.Count == 0)
			return;

		for (int i = 0; i < connectedPoints.Count; i++) {
			if (connectedPoints [i].connectedPoints.Contains (this)) {
				connectedPoints [i].connectedPoints.Remove (this);
			}
		}
	}

	public void SetWalkable(bool value)
	{
		isWalkable = value;
	}

	void OnDrawGizmosSelected()
	{
		if (connectedPoints == null || connectedPoints.Count == 0)
			return;

		for (int i = 0; i < connectedPoints.Count; i++) {

			if (connectedPoints [i] == null)
				continue;

			Gizmos.DrawLine (transform.position, connectedPoints [i].transform.position);
		}
	}
	

}
