using UnityEngine;
using System.Collections;
using System;

abstract public class AbstractTutorialController : AbstractPanelMenu {
	public Slider slider;
	public GameObject nextSlideButton; 
	public GameObject prevSlideButton;
	protected int[] availablesSlides;
	protected int currentSlideIndex = 0;
	
	
	protected void onNextSlideClick(GameObject go) {
		if (currentSlideIndex < availablesSlides.Length - 1) {
			ShowSlideByIndex(currentSlideIndex + 1);
		}
	}
	
	protected void onPrevSlideClick(GameObject go) {
		if (currentSlideIndex > 0) {
			ShowSlideByIndex(currentSlideIndex - 1);
		}
	}
	
	public void SetAvailablesSlides(int[] slides) {
		availablesSlides = slides;
		
		Debug.Log(currentSlideIndex.ToString());
		ShowSlideByIndex(currentSlideIndex);
	}
	
	protected void ShowSlideByIndex(int index) {
		currentSlideIndex = index;
		
		slider.slideByIndex(availablesSlides[currentSlideIndex]);
	}
}