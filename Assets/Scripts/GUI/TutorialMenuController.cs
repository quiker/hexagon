using UnityEngine;
using System.Collections;

public class TutorialMenuController : AbstractPanelMenu {
	public Slider slider;
	public GameObject nextSlideButton; 
	public GameObject prevSlideButton;
	
	void onNextSlideClick(GameObject go) {
		slider.nextSlide();
	}
	
	void onPrevSlideClick(GameObject go) {
		slider.prevSlide();
	}
}
