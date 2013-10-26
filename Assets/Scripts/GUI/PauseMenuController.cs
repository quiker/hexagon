using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour {
	public UIPanel pausePanel = null;
	public UICheckbox songCheckbox = null;
	
	void Start() {
		songCheckbox.startsChecked = SettingsContainer.GetMusicFlag();
	}
	
	public void onBackToGameClick() {
		GameController.resume();
	}
	
	public void onMainMenuClick() {
		GameController.mainMenuScene();
	}
	
	public void OnMuteClick() {
		GameController.setMute(!songCheckbox.isChecked);
	}
	
	public void onRestartGameClick() {
		GameController.restart();
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
