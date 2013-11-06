using UnityEngine;
using System.Collections;

public class TutorialMenuController : MonoBehaviour {
	public UIPanel panel = null;
	
	void onBackClick(GameObject button) {
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
