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
		sendMessageToAllGameObjecs("onScoreUpdate", score);
	}
	
	void Start() {
		restart();
	}
	
	public static void resume() {		
		if ( isGamePaused() ) {
			Time.timeScale = 1;
			sendMessageToAllGameObjecs("onGameResumed", null);
		}
	}
	
	public static void pause() {
		if ( !isGamePaused() ) {
			Time.timeScale = 0;
			sendMessageToAllGameObjecs("onGamePaused", null);
		}
	}
	
	public static void restart() {
		setScore(0);
		resume();
		sendMessageToAllGameObjecs("onGameRestarted", null);
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
	
	public static void levelChange() {
		Application.LoadLevel("levelChange");
	}
	
	public static void mainMenu() {
		Application.LoadLevel("mainMenu");
	}
	
	public static int getCurrentLevel() {
		return Application.loadedLevel;
	}
	
	public static void failScreen() {
		//Time.timeScale = 0;
		Debug.Log("LOOOSE!");
	}
	
	public static void setMute(bool isMute) {
		SettingsContainer.SetMusicFlag(!isMute);
		sendMessageToAllGameObjecs("onGameMute", isMute);
	}
	
	private static void sendMessageToAllGameObjecs(string methodName, object value) {
		foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject))) {
	    	gameObject.SendMessage(methodName, value, SendMessageOptions.DontRequireReceiver);
		}
	}
	
}
