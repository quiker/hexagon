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
		GameController.mainMenu();
	}
	
	public void OnMuteClick() {
		GameController.setMute(!songCheckbox.isChecked);
	}
	
	public void onRestartGameClick() {
		GameController.restart();
	}
	
	void onGamePaused() {
		songCheckbox.isChecked = SettingsContainer.GetMusicFlag();
		pausePanel.gameObject.SetActive(true);
	}
	
	
	void onGameResumed() {
		pausePanel.gameObject.SetActive(false);
	}
}
