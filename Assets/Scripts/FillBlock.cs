using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class FillBlock : MonoBehaviour {

	public enum BlockType
	{
		Any,
		Pipe
	}

	[Header("General")]
	public BlockType blockType;
	public ObsticleBlock obsticle;
	public BlockBase[] correctBlocks;

	[Header("Angle Info")]
	public bool enablePlayerAngle = false;
	public bool enableChangeAngle = true;
	public Vector3 angles;
	[SerializeField]private int[] requiredPlayerAngles;

	[Header("Materials")]
	public Material correctFill;
	public Material wrongFill;
	public MeshRenderer[] highlightMeshs;

	[Header("Actions")]
	public UnityEvent onCorrectBlockEvent;
	public UnityEvent onWrongBlockEvent;
	public UnityEvent onReplaceEvent;

	public bool isUseable = true;
	private MeshRenderer[] allChilds;

	void Start()
	{
		allChilds = GetComponentsInChildren<MeshRenderer> ();

		SetUseable (true);
		ApplyCorrectFill ();
		DisplayHighlightMesh (false);
	}

	void Reset()
	{
		GetComponent<BoxCollider> ().isTrigger = true;
		obsticle = GetComponentInChildren<ObsticleBlock> ();
	}

	void LateUpdate()
	{
		if (!isUseable)
			DisplayHighlightMesh (false);
	}

//	public int GetAngle()
//	{
//		return requiredPlayerAngle;
//	}

	public bool IsCorrectAngle(int angle)
	{
		for (int i = 0; i < requiredPlayerAngles.Length; i++) {
			if (requiredPlayerAngles [i] == angle)
				return true;
		}

		return false;
	}

	public void ApplyWrongFill()
	{
		if (!isUseable)
			return;
		
		for (int i = 0; i < highlightMeshs.Length; i++) {
			highlightMeshs [i].material = wrongFill;
		}
	}

	public void SetCorrectBlock(BlockBase block)
	{
		if (block == null)
			onWrongBlockEvent.Invoke ();

		for (int i = 0; i < correctBlocks.Length; i++) {
			if (correctBlocks [i] == block) {
				onCorrectBlockEvent.Invoke ();
				break;
			}
		}
		onReplaceEvent.Invoke ();
	}

	public void ApplyCorrectFill()
	{
		if (!isUseable)
			return;
		
		for (int i = 0; i < highlightMeshs.Length; i++) { 
			highlightMeshs [i].material = correctFill;
		}
	}

	public void SetAngle(float angle)
	{
		if(enableChangeAngle)
			transform.rotation = Quaternion.Euler (0, angle, 0);
	}

	public void DisplayHighlightMesh(bool value)
	{
		for (int i = 0; i < allChilds.Length; i++) {
			allChilds [i].gameObject.SetActive(value);
		}
	}

	public bool GetUseable()
	{
		return isUseable;
	}

	public void SetUseable(bool value)
	{
		isUseable = value;

		if (!value)
			DisplayHighlightMesh (false);
		else {
			if(PlayerHeliHolder.instance != null)
				PlayerHeliHolder.instance.RefreshHolderData ();
		}
		
		if(obsticle != null)
			obsticle.SetEnable(isUseable);
	}
}
