using UnityEngine;
using System.Collections;

public class MainMenuController : AbstractPanelMenu {
	public GameObject startButton; 
	public GameObject scoreButton;
	public GameObject survivalModeButton;
	public GameObject tutorialButton;
	public GameObject settingsButton;
	public GameObject quitButton;
	public UILabel gameTitle;
	public GameObject background;
	
	
	void Start() {
		// Hide button quit if current platform is iOS
		quitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
	}
	
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
	
	// Achievements
	void onAchievementsClick() {
		Game.GetInstance().MenuAchievements();
	}
	
	public override MenuPanel getId() {
		return MenuPanel.MainMenu;
	}
}
