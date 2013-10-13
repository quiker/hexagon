using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	public void onNewGameClick() {
		Application.LoadLevel("hex");
	}
	
	public void onQuitClick() {
		Application.Quit();
	}
}
