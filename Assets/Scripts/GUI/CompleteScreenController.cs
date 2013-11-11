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
		if (scoreLabel != null) {
			scoreLabel.text = "Result: " + score.ToString();	
		}
	}
	
	public void updateStars(int countStars) {
		if (stars != null) {
			stars.updateStars(countStars);
		}
	}
}
