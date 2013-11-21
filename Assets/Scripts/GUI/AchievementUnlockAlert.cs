using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementUnlockAlert : MonoBehaviour {
	public UILabel achieveText;
	public Animation alertAnimation;
	private List<AchievementManager.Achievement> achieveList;
	
	
	void Start() {
		achieveList = new List<AchievementManager.Achievement>();
	}
	
	public void alert(AchievementManager.Achievement achieve) {
		achieveList.Add(achieve);
	}
	
	
	private void playAnimation(AchievementManager.Achievement achieve) {
		achieveText.text = "[00BB00]Achievement unlocked:[-] " + achieve.name;
		//animation.Play();
		ActiveAnimation.Play(alertAnimation, AnimationOrTween.Direction.Forward);
	}
	
	void Update() {
		if (!animation.isPlaying && achieveList.Count > 0) {
			if (achieveList.Count > 0) {
				playAnimation(achieveList[0]);
			}
			
			achieveList.RemoveAt(0);
		}
	}
}
