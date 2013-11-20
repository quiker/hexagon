using UnityEngine;
using System.Collections;

public class CurrentFigure : MonoBehaviour
{
	public FigureFactory figureFactory;
	public LevelController levelController = null;
	
	[HideInInspector]
	public Figure figure;
	public static int startY = 18;
	private bool horizontalMoveDown = true;
	
	// Use this for initialization
	void Start () {
		figure = GetComponent("Figure") as Figure;
	}
	
	public void NewFigure()
	{
		figure.Init(0, startY);
		figure.pins = figureFactory.GetFigure(transform.FindChild("PinWrapper"));
		figure.UpdatePosition();
	}
	
	public void Reinit()
	{
		figure.Reinit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public bool Tick()
	{
		if (figure.isCollisionLeftDownWall()) {
			return MoveRightDown(true);
		} else if (figure.isCollisionRightDownWall()) {
			return MoveLeftDown(true);
		} else {
			return MoveDown();
		}
	}
	
	public bool MoveDown()
	{
		if (figure.isCollisionDown()) {
			levelController.OnConnectStart();
			return false;
		} else {
			figure.MoveDown();
			return true;
		}
	}
	
	public void MoveLeft()
	{
		bool moved;
		if (horizontalMoveDown) {
			moved = MoveLeftDown(false);
		} else {
			moved = MoveLeftUp();
		}
		horizontalMoveDown = !(horizontalMoveDown && moved);
	}
	
	public void MoveRight()
	{
		bool moved;
		if (horizontalMoveDown) {
			moved = MoveRightDown(false);
		} else {
			moved = MoveRightUp();
		}
		horizontalMoveDown = !(horizontalMoveDown && moved);
	}
	
	public bool MoveRightDown(bool connect)
	{
		if (!figure.isCollisionRightWall() && !figure.isCollisionRightDownWall() && !figure.isCollisionRightDown()) {
			figure.MoveRightDown();
			return true;
		} else {
			if (connect) { 
				levelController.OnConnectStart();
			}
		}
		return false;
	}
	
	public bool MoveRightUp()
	{
		if (!figure.isCollisionRightWall() && !figure.isCollisionRightUp()) {
			figure.MoveRightUp();
			return true;
		}
		return false;
	}
	
	public bool MoveLeftDown(bool connect)
	{
		if (!figure.isCollisionLeftWall() && !figure.isCollisionLeftDownWall() && !figure.isCollisionLeftDown()) {
			figure.MoveLeftDown();
			return true;
		} else {
			if (connect) { 
				levelController.OnConnectStart();
			}
		}
		return false;
	}
	
	public bool MoveLeftUp()
	{
		if (!figure.isCollisionLeftWall() && !figure.isCollisionLeftUp()) {
			figure.MoveLeftUp();
			return true;
		}
		return false;
	}
	
	public bool RotateCW()
	{
		if (!figure.isCollisionRotateCW() && !figure.isCollisionWallRotateCW()) {
			figure.RotateCW();
			return true;
		}
		return false;
	}
	
	public bool RotateCCW()
	{
		if (!figure.isCollisionRotateCCW() && !figure.isCollisionWallRotateCCW()) {
			figure.RotateCCW();
			return true;
		}
		return false;
	}
}

