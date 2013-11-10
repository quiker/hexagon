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
	
	// Use this for initialization
	public void Init (int x, int y) {
		position = new Vector2(x, y);
	}
	
	public void Reinit () {
		foreach (Pin pin in pins) {
			Destroy(pin.gameObject);
		}
		Init((int)position.x, (int)position.y);
	}
	
	public void UpdatePosition()
	{
		transform.localPosition = HexVector2.ConvertHexVector(position);
		foreach (Pin pin in pins) {
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
		PinWrapper pw = transform.FindChild("PinWrapper").GetComponent("PinWrapper") as PinWrapper;
		pw.StopAll();
		position.y --;
		pw.Play("TickSlideDown");
	}
	
	public void MoveRightUp()
	{
		PinWrapper pw = transform.FindChild("PinWrapper").GetComponent("PinWrapper") as PinWrapper;
		pw.StopAll();
		position.x ++;
		position.y ++;
		pw.Play("TickSlideRightUp");
	}
	
	public void MoveRightDown()
	{
		PinWrapper pw = transform.FindChild("PinWrapper").GetComponent("PinWrapper") as PinWrapper;
		pw.StopAll();
		position.x ++;
		pw.Play("TickSlideRightDown");
	}
	
	public void MoveLeftUp()
	{
		PinWrapper pw = transform.FindChild("PinWrapper").GetComponent("PinWrapper") as PinWrapper;
		pw.StopAll();
		position.x --;
		pw.Play("TickSlideLeftUp");
	}
	
	public void MoveLeftDown()
	{
		PinWrapper pw = transform.FindChild("PinWrapper").GetComponent("PinWrapper") as PinWrapper;
		pw.StopAll();
		position.y --;
		position.x --;
		pw.Play("TickSlideLeftDown");
	}
	
	public void RotateCCW()
	{
		audio.PlayOneShot(rotateAudioClip);
		PinWrapper pw = transform.FindChild("PinWrapper").GetComponent("PinWrapper") as PinWrapper;
		pw.StopAll();
		foreach (Pin pin in pins) {
			pin.position = HexVector2.RotateCCW(pin.position);
		}
		pw.Play("RotateCCW");
	}
	
	public void RotateCW()
	{
		audio.PlayOneShot(rotateAudioClip);
		PinWrapper pw = transform.FindChild("PinWrapper").GetComponent("PinWrapper") as PinWrapper;
		pw.StopAll();
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
