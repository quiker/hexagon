using UnityEngine;
using System.Collections;


public class UIController : MonoBehaviour {
	public UILabel scoreLabel = null;
	
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
}
