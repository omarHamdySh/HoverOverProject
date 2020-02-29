using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {


	public static AudioManager instance;
	public AudioSource music;
	public AudioSource collectAudio;
	public AudioSource explosionAudio;
	public AudioSource clickAudio;
	public AudioSource[] otherSounds;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		UpdateMusicVolume ();
		UpdateSoundsVolume ();
	}

	public void StopPlayerSound(bool value)
	{
		FindObjectOfType<PlayerAudioEffect> ().StopSound (value);
	}

	public void PlayCollectAudio(AudioClip clip, float pitch = 1, float volume = 1)
	{
		collectAudio.pitch = pitch;
		collectAudio.volume = volume * SaveLoad.LoadSoundsSlider ();
		collectAudio.PlayOneShot (clip);
	}

	public void PlayExplosionClip(params AudioClip[] clips)
	{
		for (int i = 0; i < clips.Length; i++) {
			explosionAudio.PlayOneShot (clips [i]);
		}
	}

	public void UpdateMusicVolume()
	{
		if(music != null)
			music.volume = SaveLoad.LoadMusicSlider ();
	}

	public void PlaySoundByName(string name)
	{
		for (int i = 0; i < otherSounds.Length; i++) {
			if (otherSounds [i].name == name) {
				otherSounds [i].Play ();
				break;
			}
		}
	}
		
	public void UpdateSoundsVolume()
	{
		float volume = SaveLoad.LoadSoundsSlider ();
		if(collectAudio != null)
			collectAudio.volume = volume;
		
		if(explosionAudio != null)
			explosionAudio.volume = volume;

		if (otherSounds != null && otherSounds.Length > 0) {
			for (int i = 0; i < otherSounds.Length; i++) {
				otherSounds [i].volume = volume;
			}
		}
	}
}
