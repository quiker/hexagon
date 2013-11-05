using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	public void onNewGameClick() {
		Game.GetInstance().MenuLevelScreen();
	}
	
	public void onQuitClick() {
		Game.GetInstance().Quit();
	}
	
	void Update() {
		if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKey(KeyCode.Escape)){
				Game.GetInstance().Quit();
            }
		}
	}		
}
