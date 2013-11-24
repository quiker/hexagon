using UnityEngine;
using System.Collections;
using System;

public class Figure : MonoBehaviour
{
	[HideInInspector]
	public Pin[] pins;
	
	public Figure[] anotherFigures;
	public Vector2 position;
	public AudioClip rotateAudioClip = null;
	
	private PinWrapper pw;
	
	// Use this for initialization
	public void Init (int x, int y) {
		position = new Vector2(x, y);
		pw = transform.FindChild("PinWrapper").GetComponent("PinWrapper") as PinWrapper;
	}
	
	public void Reinit () {
		foreach (Pin pin in pins) {
			Destroy(pin.gameObject);
		}
		Init((int)position.x, (int)position.y);
	}
	
	public void UpdatePosition()
	{
		pw.StopAll();
		transform.localPosition = HexVector2.ConvertHexVector(position);
		foreach (Pin pin in pins) {
			pin.UpdatePosition();
		}
	}
	
	public void AddPin(Pin pin)
	{
		Array.Resize< Pin >(ref pins, pins.Length + 1);
		pins[pins.Length-1] = pin;
	}
	
	public void RemovePin(Pin pin)
	{
		int n = 0;
		foreach (Pin p in pins) {
			if (pin == p) {
				break;
			}
			n++;
		}
		for (int i = n; i < pins.Length - 1; i++) {
			pins[i] = pins[i+1];
		}
		Array.Resize< Pin >(ref pins, pins.Length - 1);
	}
	
	public int[,] GetPinsMap(bool pillsOnly = false)
	{
		int[,] pinsMap = new int[41,41];
		
		// reset pinMap
		for (int i = 0; i < 41; i++) {
			for (int j = 0; j < 41; j++) {
				pinsMap[i,j] = 0;
			}
		}
		// update pinMap
		foreach (Pin pin in pins) {
			if (!pillsOnly || (pillsOnly && pin.type == Pin.PIN_TYPE_PILL)) {
				pinsMap[(int)pin.position.x+20, (int)pin.position.y+20] = pin.color;
			}
		}
		
		return pinsMap;
	}
	
	public void MoveUp()
	{
		position.y ++;
		UpdatePosition();
	}
	
	public void MoveDown()
	{
		UpdatePosition();
		position.y --;
		pw.Play("TickSlideDown");
	}
	
	public void MoveRightUp()
	{
		UpdatePosition();
		position.x ++;
		position.y ++;
		pw.Play("TickSlideRightUp");
	}
	
	public void MoveRightDown()
	{
		UpdatePosition();
		position.x ++;
		pw.Play("TickSlideRightDown");
	}
	
	public void MoveLeftUp()
	{
		UpdatePosition();
		position.x --;
		pw.Play("TickSlideLeftUp");
	}
	
	public void MoveLeftDown()
	{
		UpdatePosition();
		position.y --;
		position.x --;
		pw.Play("TickSlideLeftDown");
	}
	
	public void RotateCCW()
	{
		NGUITools.PlaySound(rotateAudioClip);
		UpdatePosition();
		foreach (Pin pin in pins) {
			pin.position = HexVector2.RotateCCW(pin.position);
		}
		pw.Play("RotateCCW");
	}
	
	public void RotateCW()
	{
		NGUITools.PlaySound(rotateAudioClip);
		UpdatePosition();
		foreach (Pin pin in pins) {
			pin.position = HexVector2.RotateCW(pin.position);
		}
		pw.Play("RotateCW");
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
	
	public Pin GetPin(Vector2 pos)
	{
		foreach (Pin pin in pins) {
			if (position + pin.position == pos) {
				return pin;
			}
		}
		return null;
	}
	
	public bool isCollisionUp()
	{
		foreach (Figure figure in anotherFigures) {
			foreach (Pin pin in pins) {
				if (figure.IsFilled(position + pin.position + new Vector2(0, 1))) {
					return true;
				}
			}
		}
		return false;
	}
	
	public bool isCollisionDown()
	{
		foreach (Figure figure in anotherFigures) {
			foreach (Pin pin in pins) {
				if (figure.IsFilled(position + pin.position + new Vector2(0, -1))) {
					return true;
				}
			}
		}
		return false;
	}
	
	public bool isCollisionRightDown()
	{
		foreach (Figure figure in anotherFigures) {
			foreach (Pin pin in pins) {
				if (figure.IsFilled(position + pin.position + new Vector2(1, 0))) {
					return true;
				}
			}
		}
		return false;
	}
	
	public bool isCollisionRightUp()
	{
		foreach (Figure figure in anotherFigures) {
			foreach (Pin pin in pins) {
				if (figure.IsFilled(position + pin.position + new Vector2(1, 1))) {
					return true;
				}
			}
		}
		return false;
	}
	
	public bool isCollisionLeftDown()
	{
		foreach (Figure figure in anotherFigures) {
			foreach (Pin pin in pins) {
				if (figure.IsFilled(position + pin.position + new Vector2(-1, -1))) {
					return true;
				}
			}
		}
		return false;
	}
	
	public bool isCollisionLeftUp()
	{
		foreach (Figure figure in anotherFigures) {
			foreach (Pin pin in pins) {
				if (figure.IsFilled(position + pin.position + new Vector2(-1, 0))) {
					return true;
				}
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
	
	public bool isCollisionRotateCW()
	{
		foreach (Figure figure in anotherFigures) {
			foreach (Pin pin in pins) {
				if (figure.IsFilled(position + HexVector2.RotateCW(pin.position))) {
					return true;
				}
			}
		}
		return false;
	}
	
	public bool isCollisionRotateCCW()
	{
		foreach (Figure figure in anotherFigures) {
			foreach (Pin pin in pins) {
				if (figure.IsFilled(position + HexVector2.RotateCCW(pin.position))) {
					return true;
				}
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
			pin.transform.parent = figure.transform.FindChild("PinWrapper");
		}
		
		int arrayOriginalSize = figure.pins.Length;
		Array.Resize< Pin >(ref figure.pins, figure.pins.Length + pins.Length);
		Array.Copy(pins, 0, figure.pins, arrayOriginalSize, pins.Length);
		pins = new Pin[0];
		Reinit();
	}
}
