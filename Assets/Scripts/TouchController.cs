using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour
{
	public Core core;
	public CurrentFigure currentFigure;
	
	Vector2 StartPos;
	Vector2 MoveStartPos;
	int SwipeID = -1;
	int MoveID = -1;
	float minMovement = 20.0f;
	float step;
	float y1;
	float y2;
	float x1;
	float x2;
	
	// Use this for initialization
	void Start ()
	{
		step = Screen.width / 19;
		y1 = Screen.height / 8 * 7;
		y2 = Screen.height / 2;
		x1 = Screen.width / 3;
		x2 = Screen.width / 3 * 2;
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Touch T in Input.touches) {
			Vector2 P = T.position;
	       	
			if (T.phase == TouchPhase.Began && SwipeID == -1) {
				SwipeID = T.fingerId;
				StartPos = P;
			} else if (T.fingerId == SwipeID) {
				Vector2 delta = P - StartPos;
				if (T.phase == TouchPhase.Moved && delta.magnitude > minMovement) {
	          		SwipeID = -1;
	          		if (Mathf.Abs (delta.x) > Mathf.Abs (delta.y)) {
						if (delta.x > 0) {
							OnSwipe(P, "right");
						} else {
							OnSwipe(P, "left");
	              		}
	          		} else {
						if (delta.y > 0) {
							OnSwipe(P, "up");
						} else {
							OnSwipe(P, "down");
						}
					}
				} else if (T.phase == TouchPhase.Canceled || T.phase == TouchPhase.Ended) {
					SwipeID = -1;
					if (delta.magnitude < minMovement) {
						OnTouch(P);
					}
				}
				
			}
			
			if (T.phase == TouchPhase.Began && MoveID == -1) {
				MoveID = T.fingerId;
				MoveStartPos = P;
			} else if (T.fingerId == MoveID) {
				Vector2 delta = P - MoveStartPos;
				if (T.phase == TouchPhase.Moved && Mathf.Abs(delta.x) > step) {
	          		if (delta.x < 0) {
						OnStepLeft(P);
					} else {
						OnStepRight(P);
					}
					MoveStartPos.x = P.x;
				} else if (T.phase == TouchPhase.Moved && Mathf.Abs(delta.y) > step) {
					if (delta.y < 0) {
						OnStepDown(P);
					}
					MoveStartPos.y = P.y;
				} else if (T.phase == TouchPhase.Canceled || T.phase == TouchPhase.Ended) {
					MoveID = -1;
				}
			}
		}
	}
	
	void OnSwipe(Vector2 startPos, string direction)
	{
		
	}
	
	void OnTouch(Vector2 pos)
	{
		if (pos.y < y2 && pos.x < x1) {
			core.RotateCCW();
		} else if (pos.y < y2 && pos.x > x2) {
			core.RotateCW();
		} else if (pos.y > y1) {
			// menu
			core.Reinit();
		} else {
			currentFigure.RotateCW();
		}
	}
	
	void OnStepLeft(Vector2 pos)
	{
		currentFigure.MoveLeft();
	}
	
	void OnStepRight(Vector2 pos)
	{
		currentFigure.MoveRight();
	}
	
	void OnStepDown(Vector2 pos)
	{
		currentFigure.Tick();
	}
}