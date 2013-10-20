using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	public void onNewGameClick() {
		Application.LoadLevel("hex");
	}
	
	public void onQuitClick() {
		Debug.Log("Btn quit click!");
		Application.Quit();
		//System.Diagnostics.Process.GetCurrentProcess().Kill();
	}
	
	void Update() {
		if (Application.platform == RuntimePlatform.Android) {
            if (Input.GetKey(KeyCode.Escape)){
				Application.Quit();
            }
		}
	}		
}
