using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetection : MonoBehaviour {

	public LayerMask hitLayer;
	public GameObject uiSelect;
	private PlayerHeli player;
	public Vector2 FixedRotDir;

	private Transform selectedBlock;
	private bool isRay = true;

	void Start()
	{
		player = FindObjectOfType<PlayerHeli> ();
	}
	
	void Update () {

		HeilController ();
	}

	void HeilController()
	{
		if (!isRay)
			return;

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, Mathf.Infinity, hitLayer.value)) {

			if (!player.IsHolderCarrying ()) {
				if (hit.collider.gameObject.GetComponent<BlockBase> ()) {
					Vector3 pos = new Vector3 (hit.collider.transform.position.x, 0, hit.collider.transform.position.z);
					//player.SetPosition (pos);
					selectedBlock = hit.collider.transform;
					EnableSelectUI (pos);
					UpdatePlayerPos (pos.x, pos.z);
				}
			} else {
				if (Input.GetMouseButtonDown (0)) {
					Vector3 pos = new Vector3 (hit.point.x, 0, hit.point.z);
					//player.SetPosition (pos);
					EnableSelectUI (pos);
					UpdatePlayerPos (hit.point.x, hit.point.z);
				}
			}

		} else {
			if(selectedBlock == null)
				uiSelect.SetActive (false);
		}
	}

	void EnableSelectUI(Vector3 pos)
	{
		uiSelect.SetActive (true);
		uiSelect.transform.position = pos;
	}

	public void EnableRay(bool value)
	{
		isRay = value;
	}

	void UpdatePlayerPos(float x, float y)
	{
		Vector3 rotDir = new Vector3 (player.transform.position.x - x, 0, player.transform.position.z - y);
		rotDir.Normalize ();
		player.SetRotate (rotDir.x* FixedRotDir.x,rotDir.z*FixedRotDir.y);
	}
}
