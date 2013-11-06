using UnityEngine;
using System.Collections;

public class SettingsMenuController : MonoBehaviour {
	public UIPanel panel = null;
	
	
	void onAllSoundsChecked(bool isChecked) {
	}
	
	void onSoundsChecked(bool isChecked) {
	}
	
	void onMusicChecked(bool isChecked) {
	}
	
	void onMusicValueChange(float value) {
	}
	
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
