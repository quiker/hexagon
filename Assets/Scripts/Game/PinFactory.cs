using UnityEngine;
using System.Collections;
using System.Linq;

public class PinFactory : MonoBehaviour {
	
	public GameObject pinPrefab;
	
	private Pin.MobAction[] actions;
	public void SetActions(float[][] actions)
	{
		this.actions = new Pin.MobAction[actions.Length];
		for (int i = 0; i < actions.Length; i++) {
			this.actions[i] = new Pin.MobAction();
			this.actions[i].inactiveInterval = actions[i][0];
			this.actions[i].activeInterval = actions[i][1];
			this.actions[i].chance = actions[i][2];
			this.actions[i].id = (int)actions[i][3];
			this.actions[i].parameters = ArrayUtils.SliceF(actions[i], 4);
		}
	}
	
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
			Pin.MobAction[] tmpPinActions = new Pin.MobAction[actionIds.Length];
			for (int i = 0; i < actionIds.Length; i++) {
				tmpPinActions[i] = actions[actionIds[i]];
			}
			pin.actions = tmpPinActions;
		} else {
			pin.actions = new Pin.MobAction[0];
		}
		
		return pin;
	}
}
