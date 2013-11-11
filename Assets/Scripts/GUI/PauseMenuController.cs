using UnityEngine;
using System.Collections;

public class PauseMenuController : AbstractPanelMenu {
	public UICheckbox songCheckbox = null;
			
	public void onBackToGameClick() {
		Game.GetInstance().MenuResume();
	}
	
	public void onMainMenuClick() {
		Game.GetInstance().MenuMainMenu();
	}
	
	public void onMuteChecked(GameObject go) {
		if (songCheckbox != null) {
			Game.GetInstance().SetMute(songCheckbox.isChecked);
		}
	}
	
	public void onRestartGameClick() {
		Game.GetInstance().MenuRestart();
	}
	
	void OnGUI() {
		if (songCheckbox != null) {
			songCheckbox.isChecked = SettingsContainer.GetMuteFlag();
		}
	}
}
