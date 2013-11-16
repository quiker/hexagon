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
	
	// Test
	void Start() {
		// get available slides
		SetAvailablesSlides(new int[] {1, 2, 3});
	}
}
