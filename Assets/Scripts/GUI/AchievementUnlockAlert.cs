using UnityEngine;
using System.Collections;

public class AchievementUnlockAlert : MonoBehaviour {
	public UILabel achieveText;
	
	public void alert(AchievementManager.Achievement achieve) {
		achieveText.text = "[00BB00]Achievement unlocked:[-] " + achieve.name;
		animation.Play();
	}
}
