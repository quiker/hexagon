using UnityEngine;
using System.Collections;

abstract public class AbstractPanelMenu : MonoBehaviour {
	public UIPanel panel = null; 
	
	public void hide() {
		if (panel != null) {
			panel.gameObject.SetActive(false);
			OnHide();
		}
	}
	
	public void show() {
		if (panel != null) {
			panel.gameObject.SetActive(true);
			OnShow();
		}
	}
	
	void onBackClick(GameObject button) {
		Game.GetInstance().MenuMainMenu();
	}
	
	abstract public MenuPanel getId();
	
	protected virtual void OnShow() {
	}
	
	protected virtual void OnHide() {
	}
}
