using UnityEngine;
using System.Collections;


public class UIController : MonoBehaviour {
	public UILabel scoreLabel = null;
	public UILabel highscore = null;
	public UILabel figureLimit = null;
	
	public void onPauseClick() {
		if ( !Game.GetInstance().IsPaused() ) {
			Game.GetInstance().MenuPause();
		} else {
			Game.GetInstance().MenuResume();
		}
	}
	
	public void updateScore(int score) {
		if (scoreLabel != null) {
			scoreLabel.text = "Score: " + score.ToString();
		}
	}
	
	public void updateHighscore(int score) {
		highscore.text = score > 0 ? "[00BB00]Highscore:[-]: " + score.ToString() : "";
	}
	
	public void updateFigureLimit(int limit) {
		figureLimit.text = limit > 0 ? "Figure limit: " + limit.ToString() : "";
	}
}
