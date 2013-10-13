using UnityEngine;
using System.Collections;
using System.Linq;

public class FigureFactory : MonoBehaviour {
	
	public GameObject pinPrefab;
	private int[] templates = {
		
		/*
		 *        *
		 *       *
		 *      *
		 *     *
		 */
		-1, -1,    0,  0,    1,  1,    2,  2,
		-1,  0,    0,  0,    1,  0,    2,  0,
		 0, -2,    0, -1,    0,  0,    0,  1,
		-2, -2,   -1, -1,    0,  0,    1,  1,
		-2,  0,   -1,  0,    0,  0,    1,  0,
		 0, -1,    0,  0,    0,  1,    0,  2,
		
		/*
		 *        *
		 *       *
		 *    * *
		 */
		-1,  0,    0,  0,    1,  1,    2,  2,
		 0,  1,    0,  0,    1,  0,    2,  0,
		 0, -2,    0, -1,    0,  0,    1,  1,
		-2, -2,   -1, -1,    0,  0,    1,  0,
		-2,  0,   -1,  0,    0,  0,    0, -1,
		-1, -1,    0,  0,    0,  1,    0,  2,
		
		/*
		 *        *
		 *       *
		 *      *
		 *       *
		 */
		 0, -1,    0,  0,    1,  1,    2,  2,
		-1, -1,    0,  0,    1,  0,    2,  0,
		-1,  0,    0,  0,    0, -1,    0, -2,
		 0,  1,    0,  0,   -1, -1,   -2, -2,
		 1,  1,    0,  0,   -1,  0,   -2,  0,
		 1,  0,    0,  0,    0,  1,    0,  2,
		
		/*
		 *        *
		 *       *
		 *        *
		 *       *
		 */
		-1, -1,    0,  0,    0,  1,    1,  2,
		-1,  0,    0,  0,    1,  1,    2,  1,
		 0,  1,    0,  0,    1,  0,    1, -1,
		 1,  1,    0,  0,    0, -1,   -1, -2,
		 1,  0,    0,  0,   -1, -1,   -2, -1,
		 0, -1,    0,  0,   -1,  0,   -1,  1,
		
		/*
		 *      *
		 *       *
		 *      *
		 *       *
		 */
		-1, -1,    0,  0,    1,  0,    2,  1,
		-1,  0,    0,  0,    0, -1,    1, -1,
		 0,  1,    0,  0,   -1, -1,   -1, -2,
		 1,  1,    0,  0,   -1,  0,   -2, -1,
		 1,  0,    0,  0,    0,  1,   -1,  1,
		 0, -1,    0,  0,    1,  1,    1,  2,
		
		/*
		 *        *
		 *      * *
		 *      *
		 */
		 0,  0,   -1,  0,    0,  1,    1,  1,
		 0,  0,    0,  1,    1,  1,    1,  0,
		 0,  0,    1,  1,    1,  0,    0, -1,
		 0,  0,    1,  0,    0, -1,   -1, -1,
		 0,  0,    0, -1,   -1, -1,   -1,  0,
		 0,  0,   -1, -1,   -1,  0,    0,  1,
		
		/*
		 *        *
		 *       * *
		 *      *
		 */
		-1, -1,    0,  0,    1,  1,    1,  0,
		-1,  0,    0,  0,    1,  0,    0, -1,
		 0,  1,    0,  0,    0, -1,   -1, -1,
		-1, -1,    0,  0,    1,  1,   -1,  0,
		-1,  0,    0,  0,    1,  0,    0,  1,
		 0,  1,    0,  0,    0, -1,    1,  1,
		
		/*
		 *        *
		 *       *
		 *      * *
		 */
		
		-1, -1,    0,  0,    1,  1,    0, -1,
		-1,  0,    0,  0,    1,  0,   -1, -1,
		 0,  1,    0,  0,    0, -1,   -1,  0,
		-1, -1,    0,  0,    1,  1,    0,  1,
		-1,  0,    0,  0,    1,  0,    1,  1,
		 0,  1,    0,  0,    0, -1,    1,  0,
	};
	
	public Pin[] GetFigure(Vector2 position, Core core)
	{
		int templateNum = Random.Range(0,48);
		int color = Random.Range(0,5);
		Pin[] pins = new Pin[4];
		
//		for (int i = -3; i <= 3; i++) {
//			for (int j = -3; j <= 3; j++) {
//			
//				GameObject pinGO = Instantiate(pinPrefab) as GameObject;
//				Pin pin = pinGO.GetComponent("Pin") as Pin;
//				
//				color = Random.Range(0,5);
//				pin.color = color;
//				pin.position = new Pin.HexVector2(
//					i,
//					j
//				);
//				pin.core = core;
//				pin.figurePosition = position;
//				pin.UpdatePosition();
//			    pins[(i+3)*7+j+3] = pin;
//			}
//		}
		
		for (int i = 0; i < 4; i++) {
			GameObject pinGO = Instantiate(pinPrefab) as GameObject;
			Pin pin = pinGO.GetComponent("Pin") as Pin;
			
			pin.color = color;
			pin.position = new Vector2(
				templates[templateNum * 8 + i * 2 + 0],
				templates[templateNum * 8 + i * 2 + 1]
			);
			pin.core = core;
			pin.figurePosition = position;
			pin.UpdatePosition();
		    pins[i] = pin;
		}
		
		return pins;
	}
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
