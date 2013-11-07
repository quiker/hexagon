﻿using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	
	private static Game _instance = null;
	
	protected Game () {}
	
	public static Game GetInstance()
	{
		if (_instance == null) {
			_instance = (Game)FindObjectOfType(typeof(Game));
 
			if ( FindObjectsOfType(typeof(Game)).Length > 1 )
			{
				Debug.LogError("[Singleton] Something went really wrong " +
					" - there should never be more than 1 singleton!" +
					" Reopenning the scene might fix it.");
			}
		}
		if (_instance == null) {
			_instance = new Game();
		}
		return _instance;	
	}
	
	public AbstractPanelMenu mainMenuPanel;
	public AbstractPanelMenu selectLevelPanel;
	public AbstractPanelMenu tutorialPanel;
	public AbstractPanelMenu scoreTablePanel;
	public AbstractPanelMenu settingsPanel;
	public AbstractPanelMenu completePanel;
	public AbstractPanelMenu failPanel;
	public AbstractPanelMenu pauseMenu = null;
	
	public AudioPlayer mainThemeSong = null;	
	public GameObject input = null;
	public bool musicOnPauseMenu = true;
	
	public LevelController levelController;
	
	private int level = 0; 
	private string mode = ""; 
	private string state = "";
	
	void Start() {
		mainThemeSong.setMute(!SettingsContainer.GetMusicFlag());
		
		//////		
		MenuMainMenu();
		mode = "level";
		level = 1;
	}
	
	public int GetCurrentLevel()
	{
		return level;
	}
	
	public void MenuMainMenu()
	{
		ActivatePanel("mainMenu");
		SendMessageToAllGameObjects("onMainMenuShow");
	}
	
	public void MenuLevelScreen()
	{
		mode = "level";
		ActivatePanel("selectLevel");
		SendMessageToAllGameObjects("onSelectLevelShow");
	}
	public void MenuSurvivalMode()
	{
		mode = "survival";
		MenuPause();
		//---------------------
	}
	public void MenuTutorial()
	{
		ActivatePanel("tutorial");
		SendMessageToAllGameObjects("onTutorialShow");
	}
	public void MenuSettings()
	{
		ActivatePanel("settings");
		SendMessageToAllGameObjects("onSettingsShow");
	}
	public void Quit()
	{
		Debug.Log("Quit");
		Application.Quit();
	}
	public void MenuStartLevel(int level)
	{
		this.level = level;
		MenuRestart();
	}
	public void MenuScoreTable()
	{
		ActivatePanel("scoreTable");
		GameObject gol;
		GameObject gov;
		for (int i = 0; i < 10; i++) {
			gol = GameObject.FindGameObjectWithTag("SCORE_"+i+"_LABEL");
			gov = GameObject.FindGameObjectWithTag("SCORE_"+i+"_VALUE");
			SettingsContainer.GetScoreTableLabel(i); //
			SettingsContainer.GetScoreTableValue(i); //
		}
		SendMessageToAllGameObjects("onScoreTable");
	}
	public void MenuPause()
	{
		Time.timeScale = 0;
		ActivatePanel("pause");
		
		if (input != null) {
			input.SetActive(false);
		}
		
		/* On / Off music */
		if (mainThemeSong != null) {
			mainThemeSong.setMute(!musicOnPauseMenu || !SettingsContainer.GetMusicFlag());
		}		
		SendMessageToAllGameObjects("onLevelPaused", true);
	}
	public void MenuResume()
	{
		Time.timeScale = 1;
		Debug.Log ("Resume");
		ActivatePanel("playing");
		
		if (input != null) {
			input.SetActive(true);
		}
		/* On / Off music */
		if (mainThemeSong != null) {
			mainThemeSong.setMute(!SettingsContainer.GetMusicFlag());
		}		
		SendMessageToAllGameObjects("onLevelPaused", false);
	}
	public void MenuRestart()
	{
		SendMessageToAllGameObjects("onLevelStarted", level);
		MenuResume();
	}
	public void MenuNextLevel()
	{
		level++;
		MenuRestart();
	}
	public void CompleteScreen()
	{
		// calc score 
		// calc stars
		// save highscore
		// save score to score table
		ActivatePanel("complete");
	}
	public void FailScreen()
	{
		ActivatePanel("fail");
	}
	
	private void ActivatePanel(string panel)
	{
		if (mainMenuPanel != null) mainMenuPanel.hide();
		if (pauseMenu != null) pauseMenu.hide();
		if (selectLevelPanel != null) selectLevelPanel.hide();
		if (tutorialPanel != null) tutorialPanel.hide();
		if (scoreTablePanel != null) scoreTablePanel.hide();
		if (settingsPanel != null) settingsPanel.hide();
		if (completePanel != null) completePanel.hide();
		if (failPanel != null) failPanel.hide();
		switch (panel)
		{
			case "mainMenu":    mainMenuPanel.show(); break;
			case "pause":       pauseMenu.show();  break;
			case "selectLevel": selectLevelPanel.show(); break;
			case "tutorial":    tutorialPanel.show(); break;
			case "scoreTable":  scoreTablePanel.show(); break;
			case "settings":    settingsPanel.show(); break;
			case "complete":    completePanel.show(); break;
			case "fail":        failPanel.show(); break;
		}
		
		state = panel;
	}
	
	public bool IsPaused()
	{
		return Time.timeScale == 0;
	}
	
	public LevelController GetLevelController()
	{
		return levelController;
	}
	
	/*
	 * Когда сворачиваешь или переходишь на другое окно
	 * */
	void OnApplicationPause(bool pauseStatus) {		
        if (pauseStatus == true && !IsPaused()) {
			MenuPause();
		}
		mainThemeSong.enabled = !pauseStatus;
    }
	
	public void SetMute(bool isMute) {
		if (SettingsContainer.GetMusicFlag() != !isMute) {
			SettingsContainer.SetMusicFlag(!isMute);
			if (mainThemeSong != null) {
				if (musicOnPauseMenu) {
					mainThemeSong.setMute(isMute);
				}
			}
			SendMessageToAllGameObjects("onGameMute", isMute);
		}
	}
	
	private void SendMessageToAllGameObjects(string methodName, object value = null) {
		foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(GameObject))) {
	    	gameObject.SendMessage(methodName, value, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public string getState() {
		return state;
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			switch(getState()) {
				case "mainMenu": Quit(); break;
				case "playing": MenuPause(); break;
				case "pause":  MenuResume(); break;
				default: MenuMainMenu(); break;					
			}
		}
	}
}