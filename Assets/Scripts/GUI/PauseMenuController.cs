using UnityEngine;
using System.Collections;

public class PauseMenuController : AbstractPanelMenu {
	public UICheckbox songCheckbox = null;
	
	void Start() {
		songCheckbox.startsChecked = SettingsContainer.GetMusicFlag();
	}
	
	public void onBackToGameClick() {
		Game.GetInstance().MenuResume();
	}
	
	public void onMainMenuClick() {
		Game.GetInstance().MenuMainMenu();
	}
	
	public void OnMuteClick() {
		Game.GetInstance().SetMute(!songCheckbox.isChecked);
	}
	
	public void onRestartGameClick() {
		Game.GetInstance().MenuRestart();
	}
	
	public void show() {
		base.show();
		
		if (songCheckbox != null) {
			songCheckbox.isChecked = SettingsContainer.GetMusicFlag();
		}
	}
	
	public void setShow(bool isShow) {
		if (isShow) {
			show();
		}else{
			hide();
		}
	}
}
