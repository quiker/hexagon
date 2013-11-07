using UnityEngine;
using System.Collections;

public class FailScreenController : AbstractPanelMenu {
	void onRestartClick(GameObject button) {
		Game.GetInstance().MenuRestart();
	}
}
