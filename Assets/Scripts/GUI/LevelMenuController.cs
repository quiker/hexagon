using UnityEngine;
using System.Collections;
using System;

public class LevelMenuController : AbstractPanelMenu {
	public string EnableSprite = "Glow";
	public string DisableSprite = "Dark";
	public string ButtonCallbackFunctionName = "onLevelSelected";
	
	// When level will be select
	void onLevelSelected(GameObject go) {
		int levelIndex = Convert.ToInt32(go.transform.name);
		Game.GetInstance().MenuStartLevel(levelIndex);
	}
	
	
	private void setButtonEnable(Transform button, bool enabled) {
		UIButtonColor buttonColor   = button.GetComponent<UIButtonColor>();
		UISlicedSprite slicedPrite  = button.transform.Find("SlicedSprite").GetComponent<UISlicedSprite>();
		UIButtonOffset buttonOffset = button.GetComponent<UIButtonOffset>();
		UIButtonSound buttonSound   = button.GetComponent<UIButtonSound>();
		
		buttonColor.enabled  = enabled;
		buttonOffset.enabled = enabled;
		buttonSound.enabled  = enabled;
		slicedPrite.spriteName = enabled ? EnableSprite : DisableSprite;
	}
	
	private void setButtonMessage(Transform button, bool enabled) {
		UIButtonMessage buttonMessage = button.GetComponent<UIButtonMessage>();
		
		if (enabled) {
			buttonMessage.target = gameObject;
			buttonMessage.functionName = ButtonCallbackFunctionName;
		}
		
		buttonMessage.enabled = enabled;
	}
	
	// When button will be create
	void onButtonDraw(Transform button) {	
		int levelIndex = Convert.ToInt32(button.name);
				
		UILabel uiLabel = button.Find("levelIndex").GetComponent<UILabel>();
		UILabel record  = button.Find("record").GetComponent<UILabel>();
		
		bool isEnabledLevel = SettingsContainer.GetLevelStars(levelIndex) > 0 || 
							  levelIndex == 1 || 
							  SettingsContainer.GetLevelMaxScore(levelIndex) > 0 ||
							  (levelIndex > 1 && SettingsContainer.GetLevelMaxScore(levelIndex - 1) > 0);
		
		setButtonEnable(button, isEnabledLevel);
		setButtonMessage(button, isEnabledLevel);	
			
		record.text = isEnabledLevel ? SettingsContainer.GetLevelMaxScore(levelIndex).ToString() : "";
		uiLabel.text = isEnabledLevel ? levelIndex.ToString() : "Locked";
	}
	
}

