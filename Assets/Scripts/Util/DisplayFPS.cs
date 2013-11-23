using UnityEngine;
using System.Collections;

public class DisplayFPS : MonoBehaviour {
	public UILabel label;
	
	// Update is called once per frame
	void Update () {
		if (label != null) label.text = "FPS: " + ((int)GetFPS()).ToString();
	}
	
	public static float GetFPS() {
		return 1.0f / RealTime.deltaTime; 
	}
}
