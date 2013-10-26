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
	
	void Start() {
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
		if (pauseMenu != null) {
			if (pauseState == true) {
				pauseMenu.show();
			}else{
				pauseMenu.hide();
			}
		}
		
		if (keyboardController != null) {
			keyboardController.enabled = !pauseState;
		}
		
		if (touchController != null) {
			touchController.enabled = !pauseState;
		}
	}
	
	private void onScoreUpdate(int score) {
		if (ui != null) {
			ui.updateScore(score);
		}		
	}
		
	private void onGameMute(bool isMute) {
		if (mainThemeSong != null) {
			if (isMute == true) {
				mainThemeSong.pause();
			}else{
				mainThemeSong.play();
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
    }
	
	/*void OnLevelWasLoaded(int level) {
    	updateLevel(GameController.getLevelName());            
    }*/
}
