using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {
	public Texture[] slides;
	public int currentSlideIndex = 0;
	
	void Start() {
		slideByIndex(currentSlideIndex);
	}
	
	public void nextSlide() {
		slideByIndex(++currentSlideIndex);
	}	
	
	public void prevSlide() {
		slideByIndex(--currentSlideIndex);
	}
	
	public void slideByIndex(int index) {
        renderer.material.mainTexture = slides[index];
	}
	
	public int getSlidersCount() {
		return slides.Length;
	}
}
