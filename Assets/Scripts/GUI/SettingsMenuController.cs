﻿using UnityEngine;
using System.Collections;

public class SettingsMenuController : AbstractPanelMenu {	
	public UICheckbox mute;
	public UISlider musicValue;
	public UISlider soundsValue;
	
	private bool Show = false;
	
	void onMuteChecked(GameObject go) {
		if (mute != null) {
			Game.GetInstance().SetMute(mute.isChecked);
		}
	}
			
	void onMusicValueChange(float value) {
		Game.GetInstance().SetMusicValue(value);
	}
	
	void onSoundValueChange(float value) {
		Game.GetInstance().SetSoundValue(value);
	}
	
	void Start() {
		if (musicValue != null) {
			musicValue.sliderValue = SettingsContainer.GetMusicValue();
			musicValue.initialValue = SettingsContainer.GetMusicValue();
		}
		
		if (soundsValue != null) {
			soundsValue.sliderValue = SettingsContainer.GetSoundValue();
			soundsValue.initialValue = SettingsContainer.GetSoundValue();
		}
	}
	
	void OnGUI() {
		if (mute != null) {
			mute.isChecked = SettingsContainer.GetMuteFlag();
		}
	}
}
