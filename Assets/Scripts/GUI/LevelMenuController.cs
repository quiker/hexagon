using UnityEngine;
using System.Collections;
using System;

public class LevelMenuController : AbstractPanelMenu {
	// When level will be select
	void onLevelSelected(GameObject go) {
		Game.GetInstance().MenuStartLevel(1);
	}
	
	// When button will be create
	void onButtonDraw(Transform button) {	
		int levelIndex = Convert.ToInt32(button.name);
		UILabel uiLabel= button.Find("levelIndex").GetComponent<UILabel>();
				
		if (uiLabel != null) { 
			if (SettingsContainer.GetLevelStars(levelIndex) > 0 || levelIndex == 1){
				// Draw a level index
			   	uiLabel.text = levelIndex.ToString();
				
				// Draw count of stars
			}else{
				// Draw a lock in the button
				uiLabel.text = "Locked";
			}
		}
	}
	
}

