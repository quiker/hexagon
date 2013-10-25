using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	void onSelectedLevel(GameObject go) {
		GameController.loadLevel(go.name);
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			GameController.mainMenu();
		}
	}
}
