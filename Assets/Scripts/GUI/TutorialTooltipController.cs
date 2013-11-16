using UnityEngine;
using System.Collections;
using System;

public class TutorialTooltipController : AbstractTutorialController {	
	void onNextSlideClick(GameObject go) {
		if (currentSlideIndex >= availablesSlides.Length - 1) {
			Game.GetInstance().MenuResume();
		}else{
			base.onNextSlideClick(go);
		}
	}
	
	void onPrevSlideClick(GameObject go) {
		base.onPrevSlideClick(go);
	}
	
	public override MenuPanel getId() {
		return MenuPanel.TutorialTooltip;
	}
	
}
