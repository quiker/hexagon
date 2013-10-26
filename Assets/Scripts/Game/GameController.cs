using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private static int score = 0;
	private static string level = ""; 
	
	/* Add value to score*/
	public static void addToScore(int value) {
		setScore(score + value);
	}
	
	public static void setScore(int value) {
		score = value;
		sendMessageToAllGameObjects("onScoreUpdate", score);
	}
	
	public static void resume() {		
		if ( isGamePaused() ) {
			Time.timeScale = 1;
			sendMessageToAllGameObjects("onLevelPaused", false);
		}
	}
	
	public static void pause() {
		if ( !isGamePaused() ) {
			Time.timeScale = 0;
			sendMessageToAllGameObjects("onLevelPaused", true);
		}
	}
	
	public static void restart() {
		setScore(0);
		sendMessageToAllGameObjects("onLevelRestarted", level);
		resume();
	}
	
	public static void quit() {
		Application.Quit();
	}
	
	public static bool isGamePaused() {
		return (Time.timeScale == 0);
	}
	
	public static void loadLevel(string levelName) {
		/*
		 * levelName - название левела. 
		 * "hex" это название сцены.
		 * Сам левел будет создаваться после загрузки сцены.
		 * */
		level = levelName;
		Application.LoadLevel("hex");
	}
	
	public static void levelChangeScene() {
		Application.LoadLevel("levelChange");
	}
	
	public static void mainMenuScene() {
		Application.LoadLevel("mainMenu");
	}
	
	public static int getCurrentLevelIndex() {
		return Application.loadedLevel;
	}
	
	public static string getLevelName() {
		return level;
	}
	
	public static void failScreen() {
		//Time.timeScale = 0;
		Debug.Log("LOOOSE!");
	}
	
	public static void completeScreen() {
		//Time.timeScale = 0;
		Debug.Log("WINNER!");
	}
	
	public static void setMute(bool isMute) {
		if (SettingsContainer.GetMusicFlag() != !isMute) {
			SettingsContainer.SetMusicFlag(!isMute);
			sendMessageToAllGameObjects("onGameMute", isMute);
		}
	}
	
	private static void sendMessageToAllGameObjects(string methodName, object value) {
		foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject))) {
	    	gameObject.SendMessage(methodName, value, SendMessageOptions.DontRequireReceiver);
		}
	}
	
}
