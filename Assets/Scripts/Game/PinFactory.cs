using UnityEngine;
using System.Collections;
using System.Linq;

public class PinFactory : MonoBehaviour {
	
	public GameObject pinPrefab;
	
	public Pin[] GetPins(Figure figure, int[][] pins)
	{
		int[] actionIds;
		Pin[] pinsArr = new Pin[pins.Length];
		for (int i = 0; i < pins.Length; i++) {
			actionIds = ArrayUtils.Slice(pins[i], 4);
			
			pinsArr[i] = GetPin(
				figure,
				pins[i][2],
				pins[i][3],
				new Vector2(
					pins[i][0],
					pins[i][1]
				),
				actionIds
			);
		}
		
		return pinsArr;
	}
	
	public Pin GetPin(Figure figure, int color, int type, Vector2 position, int[] actionIds = null)
	{
		GameObject pinGO = Instantiate(pinPrefab) as GameObject;
		pinGO.transform.parent = figure.transform.FindChild("PinWrapper");
		Pin pin = pinGO.GetComponent("Pin") as Pin;
		
		pin.color = color;
		pin.type  = type;
		pin.position = position;
		if (actionIds != null) {
			pin.actions = actionIds;
		} else {
			pin.actions = new int[0];
		}
		
		return pin;
	}
}
