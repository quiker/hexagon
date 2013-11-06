using UnityEngine;
using System.Collections;

public class CompleteScreenController : MonoBehaviour {
	public UIPanel panel = null;
	public UILabel scoreLabel = null;
		
	void onNextLevelClick(GameObject button) {
		Game.GetInstance().MenuNextLevel();
	}
	
	void onRestartClick(GameObject button) {
		Game.GetInstance().MenuRestart();
	}
	
	void onMainMenuClick(GameObject button) {
		Game.GetInstance().MenuMainMenu();
	}
	
	public void updateScore(int score) {
		if (scoreLabel != null) {
			scoreLabel.text = score.ToString();	
		}
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
