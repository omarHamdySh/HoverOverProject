using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeController : MonoBehaviour {

	public enum StartFade
	{
		FadeOut = 0,
		FadeIn = 1
	}

	public StartFade startFade;
	public Image img;
	public bool playOnAwake = true;
	public float fadeSpeed = 2f;
	public AnimationCurve fadeCurve;

	public UnityEvent onEndFade;
	public UnityEvent onEndCurrentFade;

	public static FadeController Instance{ get; private set;}
	private float currentDeltaTime = 0.02f;

	void Awake()
	{
		Instance = this;
	}

	void Start () {

		if (playOnAwake) {
		
			StartDefultFade ();
		}
	}

	public void StartDefultFade()
	{
		currentDeltaTime = Time.deltaTime;
		StopAllCoroutines ();

		if(startFade == StartFade.FadeOut)
			FadeOut (()=> onEndFade.Invoke());
		else if(startFade == StartFade.FadeIn)
			FadeIn (()=> onEndFade.Invoke());
	}

	public void FadeOut(float speed = 1)
	{
		StopAllCoroutines ();
		fadeSpeed *= speed;
		StartCoroutine (StartFadeOut (()=>onEndCurrentFade.Invoke()));
	}

	public void FadeIn(float speed = 1)
	{
		StopAllCoroutines ();
		fadeSpeed *= speed;
		StartCoroutine (StartFadeIn (()=>onEndCurrentFade.Invoke()));
	}

	public void FadeOut(UnityAction onEndFade)
	{
		StopAllCoroutines ();

		currentDeltaTime = Time.unscaledDeltaTime;
		StartCoroutine (StartFadeOut (onEndFade));
	}

	public void FadeIn(UnityAction onEndFade)
	{
		StopAllCoroutines ();

		currentDeltaTime = Time.unscaledDeltaTime;
		StartCoroutine (StartFadeIn (onEndFade));
	}

	public void SetFadeSpeed(float value)
	{
		fadeSpeed = value;
	}
		

	IEnumerator StartFadeIn(UnityAction onEndFade)
	{

		img.gameObject.SetActive (true);
		UpdateAlphaColor (img, 1);
		float alpth = 0;
		float curve = 0;
		while (alpth < 1) {
			alpth += currentDeltaTime * fadeSpeed;
			curve = fadeCurve.Evaluate (alpth);
			UpdateAlphaColor (img, curve);

			yield return null;
		}

		UpdateAlphaColor (img, 1);
		//img.gameObject.SetActive (true);

		if (onEndFade != null)
			onEndFade ();
	}

	IEnumerator StartFadeOut(UnityAction onEndFade)
	{
		img.gameObject.SetActive (true);
		UpdateAlphaColor (img, 1);
		float alpth = 1;
		float curve = 0;

		while (alpth > 0) {

			alpth -= currentDeltaTime * fadeSpeed;
			curve = fadeCurve.Evaluate (alpth);
			UpdateAlphaColor (img, curve);

			yield return null;
		}

		UpdateAlphaColor (img, 0);

		if (onEndFade != null)
			onEndFade ();
		
		img.gameObject.SetActive (false);
	}


	void UpdateAlphaColor(Image img, float a)
	{
		img.color = new Color (img.color.r, img.color.g, img.color.b, a);
	}
}
