using UnityEngine;
using System.Collections;
using System;

public class LevelController : MonoBehaviour {
	public Core core = null;
	public PauseMenuController pauseMenu = null;
	public UIController ui = null;
	public AudioPlayer mainThemeSong = null;	
	
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
	
	private void onLevelStarted(string level) {
		updateLevel(level);
	}
	
	private void onGamePaused() {
		pauseMenu.show();
	}
	
	private void onGameResumed() {
		pauseMenu.hide();
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
}
