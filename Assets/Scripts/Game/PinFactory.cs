using UnityEngine;
using System.Collections;
using System.Linq;

public class PinFactory : MonoBehaviour {
	
	public GameObject pinPrefab;
	
	private int[][] actions;
	public void SetActions(int[][] actions)
	{
		this.actions = actions;
	}
	
	public Pin[] GetPins(Figure figure, int[][] pins)
	{	 
		Pin[] pinsArr = new Pin[pins.Length];
		for (int i = 0; i < pins.Length; i++) {
			pinsArr[i] = GetPin(
				figure,
				pins[i][2],
				pins[i][3],
				new Vector2(
					pins[i][0],
					pins[i][1]
				),
				pins[i][4]
			);
		}
		
		return pinsArr;
	}
	
	public Pin GetPin(Figure figure, int color, int type, Vector2 position, int actionId = -1)
	{
		GameObject pinGO = Instantiate(pinPrefab) as GameObject;
		pinGO.transform.parent = figure.transform.FindChild("PinWrapper");
		Pin pin = pinGO.GetComponent("Pin") as Pin;
		
		pin.color = color;
		pin.type  = type;
		pin.position = position;
		if (actionId > -1) {
			pin.action = actions[actionId];
		} else {
			pin.action = new int[0];
		}
		
		return pin;
	}
}
