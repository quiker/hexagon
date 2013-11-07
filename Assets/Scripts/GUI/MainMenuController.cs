using UnityEngine;
using System.Collections;

public class MainMenuController : AbstractPanelMenu {
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
}
