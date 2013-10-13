using UnityEngine;
using System.Collections;
using System;

public class Figure : MonoBehaviour {

	public Pin[] pins;
	
	public Core core;
	public FigureFactory figureFactory;	
	public Vector2 position;
	
	// Use this for initialization
	public void Init (int x, int y) {
		position = new Vector2(x, y);
		pins = figureFactory.GetFigure(position, core);
	}
	
	public void Reinit () {
		foreach (Pin pin in pins) {
			Destroy(pin.gameObject);
		}
		Init((int)position.x, (int)position.y);
	}
	
	public void UpdatePosition()
	{
		foreach (Pin pin in pins) {
			pin.figurePosition = position;
			pin.UpdatePosition();
		}
	}
	
	public void MoveUp()
	{
		position.y ++;
		UpdatePosition();
	}
	
	public void MoveDown()
	{
		position.y --;
		UpdatePosition();
	}
	
	public void MoveRightUp()
	{
		position.x ++;
		position.y ++;
		UpdatePosition();
	}
	
	public void MoveRightDown()
	{
		position.x ++;
		UpdatePosition();
	}
	
	public void MoveLeftUp()
	{
		position.x --;
		UpdatePosition();
	}
	
	public void MoveLeftDown()
	{
		position.y --;
		position.x --;
		UpdatePosition();
	}
	
	public void RotateCCW()
	{
		foreach (Pin pin in pins) {
			pin.position = HexVector2.RotateCCW(pin.position);
			pin.UpdatePosition();
		}
	}
	
	public void RotateCW()
	{
		foreach (Pin pin in pins) {
			pin.position = HexVector2.RotateCW(pin.position);
			pin.UpdatePosition();
		}
	}
	
	public bool IsFilled(Vector2 pos)
	{
		foreach (Pin pin in pins) {
			if (position + pin.position == pos) {
				return true;	
			}
		}
		return false;
	}
	
	public bool isCollisionUp()
	{
		Figure coreFigure = core.GetComponent("Figure") as Figure;
		
		foreach (Pin pin in pins) {
			if (coreFigure.IsFilled(position + pin.position + new Vector2(0, 1))) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionDown()
	{
		Figure coreFigure = core.GetComponent("Figure") as Figure;
		
		foreach (Pin pin in pins) {
			if (coreFigure.IsFilled(position + pin.position + new Vector2(0, -1))) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionRightDown()
	{
		Figure coreFigure = core.GetComponent("Figure") as Figure;
		
		foreach (Pin pin in pins) {
			if (coreFigure.IsFilled(position + pin.position + new Vector2(1, 0))) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionRightUp()
	{
		Figure coreFigure = core.GetComponent("Figure") as Figure;
		
		foreach (Pin pin in pins) {
			if (coreFigure.IsFilled(position + pin.position + new Vector2(1, 1))) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionLeftDown()
	{
		Figure coreFigure = core.GetComponent("Figure") as Figure;
		
		foreach (Pin pin in pins) {
			if (coreFigure.IsFilled(position + pin.position + new Vector2(-1, -1))) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionLeftUp()
	{
		Figure coreFigure = core.GetComponent("Figure") as Figure;
		
		foreach (Pin pin in pins) {
			if (coreFigure.IsFilled(position + pin.position + new Vector2(-1, 0))) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionRightWall()
	{
		foreach (Pin pin in pins) {
			if (position.x+pin.position.x >= 9) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionLeftWall()
	{
		foreach (Pin pin in pins) {
			if (position.x+pin.position.x <= -9) {
				return true;
			}
		}
		return false;
	}
	public bool isCollisionRightDownWall()
	{
		foreach (Pin pin in pins) {
			if (position.y+pin.position.y <= position.x+pin.position.x-1) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionLeftDownWall()
	{
		foreach (Pin pin in pins) {
			if (position.y+pin.position.y <= -1) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionRotateCW(Figure figure = null)
	{
		if (!figure) {
			figure = core.GetComponent("Figure") as Figure;
		}
		foreach (Pin pin in pins) {
			if (figure.IsFilled(position + HexVector2.RotateCW(pin.position))) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionRotateCCW(Figure figure = null)
	{
		if (!figure) {
			figure = core.GetComponent("Figure") as Figure;
		}
		foreach (Pin pin in pins) {
			if (figure.IsFilled(position + HexVector2.RotateCCW(pin.position))) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionWallRotateCW()
	{
		Vector2 newPos;
		foreach (Pin pin in pins) {
			newPos = position + HexVector2.RotateCW(pin.position);
			if (
				newPos.x < -9 ||
				newPos.x > 9 ||
				newPos.y < newPos.x-1 ||
				newPos.y < -1
			) {
				return true;
			}
		}
		return false;
	}
	
	public bool isCollisionWallRotateCCW()
	{
		Vector2 newPos;
		foreach (Pin pin in pins) {
			newPos = position + HexVector2.RotateCCW(pin.position);
			if (
				newPos.x < -9 ||
				newPos.x > 9 ||
				newPos.y < newPos.x-1 ||
				newPos.y < -1
			) {
				return true;
			}
		}
		return false;
	}
	
	public void ConnectTo(Figure figure)
	{
		foreach (Pin pin in pins) {
			pin.position = pin.position + position - figure.position;
			pin.figurePosition = figure.position;
		}
		
		int arrayOriginalSize = figure.pins.Length;
		Array.Resize< Pin >(ref figure.pins, figure.pins.Length + pins.Length);
		Array.Copy(pins, 0, figure.pins, arrayOriginalSize, pins.Length);
	}
}
