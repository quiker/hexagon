using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	public UIPanel panel = null;
	
	// Start
	void onStartClick() {
		Game.GetInstance().MenuLevelScreen();
	}
	
	// Quit
	void onQuitClick() {
		Game.GetInstance().Quit();
	}
	
	// SurvivalMode
	void onSurvivalModeClick() {
		Game.GetInstance().MenuSurvivalMode();
	}
	
	// Settings
	void onSettingsClick() {
		Game.GetInstance().MenuSettings();
	}
	
	// Tutorial
	void onTutorialClick() {
		Game.GetInstance().MenuTutorial();
	}
	
	// Score
	void onScoreClick() {
		Game.GetInstance().MenuScoreTable();
	}
	
	void Update() {
		/*if (Input.GetKey(KeyCode.Escape)){
			Game.GetInstance().Quit();
        }*/
	}		
	
	public void hide() {
		if (panel != null) {
			panel.gameObject.SetActive(false);
		}
	}
	
	public void show() {
		if (panel != null) {
			panel.gameObject.SetActive(true);
		}
	}
}
