using UnityEngine;
using System.Collections;

public class TutorialMenuController : AbstractPanelMenu {
	public Slider slider;
	public GameObject nextSlideButton; 
	public GameObject prevSlideButton;
	
	void onNextSlideClick(GameObject go) {
		if (/*next slide is open*/ true && slider.getCurrentSlideIndex() < slider.getSlidersCount() - 1) {
			slider.nextSlide();
		}
	}
	
	void onPrevSlideClick(GameObject go) {
		if (slider.getCurrentSlideIndex() > 0) {
			slider.prevSlide();
		}
	}
	
	public void showSlideByIndex(int index) {
		slider.slideByIndex(index);
		
		// discovery slide by index
	}
			
	void onBackClick(GameObject button) {
		if (Game.GetInstance().GetGameState() == Game.GameState.Pause) {
			Game.GetInstance().MenuResume();
		}else{
			Game.GetInstance().MenuMainMenu();
		}
	}
	
	public override MenuPanel getId() {
		return MenuPanel.Tutorial;
	}
}
