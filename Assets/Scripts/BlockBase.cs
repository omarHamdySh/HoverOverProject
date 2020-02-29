using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public abstract class BlockBase : MonoBehaviour {

	public enum BlockState
	{
		Pick,
		Drop,
		Replace
	}

	[System.Serializable]
	public struct TransfromSettings
	{
		public bool localTransform;
		public bool overridePos;
		public Vector3 position;
		public bool overrideRot;
		public Vector3 rotation;
		public bool overrideScale;
		public Vector3 scale;
	}

	public FillBlock.BlockType type;
	public BlockState state;

	public bool isStatic = true;
	public bool needFillBlock = true;
	//public Vector3 posOffset = Vector3.zero;
	public int fixedAngle = 90;

	[Header("Override Settings")]
	public bool changeStaticScale = true;
	public bool replacePos = true;
	public bool replaceRot = true;
	public bool replaceScale = true;
	public bool dropScale = true;
	public TransfromSettings pickTransSettings;
	public TransfromSettings dropTransSettings;

	[Header("Actions")]
	public UnityEvent onPickEvent;
	public UnityEvent onDropEvent;
	public UnityEvent onReplaceEvent;

	private FillBlock myFillBlock;
	private Rigidbody rb;
	private Collider[] cols;


	void Reset()
	{
		GetComponent<Rigidbody> ().isKinematic = true;
	}

	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}

	protected virtual void Update()
	{
		if (state == BlockState.Drop)
			UpdateTransformSettings (dropTransSettings);
		if (state == BlockState.Pick)
			UpdateTransformSettings (pickTransSettings);
	}

	public void SetFillBlock(FillBlock fillBlock)
	{
		myFillBlock = fillBlock;
	}

	public void Drop(Transform parent)
	{
		rb.isKinematic = false;
		ActiveColliders (true);
		transform.parent = parent;

		transform.localScale = dropTransSettings.localTransform && dropTransSettings.overrideScale ? dropTransSettings.scale : Vector3.one;
		UpdateTransformSettings(dropTransSettings);

		myFillBlock = null;
		onDropEvent.Invoke ();
		state = BlockState.Drop;
	}

	public void Pick()
	{
		rb.isKinematic = true;
		UpdateMyFillBlock (true);
		ActiveColliders (false);
		UpdateTransformSettings (pickTransSettings);
		onPickEvent.Invoke ();
		state = BlockState.Pick;

		if (myFillBlock != null) {
			myFillBlock.SetCorrectBlock (null);
			myFillBlock = null;
		}
	}

	public void Replace(Vector3 pos, Quaternion rot, Transform parent)
	{
		rb.isKinematic = true;
		transform.parent = null;
		ActiveColliders (true);
		UpdateMyFillBlock (false);

		if(replaceRot)
			transform.rotation = rot;

		transform.parent = parent;

		if(replacePos)
			transform.position = pos;
//
		if(replaceScale)
			transform.localScale = Vector3.one;
		

		state = BlockState.Replace;
		onReplaceEvent.Invoke ();
	}
		

	void UpdateTransformSettings(TransfromSettings settings)
	{
		if (!settings.localTransform) {
			if (settings.overridePos)
				transform.position = settings.position;
			if (settings.overrideRot)
				transform.rotation = Quaternion.Euler (settings.rotation);
		} else {
			
			if (settings.overridePos)
				transform.localPosition = settings.position;
			if (settings.overrideRot)
				transform.localRotation = Quaternion.Euler (settings.rotation);
		}

		if (settings.overridePos)
			transform.localScale = settings.scale;
	}

	void UpdateMyFillBlock(bool useableValue)
	{
		if (myFillBlock != null) {
			myFillBlock.SetUseable (useableValue);
		}
	}

	void ActiveColliders(bool value)
	{
		if (cols == null) {
			cols = GetComponentsInChildren<Collider> ();
		}

		for (int i = 0; i < cols.Length; i++) {
			cols [i].enabled = value;
		}
	}

	public bool IsStopped()
	{
		return rb.velocity.magnitude <= 0.01f;
	}

	public void SetStatic(bool value)
	{
		isStatic = value;
	}
}
