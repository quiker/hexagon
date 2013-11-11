using UnityEngine;
using System.Collections;

public class Stars : MonoBehaviour {
	public UISprite star1;
	public UISprite star2;
	public UISprite star3;
	
	public void updateStars(int count) {		
		switch(count) {
			case 1:
				star1.gameObject.SetActive(true);
				star2.gameObject.SetActive(false);
				star3.gameObject.SetActive(false);
				break;
			case 2:
				star1.gameObject.SetActive(true);
				star2.gameObject.SetActive(true);
				star3.gameObject.SetActive(false);
				break;
			case 3:
				star1.gameObject.SetActive(true);
				star2.gameObject.SetActive(true);
				star3.gameObject.SetActive(true);
				break;
			default:
				star1.gameObject.SetActive(false);
				star2.gameObject.SetActive(false);
				star3.gameObject.SetActive(false);
				break;
		}
	}
}
