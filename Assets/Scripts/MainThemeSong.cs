using UnityEngine;
using System.Collections;

public class MainThemeSong : MonoBehaviour {	
	public void pause() {
		audio.Pause();
	}
	
	public void play() {
		audio.Play();
	}
}
