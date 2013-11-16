using UnityEngine;
using System.Collections;

public class TutorialMenuController : AbstractTutorialController {	
	public GameObject backButton;
	
	void onNextSlideClick(GameObject go) {
		base.onNextSlideClick(go);
	}
	
	void onPrevSlideClick(GameObject go) {
		base.onPrevSlideClick(go);
	}
			
	void onBackClick(GameObject button) {
		Game.GetInstance().MenuMainMenu();
	}
	
	public override MenuPanel getId() {
		return MenuPanel.Tutorial;
	}
	
	void OnEnable() {
		// get available slides
		SetAvailablesSlides(SettingsContainer.GetAvailableSlides());
	}
}
