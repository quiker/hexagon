using UnityEngine;
using System.Collections;
using System;

public class SliderFactory : MonoBehaviour {
	public Texture[] slides;
	
	public Texture GetTextureByIndex(int index) {
		return slides[index];
	}
	
	public int GetSlidersCount() {
		return slides.Length;
	}
}
