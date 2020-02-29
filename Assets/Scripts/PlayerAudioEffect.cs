using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioEffect : MonoBehaviour {

	[SerializeField]private AudioSource audio;
	public float changeSpeed = 2f;

	[Header("Movement Settings")]
	public float normalPitch = 0.7f;
	public float upleftPitch = 0.8f;
	public float downleftPitch = 0.7f;

	[Header("Explosion Settings")]
	public float explPitch = 0.5f;
	public float explPVolume = 1;

	public void UpdateUpleftEffect()
	{
		audio.pitch = Mathf.Lerp (audio.pitch, upleftPitch, Time.deltaTime * changeSpeed);
	}
	public void UpdateDownleftEffect()
	{
		audio.pitch = Mathf.Lerp (audio.pitch, downleftPitch, Time.deltaTime * changeSpeed);
	}
	public void UpdateNormalEffect()
	{
		audio.pitch = Mathf.Lerp (audio.pitch, normalPitch, Time.deltaTime * changeSpeed);
	}

	public void UpdateExplosionEffect()
	{
		audio.pitch = Mathf.Lerp (audio.pitch, explPitch, Time.deltaTime * changeSpeed);
		audio.volume = Mathf.Lerp (audio.volume, explPVolume, Time.deltaTime * changeSpeed * 2);
	}

	public void StopSound(bool value)
	{
		if (value)
			audio.Pause ();
		else
			audio.Play ();
	}
}
