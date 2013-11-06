using UnityEngine;
using System.Collections;

public class FailScreenController : MonoBehaviour {
	public UIPanel panel = null;
	
	void onRestartClick(GameObject button) {
		Game.GetInstance().MenuRestart();
	}
		
	void onMainMenuClick(GameObject button) {
		Game.GetInstance().MenuMainMenu();
	}
	
	public void hide() {
		if (panel != null) {
			panel.gameObject.SetActive(false);
		}
	}
	
	public void show() {
		if (panel != null) {
			panel.gameObject.SetActive(true);
		}
	}
}
