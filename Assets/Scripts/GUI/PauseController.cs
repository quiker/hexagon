using UnityEngine;
using System.Collections;
using System;

public class PauseController : MonoBehaviour {
	public static void pause() {
		if ( !isGamePaused() ) {
			Time.timeScale = 0;
			
			foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject))) {
		    	gameObject.SendMessage("onGamePaused", SendMessageOptions.DontRequireReceiver);
    		}
		}
	}
	
	public static void resume() {
		if ( isGamePaused() ) {
			Time.timeScale = 1;
			
			foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject))) {
		    	gameObject.SendMessage("onGameResumed", SendMessageOptions.DontRequireReceiver);
    		}
		}
	}
	
	public static bool isGamePaused() {
		return (Time.timeScale == 0);
	}
	
}