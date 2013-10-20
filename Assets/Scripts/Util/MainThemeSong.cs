using UnityEngine;
using System.Collections;

public class MainThemeSong : MonoBehaviour {	
	
	void Awake() {
		play();
	}
		
	public void pause() {
		audio.Pause();
	}
	
	
	public void play() {
		if (SettingsContainer.GetMusicFlag()) {
			audio.Play();
		}
	}
}
