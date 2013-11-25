using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class FigureFactory : MonoBehaviour {
	
	public GameObject pinPrefab;
	private int[] templates = {
		
		/*
		 *        *
		 *      * *
		 *
		 */
		 0,  0,    0,  1,    1,  1,
		 0,  0,    1,  1,    1,  0,
		 0,  0,    1,  0,    0, -1,
		 0,  0,    0, -1,   -1, -1,
		 0,  0,   -1, -1,   -1,  0,
		 0,  0,   -1,  0,    0,  1,
		 
		
		/*
		 *        *
		 *       *
		 *      *
		 */
		 0,  0,    0,  1,    0, -1,
		 0,  0,    1,  1,   -1, -1,
		 0,  0,    1,  0,   -1,  0,
		 0,  0,    0,  1,    0, -1,
		 0,  0,    1,  1,   -1, -1,
		 0,  0,    1,  0,   -1,  0,
		
		/*
		 *        *
		 *       *
		 *        *
		 */
		 0,  0,    0,  1,    1,  0,
		 0,  0,    1,  1,    0, -1,
		 0,  0,    1,  0,   -1, -1,
		 0,  0,    0, -1,   -1,  0,
		 0,  0,   -1, -1,    0,  1,
		 0,  0,   -1,  0,    1,  1,
	};
	
	int[] colors = {};
	
	public void SetColors(int[] colors)
	{
		this.colors = colors;
	}
	
	public Pin[] GetFigure(Transform parent)
	{
		if (colors.Length == 0) throw new Exception("Need to set colors to FigureFactory");
		
		int templateNum = UnityEngine.Random.Range(0,18);
		
		Pin[] pins = new Pin[3];
		
		for (int i = 0; i < 3; i++) {
			GameObject pinGO = Instantiate(pinPrefab) as GameObject;
			pinGO.transform.parent = parent;
			Pin pin = pinGO.GetComponent("Pin") as Pin;
			
			pin.color = colors[UnityEngine.Random.Range(0,colors.Length)];
			pin.type = Pin.PIN_TYPE_PILL;
			pin.position = new Vector2(
				templates[templateNum * 6 + i * 2 + 0],
				templates[templateNum * 6 + i * 2 + 1]
			);
		    pins[i] = pin;
		}
		
		return pins;
	}
}
