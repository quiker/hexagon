using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour {
	public UIPanel pausePanel = null;
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
		songCheckbox.isChecked = SettingsContainer.GetMusicFlag();
		pausePanel.gameObject.SetActive(true);
	}
	
	public void hide() {
		pausePanel.gameObject.SetActive(false);
	}
	
	public void setShow(bool isShow) {
		if (isShow) {
			show();
		}else{
			hide();
		}
	}
}
