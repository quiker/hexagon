using UnityEngine;
using System.Collections;

public class CompleteScreenController : AbstractPanelMenu {
	public UILabel scoreLabel = null;
	public Stars stars = null;
		
	void onNextLevelClick(GameObject button) {
		Game.GetInstance().MenuNextLevel();
	}
	
	void onRestartClick(GameObject button) {
		Game.GetInstance().MenuRestart();
	}
		
	public void updateScore(int score) {
		if (score > SettingsContainer.GetLevelMaxScore(Game.GetInstance().GetCurrentLevel())) {
			scoreLabel.text = "[00BB00]New highscore:[-] " + score.ToString();
		}else{
			scoreLabel.text = "Result: " + score.ToString();
		}
	}
	
	public void updateStars(int countStars) {
		if (stars != null) {
			stars.updateStars(countStars);
		}
	}
}
