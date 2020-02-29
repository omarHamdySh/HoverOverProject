using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
	
	public enum SearchType
	{
		Forward_Only,
		Forward_Backward,
		Cross
	}

	public SearchType searchType;
	public bool isStatic = false;
	public bool isWalkable = true;
	public bool reverseeConnection = false;
	public bool alwaysConnection = false;

	private Point[] points;
	public List<Point> conPoints;

	private PointExceptionEvents exceptionEvent;

	// Use this for initialization

	public void Connect (LayerMask hitLayer) {

		if (isStatic)
			return;
		points = FindObjectsOfType<Point> ();

		for (int i = 0; i < points.Length; i++) {

			if (points [i].isStatic)
				continue;

			if (points [i] == this)
				continue;

			Vector3 pos = transform.position;
			Vector3 target = (points [i].transform.position - transform.position);
			float distance = Vector3.Distance (transform.position, points [i].transform.position);

			if (Physics.Raycast (pos, target, distance, hitLayer.value)) {
				continue;
			}

			switch (searchType) {
			case SearchType.Forward_Only:
				ForwardConnection (points [i]);
				break;

			case SearchType.Forward_Backward:
				ForwardBackwardConnection (points [i]);
				break;

			case SearchType.Cross:
				CrossConnection (points [i]);
				break;
			}

			float fixDis = Vector3.Distance(transform.position, points[i].transform.position);
			if(fixDis <= 0.1f)
			{
				if (!conPoints.Contains (points [i])) {
					conPoints.Add (points [i]);

					if (reverseeConnection) {
						if(!points[i].conPoints.Contains(this))
							points [i].conPoints.Add (this);
					}
				}
			}
		}
		if (exceptionEvent != null) {
			for (int i = 0; i < conPoints.Count; i++) {
				exceptionEvent.ApplyEvent (conPoints[i]);
			}
		}


	}

	void CrossConnection(Point p)
	{
		float dis = Vector3.Dot (transform.forward, (p.transform.position - transform.position).normalized);

		bool found = false;

		if (dis >= 1) {
			found = true;
		}

		dis = Vector3.Dot (-transform.forward, (p.transform.position - transform.position).normalized);
		if (dis >= 1) {
			found = true;
		}

		dis = Vector3.Dot (transform.right, (p.transform.position - transform.position).normalized);
		if (dis >= 1) {
			found = true;
		}

		dis = Vector3.Dot (-transform.right, (p.transform.position - transform.position).normalized);
		if (dis >= 1) {
			found = true;
		}

		if (found) {
			if (!conPoints.Contains (p)) {
				conPoints.Add (p);

				ConnectOtherPoint (p);

			}
		}
	}

	void ForwardConnection(Point p)
	{
		float dis = Vector3.Dot (transform.forward, (p.transform.position - transform.position).normalized);
		if (dis >= 1) {
			if (!conPoints.Contains (p)) {
				conPoints.Add (p);

				ConnectOtherPoint (p);

			}
		}
	}

	void ForwardBackwardConnection(Point p)
	{

		float dis = Vector3.Dot (transform.forward, (p.transform.position - transform.position).normalized);
		bool found = false;
		if (dis >= 1) {
			found = true;
		}

		dis = Vector3.Dot (-transform.forward, (p.transform.position - transform.position).normalized);
		if (dis >= 1) {
			found = true;
		}

		if (found) {
			if (!conPoints.Contains (p)) {
				conPoints.Add (p);

				ConnectOtherPoint (p);
			}
		}
	}

	void ConnectOtherPoint(Point p)
	{
		if (reverseeConnection) {
			if(!p.conPoints.Contains(this))
				p.conPoints.Add (this);
		}

		if (p.alwaysConnection) {
			if (!conPoints.Contains (this)) {
				p.conPoints.Add (this);
			}
		}
	}

	public void SetUpExceptionEvents(PointExceptionEvents pee)
	{
		exceptionEvent = pee;
	}

	public  List<Point> GetPoints()
	{
		return conPoints;
	}

	void OnDrawGizmosSelected()
	{
		if (!Application.isPlaying) {
			if (conPoints == null)
				return;

			for (int i = 0; i < conPoints.Count; i++) {

				Gizmos.DrawLine (transform.position, conPoints [i].transform.position);
			}
		}
	}

	void OnDrawGizmos()
	{
		if (Application.isPlaying) {
			if (conPoints == null)
				return;

			for (int i = 0; i < conPoints.Count; i++) {

				Gizmos.DrawLine (transform.position, conPoints [i].transform.position);
			}
		}
	}
	

}
