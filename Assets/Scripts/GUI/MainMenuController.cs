﻿using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	public void onNewGameClick() {
		GameController.loadLevel("hex");
	}
	
	public void onQuitClick() {
		GameController.quit();
	}
	
	void Update() {
		if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKey(KeyCode.Escape)){
				GameController.quit();
            }
		}
	}		
}