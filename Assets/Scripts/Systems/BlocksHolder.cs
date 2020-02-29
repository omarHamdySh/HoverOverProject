using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksHolder : MonoBehaviour {

	public enum HolderType
	{
		Static,
		Dynamic
	}

	public HolderType type;

	public bool click;
	public bool enable = true;


	void OnValidate()
	{
//		if (!enable)
//			return;
		

		BlockBase[] blocks = GetComponentsInChildren<BlockBase> ();

		for (int i = 0; i < blocks.Length; i++) {
			if (type == HolderType.Dynamic)
				blocks[i].isStatic = false;
			else
				blocks[i].isStatic = true;

//			if(blocks[i].changeStaticScale)
//				blocks [i].transform.localScale = Vector3.one;
		}
	}

	public bool IsStatic()
	{
		return type == HolderType.Static;
	}

}
