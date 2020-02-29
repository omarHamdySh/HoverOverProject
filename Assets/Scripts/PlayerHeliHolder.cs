using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerHeliHolder : MonoBehaviour {

	public static PlayerHeliHolder instance;

	public PlayerHeli player;
	public float rangeActiveFillBlocks = 10f;
	public int currentAngle;

	private BlockBase currentBlock;
	private FillBlock currentFillBlock;
	private List<FillBlock> allFillBlock = new List<FillBlock>();
	private BoxCollider colliderHolder;

	private Transform staticHolder;
	private Transform dynamicHolder;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		staticHolder = GameObject.FindGameObjectWithTag (Tags.STATIC_HOLDER).transform;
		dynamicHolder = GameObject.FindGameObjectWithTag (Tags.DYNAMIC_HOLDER).transform;

		colliderHolder = GetComponent<BoxCollider> ();
		currentBlock = null;
		currentFillBlock = null;
	}

	void Update()
	{
		if (player == null)
			return;

		if (player.IsPlayerDestroied ()) {
			if (currentBlock != null) {

				DropBlock ();

				currentBlock = null;

			}
			gameObject.SetActive (false);

			return;
		}

		if (currentBlock != null) {
			
			if (currentFillBlock != null) {
				
				if (currentFillBlock.enablePlayerAngle) {
					if (currentFillBlock.IsCorrectAngle (player.GetAngle ())) {
						currentFillBlock.ApplyCorrectFill ();
					} else
						currentFillBlock.ApplyWrongFill ();
				}
				currentFillBlock.SetAngle (player.GetAngle ());
			}

			if (Input.GetKeyDown ("space")) {
				
				ApplyRelease ();

			}
			if (currentBlock != null && currentBlock.needFillBlock) {
				if (Time.renderedFrameCount % 1 == 0) {
					FindNearestFillBlock ();
				}
			}

		}
	}
	public void ApplyRelease()
	{
		if (currentBlock != null) {
			StartCoroutine (Drop ());
			if (currentFillBlock != null) {
				if (currentFillBlock.enablePlayerAngle) {
					if (currentFillBlock.IsCorrectAngle (player.GetAngle ())) {
						ReplaceBlock ();
						currentBlock = null;
					} else {
						AudioManager.instance.PlaySoundByName ("Wronge Block Angle");
					}

				} else {
					ReplaceBlock ();
					currentBlock = null;
				}

			} else {
				DropBlock ();
				currentBlock = null;
			}
		}
	}

	void DropBlock()
	{
		currentBlock.Drop (dynamicHolder);
	}

	void ReplaceBlock()
	{
		AudioManager.instance.PlaySoundByName ("Replace Block");
		currentBlock.SetFillBlock (currentFillBlock); // this first

		currentBlock.Replace (currentFillBlock.transform.position, 
			Quaternion.Euler (0, player.GetAngle () - currentBlock.fixedAngle, 0), 
			staticHolder);

		if (currentBlock is TurnBlock) {
			currentBlock.GetComponent<TurnBlock> ().SetAngle (player.GetAngle ());
		}

		currentFillBlock.SetCorrectBlock (currentBlock);
		currentBlock.SetFillBlock (currentFillBlock);

		currentFillBlock = null;
		player.IncreaseUpPos (3);
		//GameManager.instance.CalculatePointsConnection ();
	}

	void OnTriggerEnter(Collider col)
	{
		if (currentBlock != null)
			return;

		if (col.GetComponent<BlockBase>() != null) {

			BlockBase cbb = col.GetComponent<BlockBase> ();
			if (cbb.isStatic)
				return;
			
			cbb.Pick ();
			cbb.transform.parent = transform;
			cbb.transform.localScale = Vector3.one;
			cbb.transform.position = transform.position;
			cbb.transform.localRotation = Quaternion.identity;
			currentBlock = cbb;
			if(cbb.needFillBlock)
				FindAllFillBlock(cbb.type);
		}
	}

	public bool IsCarrying()
	{
		return currentBlock != null;
	}

	IEnumerator Drop()
	{
		colliderHolder.enabled = false;
		yield return new WaitForSeconds (1f);
		colliderHolder.enabled = true;
		yield return null;
	}

	public void RefreshHolderData()
	{
		if (currentBlock != null) {

			FindAllFillBlock(currentBlock.type);
		}
	}

	void FindAllFillBlock(FillBlock.BlockType type)
	{
		FillBlock[] blocks = FindObjectsOfType<FillBlock> ();

		allFillBlock.Clear ();

		for (int i = 0; i < blocks.Length; i++) {
			
			if (!blocks [i].GetUseable())
				continue;

			if (blocks [i].blockType == type) {
				allFillBlock.Add (blocks [i]);
			}
		}
	}

	void FindNearestFillBlock()
	{
		if (allFillBlock == null || allFillBlock.Count < 1)
			return;

		FillBlock nesrest = allFillBlock [0];
		float min = Vector3.Distance (transform.position, nesrest.transform.position);

		for (int i = 0; i < allFillBlock.Count; i++) {
			
			allFillBlock [i].DisplayHighlightMesh (false);

			FillBlock farest = allFillBlock [i];
			float max = Vector3.Distance (transform.position, farest.transform.position);
			if (max < min) {
				nesrest = farest;
				min = max;
			}
		}

		if (Vector3.Distance (transform.position, nesrest.transform.position) <= player.sttings.grabRange) {
			if (!nesrest.GetUseable ())
				return;
			
			nesrest.DisplayHighlightMesh (true);
			currentFillBlock = nesrest;
		} else {
			currentFillBlock = null;
		}
	}
}
