using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeli : MonoBehaviour {

	public Transform heliMesh;

	[Header("General Settings")]
	public HeliStoreSettingsSO sttings;

	[Header("Rotate Settings")]
	public float maxRotate = 30f;
	public float fixedRotate = -90f;
	public float rotateSpeed = 2f;

	[Header("Upleft Settings")]
	public float UpleftSpeed = 2f;
	public MinMax normalUpleftClamp = new MinMax(.4f, 20);
	public MinMax carryingUpleftClamp = new MinMax(5, 20);

	[Header("Control Settings")]
	public bool enableMobileController = true;

	[Header("Information")]
	[SerializeField]private int currentAngle;

	[Header("Player Dead Settings")]
	private float downForce = 1f;
	private float torqueForc = 5f;

	private Vector3 movementDir;
	private Vector3 rotateDir;
	private Vector3 moveRef;
	private float angle;
	private PlayerHeliHolder holder;
	private PlayerAudioEffect audioEffect;

	[HideInInspector]public Rigidbody rb;
	[HideInInspector]public Transform playerSpawnLocation;

	private bool isUp = false;
	private bool isDown = false;

	public bool isDistroied{ get; private set;}
	public Vector3 reverseAxis{ get; set;}

	void Start () {

		rb = GetComponent<Rigidbody> ();
		holder = GetComponentInChildren<PlayerHeliHolder> ();
		audioEffect = GetComponent<PlayerAudioEffect> ();
		PlayerHeli.FindObjectOfType<PlayerSpot> ().StartFollow ();
		movementDir = transform.position;

		if (Application.isMobilePlatform)
			enableMobileController = true;
		else
			enableMobileController = false;
	}

	public void Revive()
	{
		isDistroied = false;
		rb.isKinematic = true;
		movementDir = playerSpawnLocation.position;
		rb.position = playerSpawnLocation.position;
		rb.rotation = playerSpawnLocation.rotation;
		FindObjectOfType<PlayerSpot> ().Play ();
		holder.gameObject.SetActive (true);
		rb.isKinematic = false;
	}

	void FixedUpdate()
	{
		print (movementDir);
		if (IsPlayerDestroied ()) {
			ApplyDestroyAnimation ();
		}

		if (CantControl())
			return;
		
		Move ();
	}

	void Update () {

		if (IsPlayerDestroied()) {
			audioEffect.UpdateExplosionEffect ();
		}
			
		if (CantControl()) {
			return;
		}

		UpdateUpDownState ();
		CalculateInputs ();
		Rotate ();
	}

	void ApplyDestroyAnimation()
	{
		rb.AddForce (Vector3.down * downForce * 2 + reverseAxis * downForce);
		rb.AddTorque (Vector3.up * torqueForc);
	}

	public bool CantControl()
	{
		return isDistroied || GameManager.instance.IsGamePlaying ();
	}

	public void PlayerExploded()
	{
		isDistroied = true;
	}

	public bool IsPlayerDestroied()
	{
		return isDistroied;
	}

	void Rotate()
	{
		rb.MoveRotation(Quaternion.Lerp (rb.rotation, Quaternion.Euler (rotateDir), Time.deltaTime * rotateSpeed));
		heliMesh.localRotation = Quaternion.Lerp (heliMesh.localRotation, Quaternion.Euler (0,angle,0), Time.deltaTime * rotateSpeed);
	}

	void Move()
	{
		if(!holder.IsCarrying())
			movementDir.y = Mathf.Clamp (movementDir.y, normalUpleftClamp.min, normalUpleftClamp.max);
		else if(holder.IsCarrying())
			movementDir.y = Mathf.Clamp (movementDir.y, carryingUpleftClamp.min, carryingUpleftClamp.max);


		movementDir.x = Mathf.Clamp (movementDir.x, LevelInfo.instance.GetMinLimit().x,  LevelInfo.instance.GetMaxLimit().x);
		movementDir.z = Mathf.Clamp (movementDir.z,  LevelInfo.instance.GetMinLimit().y,  LevelInfo.instance.GetMaxLimit().y);

		rb.MovePosition (Vector3.SmoothDamp (rb.position, movementDir, ref moveRef, sttings.weight));
	}

	public void IncreaseUpPos(float y)
	{
		movementDir.y += y;
	}

	public bool IsOn(Vector3 targetPos)
	{
		Vector3 pos = new Vector3 (transform.position.x, 0, transform.position.z);
		Vector3 tPos = new Vector3 (targetPos.x, 0, targetPos.z);

		return Vector3.Distance (pos, tPos) < 0.1f;
	}

	void UpdateUpDownState()
	{
		if (isUp) {
			movementDir.y += Time.deltaTime * UpleftSpeed;
			audioEffect.UpdateUpleftEffect ();
		} else if (isDown) {
			movementDir.y -= Time.deltaTime * UpleftSpeed;
			audioEffect.UpdateDownleftEffect ();

		} else {
			audioEffect.UpdateNormalEffect ();
		}
	}

	public void GoDown(bool value)
	{
		isDown = value;
		isUp = false;
	}

	public void GoUp(bool value)
	{
		isUp = value;
		isDown = false;
	}

	public bool IsHolderCarrying()
	{
		return holder.IsCarrying ();
	}

	public void SetNormlizeRot(float a)
	{
		angle = a;
	}

	public void SetRotate(float x, float y)
	{
		float calAngle = Mathf.Atan2 (y, x) * Mathf.Rad2Deg + fixedRotate;

		if (x != 0 || y != 0)
			angle = calAngle;

		if (x > 0)
			currentAngle = 90;
		else if (x < 0)
			currentAngle = -90;
		if (y > 0)
			currentAngle = 0;
		else if (y < 0)
			currentAngle = 180;
	}

	void CalculateInputs()
	{
		if (!isDistroied) {
			if (!enableMobileController) {
				if (Input.GetKey ("e")) {
					movementDir.y += 0.5f * UpleftSpeed;
					audioEffect.UpdateUpleftEffect ();
				} else if (Input.GetKey ("q")) {
					movementDir.y -= 0.5f * UpleftSpeed;
					audioEffect.UpdateDownleftEffect ();
				} else {
					audioEffect.UpdateNormalEffect ();
				}
			}

		}
		float x = 0, y = 0;
		if (enableMobileController) {
		
			if (SimpleTouchController.instance != null) {
				x = SimpleTouchController.instance.GetVertical ();
				y = SimpleTouchController.instance.GetHorizontal ();
			}


		} else {
			x = Input.GetAxisRaw ("Horizontal");
			y = Input.GetAxisRaw ("Vertical");
		}

		movementDir += (Vector3.right * x + Vector3.forward * y) * sttings.speed;

		rotateDir = new Vector3 (y,0, -x) * maxRotate;

		SetRotate (-x, y);

		int playerAngle = (int)heliMesh.transform.localEulerAngles.y;

		if(playerAngle >= 315 && playerAngle < 45)
			currentAngle = 0;
		else if(playerAngle >= 45 && playerAngle < 135)
			currentAngle = 90;
		else if(playerAngle >= 135 && playerAngle < 225)
			currentAngle = 180;
		else if(playerAngle >= 225 && playerAngle < 315)
			currentAngle = 270;
	}



	public int GetAngle()
	{
		return currentAngle;
	}
}
