using UnityEngine;
using System.Collections;

public class AchievementsMenuController : AbstractPanelMenu {
	void Update() {
		if (Input.GetKey(KeyCode.Escape)){
			Game.GetInstance().MenuMainMenu();
        }
	}
	
	public override MenuPanel getId() {
		return MenuPanel.Achievements;
	}
}
