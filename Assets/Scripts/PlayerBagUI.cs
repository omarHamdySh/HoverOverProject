using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBagUI : MonoBehaviour {

	public static PlayerBagUI instance;

	[System.Serializable]
	public class Item
	{
		[SerializeField]private string name;
		public RectTransform itemPrefab;
		public RectTransform itemTarget;
	}

	[SerializeField]private RectTransform continer;

	public Item[] items;

	[SerializeField]private Text starsText;
	[SerializeField]private Text coinsText;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		UpdateBagUI ();
	}

	public void UpdateBagUI()
	{
		starsText.text = PlayerBag.instance.stars.ToString();
		coinsText.text = PlayerBag.instance.coins.ToString();
	}
		
	public void InstantiateItem(Item item, Vector3 pos)
	{
		Vector3 position = Camera.main.WorldToScreenPoint (pos);
		StartCoroutine (InstantiateStarCoroutine (item, position));
	}

	IEnumerator InstantiateStarCoroutine(Item item, Vector3 pos)
	{
		RectTransform s = Instantiate (item.itemPrefab) as RectTransform;
		s.SetParent (continer, false);
		s.position = pos;

		yield return new WaitForSeconds (0.03f);
		while (Vector2.Distance (s.position, item.itemTarget.position) > 30f) {

			s.position = Vector2.Lerp (s.position,item.itemTarget.position, Time.deltaTime * 10f);
			yield return null;
		}

		Destroy (s.gameObject);
	}

}
