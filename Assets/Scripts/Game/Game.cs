using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	public enum GameMode {Normal, Survival}; 
	public enum GameState {Menu, Playing, Pause};
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
	public AbstractPanelMenu[] panels;
		
	public UIController ui;
	
	public AudioPlayer mainThemeSong = null;	
	public GameObject input = null;
	public bool musicOnPauseMenu = true;
	
	public LevelController levelController;
	
	private int level = 0; 
	private GameMode mode; 
	private GameState gameState;
	private AbstractPanelMenu activePanel;
	
	void Start()
	{
		EnableAudioListener(!SettingsContainer.GetMuteFlag());
		MenuMainMenu();
	}
	
	public int GetCurrentLevel()
	{
		return level;
	}
	
	public void MenuMainMenu()
	{
		ui.gameObject.SetActive(false);
		StopTime();
		gameState = GameState.Menu;
		ActivatePanel(MenuPanel.MainMenu);
		SendMessageToAllGameObjects("onMainMenuShow");
	}
	
	public void MenuLevelScreen()
	{
		StopTime();
		mode = GameMode.Normal;
		ActivatePanel(MenuPanel.Level);
		SendMessageToAllGameObjects("onSelectLevelShow");
	}
	
	public void MenuSurvivalMode()
	{
		PlayerPrefs.DeleteAll();
		AchievementManager.GetInstance().ClearTrackingVars();
		//mode = GameMode.Survival;
		//MenuPause();
		//---------------------
	}
	
	public void MenuTutorial(int[] slides)
	{
		ui.gameObject.SetActive(false);
		StopTime();
		TutorialTooltipController tooltip = GetPanelByName(MenuPanel.TutorialTooltip) as TutorialTooltipController;
		tooltip.SetAvailablesSlides(slides);
		
		ActivatePanel(tooltip);
	}
	
	public void MenuTutorial()
	{
		StopTime();
		ActivatePanel(MenuPanel.Tutorial);
		
		SendMessageToAllGameObjects("onTutorialShow");
	}
	
	public void MenuSettings()
	{
		StopTime();
		ActivatePanel(MenuPanel.Settings);
		SendMessageToAllGameObjects("onSettingsShow");
	}
	
	public void Quit()
	{
		Application.Quit();
	}
	
	public void MenuStartLevel(int level)
	{
		StartTime();
		this.level = level;
		MenuRestart();
	}
	
	public void MenuAchievements()
	{
		StopTime();
		ActivatePanel(MenuPanel.Achievements);
		SendMessageToAllGameObjects("onAchievementsTable");
	}
	
	public void MenuPause()
	{
		StopTime();
		gameState = GameState.Pause;
		ActivatePanel(MenuPanel.Pause);
		DisableInput();
		
		/* On / Off music */
		EnableAudioListener(musicOnPauseMenu && !SettingsContainer.GetMuteFlag());
		
		SendMessageToAllGameObjects("onLevelPaused", true);
	}
	
	public void MenuResume()
	{
		Time.timeScale = 1;
		gameState = GameState.Playing;
		StartTime();
		Debug.Log ("Resume");
		HidePanels();
		EnableInput();
		ui.gameObject.SetActive(true);
		
		/* On / Off music */
		EnableAudioListener(!SettingsContainer.GetMuteFlag());		
		
		SendMessageToAllGameObjects("onLevelPaused", false);
	}
	
	public void MenuRestart()
	{
		SendMessageToAllGameObjects("onLevelStarted", level);
		//MenuResume();
	}
	
	public void MenuNextLevel()
	{
		level++;
		MenuRestart();
	}
	
	public void CompleteScreen(int score, int stars)
	{
		StopTime();
		DisableInput();
		CompleteScreenController completePanel = GetPanelByName(MenuPanel.Complete) as CompleteScreenController;
		
		completePanel.updateScore(score);
		completePanel.updateStars(stars);
		
		// save highscore
		// save score to score table
		ActivatePanel(MenuPanel.Complete);
	}
	
	public void FailScreen()
	{
		StopTime();
		DisableInput();
		ActivatePanel(MenuPanel.Fail);
	}
	
	private void ActivatePanel(MenuPanel panelId)
	{
		foreach (AbstractPanelMenu panel in panels) {
			if (panel.getId() == panelId) {
				activePanel = panel;
				panel.show();
			}else{
				panel.hide();
			}
		}
	}
	
	private void ActivatePanel(AbstractPanelMenu panel)
	{
		ActivatePanel(panel.getId());
	}
	
	private AbstractPanelMenu GetPanelByName(MenuPanel panel)
	{
		foreach (AbstractPanelMenu currentPanel in panels) {
			if (currentPanel.getId() == panel) {
				return currentPanel;
			}
		}
		
		return null;
	}
	
	public AbstractPanelMenu GetActivePanel()
	{
		return activePanel;
	}
	
	private void HidePanels()
	{
		foreach (AbstractPanelMenu panel in panels) {
			panel.hide();
		}
		
		activePanel = null;
	}
	
	protected void StopTime()
	{
		Time.timeScale = 0;
	}
	
	protected void DisableInput()
	{
		input.SetActive(false);
	}
	
	protected void EnableInput()
	{
		input.SetActive(true);
	}
	
	protected void StartTime()
	{
		Time.timeScale = 1;
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
	void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus == true && !IsPaused()) {
			MenuPause();
		}
		mainThemeSong.enabled = !pauseStatus;
	}
	
	public void SetMute(bool isMute)
	{
		if (SettingsContainer.GetMuteFlag() != isMute) {
			SettingsContainer.SetMuteFlag(isMute);
			
			if (mainThemeSong != null) {
				if (musicOnPauseMenu) {
					EnableAudioListener(!isMute);
				}
			}
			
			SendMessageToAllGameObjects("onGameMute", isMute);
		}
	}
	
	private void SendMessageToAllGameObjects(string methodName, object value = null)
	{
		foreach (GameObject gameObject in GameObject.FindObjectsOfType(typeof(GameObject))) {
			gameObject.SendMessage(methodName, value, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void EnableAudioListener(bool enabled)
	{
		Camera cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
		cam.audio.enabled = enabled;
		AudioListener.volume = enabled ? 1 : 0;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (GetActivePanel() != null) {
				switch(GetActivePanel().getId()) {
					case MenuPanel.MainMenu: Quit(); break;
					case MenuPanel.Pause: MenuResume(); break;
					case MenuPanel.TutorialTooltip: MenuResume(); break;
					default: MenuMainMenu(); break;					
				}
			}else{
				// is palying
				MenuPause();
			}
		}
	}
	
	public void SetMusicValue(float value)
	{
		if (mainThemeSong != null) {
			mainThemeSong.audio.volume = value;
		}
		
		SettingsContainer.SetMusicValue(value);
	}
	
	public void SetSoundValue(float value)
	{
		float mainThemeSongVolume = mainThemeSong.audio.volume; 
			
		foreach (AudioSource aud in GameObject.FindObjectsOfType(typeof(AudioSource))) {
			aud.volume = value;
		}
		
		SettingsContainer.SetSoundValue(value);
		
		mainThemeSong.audio.volume = mainThemeSongVolume;
	}
	
	public GameMode GetGameMode()
	{
		return mode;
	}
	
	public GameState GetGameState()
	{
		return gameState;
	}
}
