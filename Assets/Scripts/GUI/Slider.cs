using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {
	public SliderFactory factory;
	
	public void slideByIndex(int index) {
		renderer.material.mainTexture = factory.GetTextureByIndex(index);
	}
}
