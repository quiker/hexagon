using UnityEngine;
using System.Collections;
using System;

public class LevelMenuController : AbstractPanelMenu {
	public string EnableSprite = "Glow";
	public string DisableSprite = "Dark";
	public string ButtonCallbackFunctionName = "onLevelSelected";
	public GameObject buttonNext;
	public GameObject buttonBack;
	public int countLevels = 35;
	public Tile tile;
	
	// When level will be select
	void onLevelSelected(GameObject go) {
		int levelIndex = Convert.ToInt32(go.transform.name);
		Game.GetInstance().MenuStartLevel(levelIndex);
	}
	
	void OnButtonBackClick(GameObject go) {
		if (tile.GetCurrentGridIndex() > 0) {
			tile.BackGrid();
		}
	}
	
	void OnButtonNextClick(GameObject go) {
		if (tile.GetCurrentGridIndex() < tile.GetCountGrids() - 1) {
			tile.NextGrid();
		}
	}
	
	
	private void setButtonEnable(Transform button, bool enabled) {
		UIButtonColor buttonColor   = button.GetComponent<UIButtonColor>();
		UISlicedSprite slicedPrite  = button.transform.Find("SlicedSprite").GetComponent<UISlicedSprite>();
		UIButtonSound buttonSound   = button.GetComponent<UIButtonSound>();
		
		buttonColor.enabled  = enabled;
		buttonSound.enabled  = enabled;
		button.collider.enabled = enabled;
		slicedPrite.spriteName = enabled ? EnableSprite : DisableSprite;
	}
	
	private void setButtonMessage(Transform button, bool enabled) {
		UIButtonMessage buttonMessage = button.GetComponent<UIButtonMessage>();
		
		if (buttonMessage == null) {
			buttonMessage = button.gameObject.AddComponent<UIButtonMessage>();
		}
		
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
		Stars stars     = button.Find("PanelStars").GetComponent<Stars>();  
		
		bool isEnabledLevel = SettingsContainer.GetLevelStars(levelIndex) > 0 || 
							  levelIndex == 1 || 
							  SettingsContainer.GetLevelMaxScore(levelIndex) > 0 ||
							  (levelIndex > 1 && SettingsContainer.GetLevelStars(levelIndex - 1) > 0);
		
		setButtonEnable(button, isEnabledLevel);
		setButtonMessage(button, isEnabledLevel);	
		
		stars.updateStars(SettingsContainer.GetLevelStars(levelIndex));
		record.text = isEnabledLevel ? SettingsContainer.GetLevelMaxScore(levelIndex).ToString() : "";
		uiLabel.text = isEnabledLevel ? levelIndex.ToString() : "Locked";
	}
	
	public override MenuPanel getId() {
		return MenuPanel.Level;
	}	
	
	protected override void OnShow () {
		if (!tile.isGenerated()) {
			tile.Generate(countLevels);
			tile.ActivateGrid(0);
		}else{
			tile.UpdateLevels();
		}
	}
}

