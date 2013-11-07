using UnityEngine;
using System.Collections;
using System.Linq;

public class LevelFactory : MonoBehaviour {
	
	public GameObject pinPrefab;
	
	public Pin[] GetLevel(Core core, int[][] pins)
	{	 
		Pin[] pinsArr = new Pin[pins.Length];
		
		for (int i = 0; i < pins.Length; i++) {
			GameObject pinGO = Instantiate(pinPrefab) as GameObject;
			pinGO.transform.parent = core.transform.FindChild("PinWrapper");
			Pin pin = pinGO.GetComponent("Pin") as Pin;
			
			pin.color = pins[i][2];
			pin.type  = pins[i][3];
			pin.position = new Vector2(
				pins[i][0],
				pins[i][1]
			);
			
		    pinsArr[i] = pin;
		}
		
		return pinsArr;
	}
	
	public Pin[] GetTestLevel(Core core)
	{
		Pin[] pins = new Pin[121];
		
		for (int i = 0; i < 121; i++) {
			GameObject pinGO = Instantiate(pinPrefab) as GameObject;
			pinGO.transform.parent = core.transform.FindChild("PinWrapper");
			Pin pin = pinGO.GetComponent("Pin") as Pin;
			
			pin.color = 0;
			pin.position = new Vector2(
				i/11 - 5,
				i%11 - 5
			);
			
		    pins[i] = pin;
		}
		
		return pins;
	}
}
