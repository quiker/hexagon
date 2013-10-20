using UnityEngine;
using System.Collections;
using System.Linq;

public class LevelFactory : MonoBehaviour {
	
	public GameObject pinPrefab;
	
	private int[] level1 = {0,0,0};
	
	public Pin[] GetLevel(int level, Core core)
	{
		int[] template;
		template = level1; 
		 
		Pin[] pins = new Pin[template.Length / 3];
		
		for (int i = 0; i < template.Length / 3; i++) {
			GameObject pinGO = Instantiate(pinPrefab) as GameObject;
			pinGO.transform.parent = core.transform.FindChild("PinWrapper");
			Pin pin = pinGO.GetComponent("Pin") as Pin;
			
			pin.color = template[i*3+2];
			pin.position = new Vector2(
				template[i*3],
				template[i*3+1]
			);
			
		    pins[i] = pin;
		}
		
		return pins;
	}
	
	public Pin[] GetTestLevel(int num, Core core)
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
