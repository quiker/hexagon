using UnityEngine;
using System.Collections;

public class CurrentFigure : MonoBehaviour, Ticker.TickListener {
	
	private Figure figure;
	private int startY = 20;
	private bool horizontalMoveDown = true;
	
	// Use this for initialization
	void Start () {
		figure = GetComponent("Figure") as Figure;
		figure.Init(0, startY);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void OnTick()
	{
		Tick();
	}
	
	public void Tick()
	{
		if (figure.isCollisionLeftDownWall()) {
			MoveRightDown(true);
		} else if (figure.isCollisionRightDownWall()) {
			MoveLeftDown(true);
		} else {
			MoveDown();
		}
	}
	
	public bool MoveDown()
	{
		if (figure.isCollisionDown()) {
			figure.Connect();
			figure.Init(0, startY);
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
				figure.Connect();
				figure.Init(0, startY);
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
				core.Connect();
				figure.Init(0, startY);
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

