using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	[System.Serializable]
	public class SettingsSlider
	{
		public Slider slider;
		public Text text;
		public float defultValue = 1;

		public void LoadDefaultSettings()
		{
			slider.value = defultValue;
		}
	}

	public SettingsSlider soundsSlider;
	public SettingsSlider musicSlider;
	public SettingsSlider graphicSlider;

	void Awake()
	{
		LoadSettings ();
		//UpdateGraphicSlider ();
	}

	public void UpdateGraphicSlider()
	{
		int index = (int)graphicSlider.slider.value;
		QualitySettings.SetQualityLevel (index);
		graphicSlider.text.text = QualitySettings.names[index];
		SaveLoad.SaveGraphicSlider (index);
	}

	public void UpdateSoundsSlider()
	{
		float volume = soundsSlider.slider.value;
		float progress = volume * 100;
		soundsSlider.text.text = progress.ToString("0") + "%";
		SaveLoad.SaveSoundsSlider (volume);
	}

	public void UpdateMusicSlider()
	{
		float volume = musicSlider.slider.value;
		float progress = volume * 100;
		musicSlider.text.text = progress.ToString("0") + "%";
		SaveLoad.SaveMusicSlider (volume);
	}

	void LoadSettings()
	{
		if (PlayerPrefs.HasKey ("Graphic")) {
			graphicSlider.slider.value = SaveLoad.LoadGraphicSlider ();

		} else {
			graphicSlider.LoadDefaultSettings ();
			SaveLoad.SaveGraphicSlider ((int)graphicSlider.slider.value);
		}

		graphicSlider.text.text = QualitySettings.names [(int)graphicSlider.slider.value];

		if (PlayerPrefs.HasKey ("Music")) {
			musicSlider.slider.value = SaveLoad.LoadMusicSlider ();

		} else {
			musicSlider.LoadDefaultSettings ();
			SaveLoad.SaveMusicSlider (musicSlider.defultValue);
		}

		if (PlayerPrefs.HasKey ("Sounds")) {
			soundsSlider.slider.value = SaveLoad.LoadSoundsSlider ();
		} else {
			soundsSlider.LoadDefaultSettings ();
			SaveLoad.SaveSoundsSlider (soundsSlider.slider.value);
		}
	}
}
