using UnityEngine;
using System.Collections;

public class ScoreMenuController : AbstractPanelMenu {
	void Update() {
		if (Input.GetKey(KeyCode.Escape)){
			Game.GetInstance().MenuMainMenu();
        }
	}
}
