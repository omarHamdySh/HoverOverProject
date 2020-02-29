using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointUnitConnector : MonoBehaviour {

	[SerializeField]private Point nextPoint;
	[SerializeField]private List<Point> visitedPoints;
	private Point lastPoint;

	public void StartFindPath(){
		
		lastPoint = nextPoint;
		visitedPoints.Add (nextPoint);
		FindNextPoint ();
	}

	public void StopConnection()
	{
		nextPoint = null;
		lastPoint = null;
	}

	public Point GetCurrentPoint()
	{
		return nextPoint;
	}

	public Point GetLastPoint()
	{
		return lastPoint;
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
		if (nextPoint.conPoints.Count == 0)
			return;

		List<Point> points = new List<Point> ();
		List<Point> con = nextPoint.GetPoints ();

		for (int i = 0; i < con.Count; i++) {

			if (con [i] == lastPoint || !con [i].isWalkable) {
				continue;
			}

			if (visitedPoints.Contains (con [i]))
				continue;

			Vector3 starRay = transform.position;
			Vector3 endRay = con [i].transform.position - transform.position;
			float distanceRay = Vector3.Distance (transform.position, con [i].transform.position);

			if (!Physics.Raycast (starRay, endRay, distanceRay, GameManager.instance.hitLayer.value)) {

				if(!points.Contains(con[i]))
					points.Add (con[i]);
			}

		}

		List<Point> forawrdPoints = new List<Point>();


		Point closestPoint = null;
		if (points.Count > 0) {
			closestPoint = points [0];
			float min = Vector3.Dot (transform.forward, (closestPoint.transform.position - transform.position).normalized);

			if (points.Count != null && points.Count > 0) {

				for (int i = 0; i < points.Count; i++) {
					float max = Vector3.Dot (transform.forward, (points[i].transform.position - transform.position).normalized);
					if (max > min) {
						min = max;
						closestPoint = points [i];
					}
				}
			}
		}

		nextPoint = null;

		if (closestPoint != null) {
		
			float distance = Vector3.Distance (transform.position, closestPoint.transform.position);
			if (distance > 0.2f) {
				float fixPoint =  Vector3.Dot (transform.forward, (closestPoint.transform.position - transform.position).normalized);
				print (fixPoint);
				if (fixPoint < 0.5f) {
					closestPoint = null;
				}
			}
		}


		if (closestPoint == null || closestPoint == lastPoint) {
			nextPoint = null;
		} else {
			lastPoint = nextPoint;
			nextPoint = closestPoint;
		}
			
		if (!visitedPoints.Contains (nextPoint))
			visitedPoints.Add (nextPoint);
	}

	void OnDrawGizmos()
	{
		if (nextPoint != null) {
			Gizmos.DrawLine (transform.position, nextPoint.transform.position);
		}
	}
}
