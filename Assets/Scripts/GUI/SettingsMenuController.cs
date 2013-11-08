using UnityEngine;
using System.Collections;

public class SettingsMenuController : AbstractPanelMenu {	
	public UICheckbox mute;
	public UISlider musicValue;
	public UISlider soundsValue;
	
	void onMuteChecked(bool isChecked) {
		if (mute != null) {
			Game.GetInstance().SetMute(!isChecked);
		}
	}
			
	void onMusicValueChange(float value) {
		Game.GetInstance().mainThemeSong.audio.volume = value;
	}
	
	void onSoundValueChange(float value) {
		foreach (AudioSource aud in GameObject.FindObjectsOfType(typeof(AudioSource))) {
			aud.volume = value;
		}
		
		Game.GetInstance().mainThemeSong.audio.volume = musicValue.sliderValue;
	}
	
	
	void Start() {
		if (mute != null) {
			mute.startsChecked = SettingsContainer.GetMusicFlag();
		}
	}
	
	void OnEnable() {
		if (mute != null) {
			mute.isChecked = SettingsContainer.GetMusicFlag();
		}
		
		if (musicValue != null) {
			//musicValue.sliderValue = SettingsContainer.GetMusicValue();
		}
		
		if (soundsValue != null) {
			//soundsValue.sliderValue = SettingsContainer.GetSoundValue();
		}
	}
}
