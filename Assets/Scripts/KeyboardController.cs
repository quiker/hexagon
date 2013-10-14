using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour {
	
	public Core core;
	public CurrentFigure currentFigure;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.J)) {
			core.RotateCCW();
		}
		if (Input.GetKeyDown(KeyCode.L)) {
			core.RotateCW();
		}
		if (Input.GetKeyDown(KeyCode.A)) {
			currentFigure.MoveLeft();
		}
		if (Input.GetKeyDown(KeyCode.D)) {
			currentFigure.MoveRight();
		}
		if (Input.GetKey(KeyCode.S)) {
			currentFigure.Tick();
		}
		if (Input.GetKeyDown(KeyCode.Q)) {
			currentFigure.RotateCCW();
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			currentFigure.RotateCW();
		}
	}
}
