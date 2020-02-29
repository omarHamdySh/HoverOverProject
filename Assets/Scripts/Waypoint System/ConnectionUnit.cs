using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionUnit : MonoBehaviour {

	[SerializeField]private Waypoint waypoint;
	[SerializeField]private ConnectionPoint nextPoint;
	public bool searchOnAwake = false;
	public LayerMask hitLayer;
	public bool searchUsingDotProduct;

	private ConnectionPoint lastPoint;
	private bool isConnected = false;

	void Start()
	{
		if(searchOnAwake)
			FindClosestPointToStart ();
	}
		

	public void StopConnection()
	{
		nextPoint = null;
		lastPoint = null;
		isConnected = false;
	}

	public void StartConnection()
	{
		FindClosestPointToStart ();	
	}

	public ConnectionPoint GetCurrentPoint()
	{
		return nextPoint;
	}

	public bool TimeToNextPoint()
	{
		if (nextPoint == null)
			return false;
		
		float distance = Vector3.Distance (transform.position, nextPoint.transform.position);
		if (distance <= 0.1f) {
			return true;
		}

		return false;
	}

	public void FindNextPoint()
	{
		if (nextPoint == null)
			return;
		if (nextPoint.connectedPoints.Count == 0)
			return;
		
		ConnectionPoint closestPoint = null;
		List<ConnectionPoint> possiblePoints = new List<ConnectionPoint> ();

		for (int i = 0; i < nextPoint.connectedPoints.Count; i++) {

			if (nextPoint.connectedPoints [i] == lastPoint || !nextPoint.connectedPoints[i].isWalkable) {
				continue;
			}

			if(!possiblePoints.Contains(nextPoint.connectedPoints [i]))
				possiblePoints.Add (nextPoint.connectedPoints [i]);
		}

		closestPoint = possiblePoints[0];
		float min = Vector3.Distance (closestPoint.transform.position, transform.position);

		for (int i = 0; i < possiblePoints.Count; i++) {

			float max = Vector3.Distance (possiblePoints[i].transform.position, transform.position);
			print (max);
			if (max > min) {
				min = max;
				closestPoint = possiblePoints[i];
			}
		}

		if (closestPoint == null || closestPoint == lastPoint) {
			nextPoint = null;
			print("Next point is last point");
			return;
		} else {
			lastPoint = nextPoint;
			nextPoint = closestPoint;
		}
	}

	public void FindNextPointByRaycast()
	{
		if (nextPoint == null)
			return;
		if (nextPoint.connectedPoints.Count == 0)
			return;

		ConnectionPoint closestPoint = null;
		List<ConnectionPoint> possiblePoints = new List<ConnectionPoint> ();

		for (int i = 0; i < nextPoint.connectedPoints.Count; i++) {

			if (nextPoint.connectedPoints [i] == null)
				continue;

			if (nextPoint.connectedPoints [i] == lastPoint || !nextPoint.connectedPoints[i].isWalkable) {
				continue;
			}

			Vector3 starRay = transform.position;
			Vector3 endRay = nextPoint.connectedPoints [i].transform.position - transform.position;
			float distanceRay = Vector3.Distance (transform.position, nextPoint.connectedPoints [i].transform.position);

			if (!Physics.Raycast (starRay, endRay, distanceRay, hitLayer.value)) {
				possiblePoints.Add (nextPoint.connectedPoints [i]);
			}
		}

		closestPoint = null;
		if (possiblePoints.Count > 0) {

			closestPoint = possiblePoints[0];

			if (searchUsingDotProduct) {
				float min = Vector3.Dot (transform.forward, (closestPoint.transform.position - transform.position).normalized);

				for (int i = 0; i < possiblePoints.Count; i++) {

					float max = Vector3.Dot (transform.forward, (possiblePoints [i].transform.position - transform.position).normalized);
					if (max > min) {
						min = max;
						closestPoint = possiblePoints [i];
					}
				}
			} else {
				
				float min = Vector3.Distance (closestPoint.transform.position, transform.position);
				for (int i = 0; i < possiblePoints.Count; i++) {

					if(!possiblePoints[i].isWalkable)
						continue;
					
					float max = Vector3.Distance (possiblePoints[i].transform.position, transform.position);
					if (max < min) {
						min = max;
						closestPoint = possiblePoints[i];
					}
				}
			}
		}

		if (closestPoint == null || closestPoint == lastPoint) {
			nextPoint = null;
			print("Next point is last point");
			return;
		} else {
			lastPoint = nextPoint;
			nextPoint = closestPoint;
		}
	}

	void FindClosestPointToStart()
	{
		if (isConnected)
			return;

		isConnected = true;

		if (nextPoint != null) {
			lastPoint = nextPoint;
			return;
		}

		ConnectionPoint[] allPoints = waypoint.GetPoints ();
		if (allPoints == null || allPoints.Length == 0)
			return;

		ConnectionPoint closestPoint = allPoints [0];
		float min = Vector3.Distance (closestPoint.transform.position, transform.position);
		for (int i = 0; i < allPoints.Length; i++) {
			float max = Vector3.Distance (allPoints [i].transform.position, transform.position);
			if (max < min) {
				min = max;
				closestPoint = allPoints [i];
			}
		}

		nextPoint = closestPoint;
		lastPoint = nextPoint;
	}

}

