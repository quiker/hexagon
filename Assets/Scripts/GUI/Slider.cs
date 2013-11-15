using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {
	public Texture[] slides;
	public int currentSlideIndex = 0;
		
	public void nextSlide() {
		slideByIndex(++currentSlideIndex);
	}	
	
	public void prevSlide() {
		slideByIndex(--currentSlideIndex);
	}
	
	public void slideByIndex(int index) {
        currentSlideIndex = index;
		renderer.material.mainTexture = slides[currentSlideIndex];
	}
	
	public int getSlidersCount() {
		return slides.Length;
	}
	
	public int getCurrentSlideIndex() {
		return currentSlideIndex; 
	}
}
