using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementsMenuController : AbstractPanelMenu {
	public UIGrid grid;
	public Transform achievePrefub;
	private List<Transform> achieveList;
	
	void OnAchievElementDraw(Transform achievObject, AchievementManager.Achievement achieve) {
		//AchievementManager.Achievement achieve = AchievementManager.GetInstance().GetAchieveById(achievObject.name);
		
		UISlicedSprite slisedSprite = achievObject.Find("BG").GetComponent<UISlicedSprite>();
		UILabel title = achievObject.Find("Title").GetComponent<UILabel>();
		UILabel description = achievObject.Find("Description").GetComponent<UILabel>();
		UILabel progress = achievObject.Find("Progress").GetComponent<UILabel>();
		
		title.text = achieve.name;
		description.text = achieve.description;
		progress.text = achieve.IsCompleted() ? "" : achieve.GetProgressText();
		
		slisedSprite.spriteName = achieve.IsCompleted() ? "Glow" : "Dark";	
	}
	
	void Load() {
		foreach (AchievementManager.Achievement achieve in AchievementManager.GetInstance().achievements) {
			Transform achieveObject = Instantiate(achievePrefub, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
			achieveObject.parent = grid.transform;
			achieveObject.name = achieve.id;
			OnAchievElementDraw(achieveObject, achieve);
		}
		
		grid.repositionNow = true;
	}
	
	void DestroyChildrens() {
		foreach (Transform childTransform in grid.transform) {
		    Destroy(childTransform.gameObject);
		}
	}
	
	
	public override MenuPanel getId() {
		return MenuPanel.Achievements;
	}
	
	
	protected override void OnShow() {
		DestroyChildrens();
		Load();
	}
}
