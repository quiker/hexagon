using UnityEngine;
using System.Collections;
using System;

public class LevelController : MonoBehaviour {
	public Core core = null;
	public PauseMenuController pauseMenu = null;
	public UIController ui = null;
	public AudioPlayer mainThemeSong = null;	
	public KeyboardController keyboardController = null;
	public TouchController touchController = null;
	public bool musicOnPauseMenu = true;
	
	void Start() {
		mainThemeSong.setMute(!SettingsContainer.GetMusicFlag());
		updateLevel(GameController.getLevelName());
	}
	
	private void updateLevel(string level) {
		if (core != null) {
			/*
			 * level = LevelFactory.getLevel(level);
			 * core.setLevel(level);
			 * 
			 * */
			core.Reinit();
		}else{
			throw new Exception("Core is empty!");
		}
	}
	
	private void onLevelRestarted(string level) {
		updateLevel(level);
	}	
	
	private void onLevelPaused(bool pauseState) {
		/* show / hide pauseMenu*/
		if (pauseMenu != null) {
			pauseMenu.setShow(pauseState);
		}
		
		/* On / Off keyboardController */
		if (keyboardController != null) {
			keyboardController.enabled = !pauseState;
		}
		
		/* On / Off touchController */
		if (touchController != null) {
			touchController.enabled = !pauseState;
		}
		
		/* On / Off music */
		if (mainThemeSong != null) {
			if (pauseState == true) {
				if (SettingsContainer.GetMusicFlag()) {
					mainThemeSong.setMute(!musicOnPauseMenu);
				}
			}else{
				mainThemeSong.setMute(!SettingsContainer.GetMusicFlag());
			}
		}		
	}
	
	private void onScoreUpdate(int score) {
		if (ui != null) {
			ui.updateScore(score);
		}		
	}
		
	private void onGameMute(bool isMute) {
		if (mainThemeSong != null) {
			if (musicOnPauseMenu == true) {
				mainThemeSong.setMute(isMute);
			}
		}
	}	
	
	/*
	 * Когда сворачиваешь или переходишь на другое окно
	 * */
	void OnApplicationPause(bool pauseStatus) {		
        if (pauseStatus == true) {
			GameController.pause();
		}
		
		mainThemeSong.enabled = !pauseStatus;
    }
	
	/*
	 * Когда перешел на эту сцену через Application.LoadLevel("hex")
	 * */
	void OnLevelWasLoaded(int level) {
		Time.timeScale = 1;
    }
}
