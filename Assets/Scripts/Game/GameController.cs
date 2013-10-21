using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private static int score = 0;	
	
	/* Add value to score*/
	public static void addToScore(int value) {
		setScore(score + value);
	}
	
	public static void setScore(int value) {
		score = value;
		
		foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject))) {
	    	gameObject.SendMessage("onScoreUpdate", score, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void Start() {
		restart();
	}
	
	public static void resume() {		
		PauseController.resume();
	}
	
	public static void pause() {
		PauseController.pause();
	}
	
	public static void restart() {
		setScore(0);
		resume();
		
		foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject))) {
	    	gameObject.SendMessage("onGameRestarted", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public static void quit() {
		Application.Quit();
	}
	
	public static bool isGamePaused() {
		return PauseController.isGamePaused();
	}
	
	public static void loadLevel(string level) {
		Application.LoadLevel(level);
	}
	
	public static void mainMenu() {
		Application.LoadLevel("mainMenu");
	}
	
	public static int getCurrentLevel() {
		return Application.loadedLevel;
	}
	
	public static void failScreen() {
		Time.timeScale = 0;
		Debug.Log("LOOOSE!");
	}
	
	public static void setMute(bool isMute) {
		SettingsContainer.SetMusicFlag(!isMute);
		
		foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject))) {
	    	gameObject.SendMessage("onGameMute", isMute, SendMessageOptions.DontRequireReceiver);
		}
	}
	
}
