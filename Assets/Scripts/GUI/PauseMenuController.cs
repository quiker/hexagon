using UnityEngine;
using System.Collections;

public class PauseMenuController : AbstractPanelMenu {
	public UICheckbox songCheckbox = null;
			
	public void onBackToGameClick() {
		Game.GetInstance().MenuResume();
	}
	
	public void onMainMenuClick() {
		Game.GetInstance().MenuMainMenu();
	}
	
	public void onMuteChecked(bool isChecked) {
		if (songCheckbox != null) {
			Game.GetInstance().SetMute(!isChecked);
		}
	}
	
	public void onRestartGameClick() {
		Game.GetInstance().MenuRestart();
	}
	
	void Start() {
		if (songCheckbox != null) {
			songCheckbox.startsChecked = SettingsContainer.GetMusicFlag();
		}
	}
	
	void OnEnable() {
		if (songCheckbox != null) {
			songCheckbox.isChecked = SettingsContainer.GetMusicFlag();
		}
	}
}
