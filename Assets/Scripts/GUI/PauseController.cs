using UnityEngine;
using System.Collections;
using System;

public class PauseController : MonoBehaviour {
	public UIPanel pausePanel = null;
	public Core core = null;
	public MainThemeSong themeSong = null;
	public UICheckbox songCheckbox = null;
	
	void Start() {
		resume();
	}
		
	public void onPauseClick() {
		if ( !isGamePaused() ) {
			pause();
		}
	}
	
	public void onBackToGameClick() {
		resume();
	}
	
	public void onMainMenuClick() {
		Application.LoadLevel("mainMenu");
	}
	
	public void OnMuteClick() {
		SettingsContainer.SetMusicFlag(songCheckbox.isChecked);
	}
	
	public void onRestartGameClick() {
		if (core != null) {
			core.Reinit();
			resume();
		}else{
			throw new Exception("Core is NULL!");
		}
	}
	
	// Set state pause/resume
	private void setPause(bool itPause) {
		if (pausePanel != null) {
			pausePanel.gameObject.SetActive(itPause);
			songCheckbox.isChecked = SettingsContainer.GetMusicFlag();
			Time.timeScale = (itPause ? 0 : 1);
		}else{
			throw new Exception("Pause panel is null");
		}
	}
	
	public void pause() {
		if (themeSong != null) {
			themeSong.pause();
		}
		
		setPause(true);
	}
	
	public void resume() {
		if (themeSong != null) {
			themeSong.play();
		}
		
		setPause(false);
	}
	
	public bool isGamePaused() {
		return (Time.timeScale == 0);
	}
	
}