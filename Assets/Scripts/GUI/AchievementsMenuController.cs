using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementsMenuController : AbstractPanelMenu {
	public UIGrid grid;
	public Transform achievePrefub;
	private List<Transform> achieveList;
	
	void OnAchievElementDraw(Transform achievObject) {
		AchievementManager.Achievement achieve = AchievementManager.GetInstance().GetAchieveById(achievObject.name);
		
		UISlicedSprite slisedSprite = achievObject.Find("BG").GetComponent<UISlicedSprite>();
		UILabel title = achievObject.Find("Title").GetComponent<UILabel>();
		UILabel description = achievObject.Find("Description").GetComponent<UILabel>();
		UILabel progress = achievObject.Find("Progress").GetComponent<UILabel>();
		
		title.text = achieve.name;
		description.text = achieve.description;
		progress.text = achieve.GetProgressText();
		
		slisedSprite.spriteName = achieve.IsCompleted() ? "Glow" : "Dark";
		
	}
	
	void updateList() {
		foreach (Transform achieve in achieveList) {
			OnAchievElementDraw(achieve);
		}
	}
	
	void Start() {
		achieveList = new List<Transform>();
		
		foreach (AchievementManager.Achievement achieve in AchievementManager.GetInstance().achievements) {
			Transform achieveObject = Instantiate(achievePrefub, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
			achieveObject.parent = grid.transform;
			achieveObject.name = achieve.id;
			OnAchievElementDraw(achieveObject);
			achieveList.Add(achieveObject);
		}
		
		grid.repositionNow = true;
	}
	
	void OnEnable() {
		if (achieveList != null && achieveList.Count > 0) {
			updateList();
		}
	}
	
	public override MenuPanel getId() {
		return MenuPanel.Achievements;
	}
}
