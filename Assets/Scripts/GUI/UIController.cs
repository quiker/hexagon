using UnityEngine;
using System.Collections;


public class UIController : MonoBehaviour {
	public UILabel scoreLabel = null;
	
	public void onPauseClick() {
		if ( !GameController.isGamePaused() ) {
			GameController.pause();
		} else {
			GameController.resume();
		}
	}
	
	public void updateScore(int score) {
		if (scoreLabel != null) {
			scoreLabel.text = "Score: " + score.ToString();
		}
	}
}
