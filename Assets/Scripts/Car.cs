using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PointUnitConnector))]
public class Car : MonoBehaviour {

	public float speed = 2f;
	public float smoothReposMoveSpeed = 2f;
	public float rotateSpeed = 2f;
	public float angle = 0;

	[Header("Actions")]
	public UnityEvent onCompletedEvent;

	private bool isMoving = false;
	private Transform currentBlock;
	private Rigidbody rb;
	private bool isCompleted = false;
	private AudioSource audio;
	private PointUnitConnector unit;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		audio = GetComponent<AudioSource> ();
		unit = GetComponent<PointUnitConnector> ();
		isMoving = false;
		angle = (int)transform.eulerAngles.y;
	}

	void Update()
	{
		if (!isMoving)
			return;
		
		if (unit.GetCurrentPoint() == null)
			return;
		if (unit.TimeToNextPoint()) {
			unit.FindNextPoint ();
		}
	}

	void FixedUpdate () {

		if (!isMoving)
			return;
		
		if (unit.GetCurrentPoint () != null) {

			Vector3 nextPointPos = unit.GetCurrentPoint ().transform.position;	

			Vector3 rotAxis = nextPointPos - transform.position;

			rb.MovePosition (Vector3.MoveTowards (transform.position, nextPointPos, Time.deltaTime * speed));
			if (rotAxis.magnitude > 0)
				rb.MoveRotation (Quaternion.RotateTowards (rb.rotation, Quaternion.LookRotation (rotAxis), rotateSpeed * Time.deltaTime));

		} else {
			rb.position += transform.forward * Time.deltaTime * speed;
		}
	}

	public void Move(bool value)
	{
		isMoving = value;

		if (isMoving) {
			unit.StartFindPath();
		}

	}


	public bool IsCompleted()
	{
		return isCompleted;
	}

	void OnCarCompleted()
	{
		onCompletedEvent.Invoke ();
	}

	void OnTriggerEnter(Collider col)
	{

		if (col.gameObject.CompareTag (Tags.COIN)) {
			PlayerBagUI.instance.InstantiateItem (PlayerBagUI.instance.items[1], col.transform.position);
			PlayerBag.instance.AddCoin ();
			col.GetComponent<CollectableObject> ().Collect ();

		}
		if (col.gameObject.CompareTag (Tags.GEM)) {
			PlayerBag.instance.AddGem ();
			col.GetComponent<CollectableObject> ().Collect ();
		}

		if (col.gameObject.CompareTag (Tags.SE_BLOCK)) {
			if (col.GetComponent<StartEndBlock> ().isEnd) {
				isMoving = false;
				isCompleted = true;
				GameManager.instance.UpdateCompletedCars ();
				onCompletedEvent.Invoke ();
			}
		}

		if (col.gameObject.CompareTag (Tags.OBSTICLE_BLOCK) || col.gameObject.CompareTag (Tags.CAR)) {
			
			if (col.GetComponent<ObsticleBlock> () != null) {
				if (col.GetComponent<ObsticleBlock> ().IsEnable ()) {
					print ("Crash");
				}
			}
			GameManager.instance.GameOver ();

			audio.Play ();
		}
	}

	void OnDrawGizmos()
	{
		if (unit == null)
			return;

		if (unit.GetCurrentPoint () != null) {
			Gizmos.color = Color.green;
			Gizmos.DrawLine (transform.position, unit.GetCurrentPoint ().transform.position);
			Gizmos.DrawSphere (unit.GetCurrentPoint ().transform.position, .5f);
		}

		if (unit.GetLastPoint () != null) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere (unit.GetLastPoint ().transform.position, .5f);
		}

	}
}
