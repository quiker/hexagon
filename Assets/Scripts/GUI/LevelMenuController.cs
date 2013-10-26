using UnityEngine;
using System.Collections;

public class LevelMenuController : MonoBehaviour {
	void onSelectedLevel(GameObject go) {
		/*
		 * go.name - название кнопки
		 * */
		GameController.loadLevel(go.name);
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			GameController.mainMenuScene();
		}
	}
}
