using UnityEngine;
using System.Collections;

abstract public class AbstractPanelMenu : MonoBehaviour {
	public UIPanel panel = null; 
	
	public void hide() {
		if (panel != null) {
			enabled = false;
			panel.gameObject.SetActive(false);
		}
	}
	
	public void show() {
		if (panel != null) {
			enabled = true;
			panel.gameObject.SetActive(true);
		}
	}
	
	void onBackClick(GameObject button) {
		Game.GetInstance().MenuMainMenu();
	}
}
