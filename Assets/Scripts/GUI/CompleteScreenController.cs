using UnityEngine;
using System.Collections;

public class CompleteScreenController : AbstractPanelMenu {
	public UILabel scoreLabel = null;
		
	void onNextLevelClick(GameObject button) {
		Game.GetInstance().MenuNextLevel();
	}
	
	void onRestartClick(GameObject button) {
		Game.GetInstance().MenuRestart();
	}
		
	public void updateScore(int score) {
		if (scoreLabel != null) {
			scoreLabel.text = score.ToString();	
		}
	}
}
