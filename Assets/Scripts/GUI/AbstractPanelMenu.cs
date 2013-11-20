using UnityEngine;
using System.Collections;

abstract public class AbstractPanelMenu : MonoBehaviour {
	public UIPanel panel = null; 
	
	public void hide() {
		if (panel != null) {
			OnHide();
			panel.gameObject.SetActive(false);
		}
	}
	
	public void show() {
		if (panel != null) {
			OnShow();
			panel.gameObject.SetActive(true);
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
