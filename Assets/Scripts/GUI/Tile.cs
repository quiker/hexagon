using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
	public int RowCount = 2;
	public int ColCount = 5;
	public int MarginTop = 0;
	public int MarginLeft = 0;
	public Transform ButtonPrefub = null;
	public GameObject Controller;
	public string FunctionName = ""; 
	private List<Transform> tileList;
	
	
	public void updateTile() {
		for (int i = 0; i < tileList.Count; i++) {
			Controller.SendMessage(FunctionName, tileList[i], SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void Start() {
		tileList = new List<Transform>();
		
		for (int i = 0; i < RowCount; i++) {
			for (int j = 0; j < ColCount; j++) {
				Transform button = Instantiate(ButtonPrefub, new Vector3(-220 + (100 + MarginLeft) * j, 330 - (100 + MarginTop) * i, -40), Quaternion.identity) as Transform;
				button.parent = gameObject.transform;
				
				button.gameObject.AddComponent<UIButtonMessage>();
				
				UIButtonMessage bm = button.GetComponent<UIButtonMessage>();
				bm.target = Controller;
				bm.functionName = "onLevelSelected";
				
				int levelIndex = ColCount * i + j + 1;
				button.name  = levelIndex.ToString();
				
				tileList.Add(button);
				
				if (Controller != null) {
					Controller.SendMessage(FunctionName, button, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
	
	void OnEnable() {
		if (tileList != null && tileList.Count > 0) {
        	updateTile();
		}
    }
}