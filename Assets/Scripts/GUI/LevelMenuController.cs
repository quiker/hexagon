using UnityEngine;
using System.Collections;

public class LevelMenuController : MonoBehaviour {
	void onSelectedLevel(GameObject go) {
		Game.GetInstance().MenuStartLevel(1);
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Game.GetInstance().MenuMainMenu();
		}
	}
}
