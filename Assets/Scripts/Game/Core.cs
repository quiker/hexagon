using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Core : MonoBehaviour
{
	[HideInInspector]
	public Figure figure;
	private Vector2[][] rings;
	private int[,] pinsMap = new int[41,41];
	private int[,] actionsMap = new int[41,41];
	private float delayEndTime = 0;
	private bool isPinsRemoved = false;
	
	public int enableRings = 0;
	public int enableLines = 0;
	public int enableGroups = 0;
	public int enableHexagons = 0;
	public int lineLength = 0;
	public int groupSize = 0;
	
	public LevelController levelController = null;
	public AudioClip rotateAudioClip = null;
	public AudioClip breakAudioClip = null;
	public AudioClip connectAudioClip = null;
	
	
	// data for checking
	private Pin[] checkPins;
		
	// Use this for initialization
	void Start ()
	{
		// init ring coordinates
		rings = new Vector2[15][]; 
		
		for (int i = 0; i < 15; i++) {
			Vector2[] ring = new Vector2[(i+1)*6];
			
			for (int j = 0; j <= i; j++) {
				ring[j] = new Vector2(j, i+1);
			}
			
			for (int k = 1; k <= 5; k++) {
				for (int j = 0; j <= i; j++) {
					ring[k*(i+1) + j] = HexVector2.RotateCW(ring[(k-1)*(i+1) + j]);
				}
			}
			
			rings[i] = ring;
		}
	}
	
	public void SetPins(Pin[] pins)
	{
		figure = GetComponent("Figure") as Figure;
		figure.Init(0, 0);
		figure.Reinit();
		figure.pins = pins;
		figure.UpdatePosition();
	}

	public void RotateCW()
	{
		if (!figure.isCollisionRotateCW()) {
			figure.RotateCW();
			
			if (rotateAudioClip != null) {
				audio.PlayOneShot(rotateAudioClip);
			}
		}
	}
	
	public void RotateCCW()
	{
		if (!figure.isCollisionRotateCCW()) {
			figure.RotateCCW();
			
			if (rotateAudioClip != null) {
				audio.PlayOneShot(rotateAudioClip);
			}
		}
	}
	
	public void Connect(Figure _figure)
	{
		checkPins = _figure.pins;
		_figure.ConnectTo(figure);
		
		figure.UpdatePosition();
		
		if (CheckFail()) {
			Game.GetInstance().FailScreen();
			levelController.OnConnectFinish();
		} else {
			CheckAll();
			if (!isPinsRemoved) {
				audio.PlayOneShot(connectAudioClip);
			}
		}
	}
	
	private bool CheckFail()
	{
		foreach (Pin pin in figure.pins) {
			if (pin.position.x == 0 && pin.position.y == CurrentFigure.startY) {
				return true;
			}
		}
		return false;
	}
	
	private void CheckAll()
	{
		isPinsRemoved = false;
		// update pinsMap
		pinsMap = figure.GetPinsMap();
		
		bool needDalay = false;
		if (!needDalay && enableRings != 0) {
			needDalay = CheckRings();
		}
		if (!needDalay && enableLines != 0) {
			needDalay = CheckLines();
		}
		if (!needDalay && enableGroups != 0) {
			needDalay = CheckGroups();
		}
		if (!needDalay && enableHexagons != 0) {
			needDalay = CheckHexagons();
		}
		
		if (isPinsRemoved) {
			audio.PlayOneShot(breakAudioClip);
		}
		
		if (needDalay) {
			StartDelay();
		} else {
			levelController.OnConnectFinish();
		}
	}
	
	private void StartDelay()
	{
		delayEndTime = Time.timeSinceLevelLoad + 0.3f;
	}
	
	public void Update()
	{
		if (delayEndTime != 0 && Time.timeSinceLevelLoad >= delayEndTime) {
			delayEndTime = 0;
			CheckAll();
		}
	}
	
	public bool CheckRings()
	{
		// figure pins rings
		HashSet<int> ringNumsSet = new HashSet<int>();
		foreach (Pin pin in checkPins) {
			ringNumsSet.Add(RingNum(pin.position));
		}
		int [] ringNums = new int[ringNumsSet.Count];
		ringNumsSet.CopyTo(ringNums);
		Array.Sort(ringNums);
		Array.Reverse(ringNums);
		
		// search rings
		HashSet<int> foundRingNumsSet = new HashSet<int>();
		foreach (int ringNum in ringNums) {
			bool found = true;
			if (ringNum >= rings.Length || ringNum == 0) continue;
			foreach (Vector2 pos in rings[ringNum-1]) {
				if (pinsMap[(int)pos.x+20, (int)pos.y+20] == 0) {
					found = false;
					break;
				}
			}
			if (found) {
				foundRingNumsSet.Add(ringNum);
			}
		}
		
		// remove pins
		LinkedList<Pin> newPins = new LinkedList<Pin>();
		LinkedList<Pin> removePins = new LinkedList<Pin>();
		foreach(Pin pin in figure.pins) {
			foreach(int ring in foundRingNumsSet) {
				foreach(Vector2 pos in rings[ring-1]) {
					if (pos == pin.position) {
						removePins.AddLast(pin);
					}
				}
			}
		}
		
		foreach (Pin pin in figure.pins) {
			newPins.AddLast(pin);
		}
		foreach (Pin pin in removePins) {
			newPins.Remove(pin);
		}
		
		figure.pins = new Pin[newPins.Count];
		newPins.CopyTo(figure.pins, 0);
		
		foreach (Pin pin in removePins) {
			pin.DestroyPin();
		}
		
		// move up rings
		LinkedList<Pin> newCheckPins = new LinkedList<Pin>();
		foreach(int ring in foundRingNumsSet) {
			for (int pn = 0; pn < figure.pins.Length; pn++) {
				Pin pin = figure.pins[pn];
				// up
				if (pin.position.x >= -ring &&
				    pin.position.y > pin.position.x + ring) {
					figure.pins[pn].MoveOn(new Vector2(1, 0));
					newCheckPins.AddLast(pin);
				}
				// right up
				else if (pin.position.y <= pin.position.x + ring &&
				    pin.position.y > ring) {
					figure.pins[pn].MoveOn(new Vector2(0, -1));
					newCheckPins.AddLast(pin);
				}
				// right down
				else if (pin.position.x > ring &&
				    pin.position.y <= ring) {
					figure.pins[pn].MoveOn(new Vector2(-1, -1));
					newCheckPins.AddLast(pin);
				}
				// down
				else if (pin.position.x <= ring &&
				    pin.position.y < pin.position.x - ring) {
					figure.pins[pn].MoveOn(new Vector2(-1, 0));
					newCheckPins.AddLast(pin);
				}
				// left down
				else if (pin.position.y < -ring &&
				    pin.position.y >= pin.position.x - ring) {
					figure.pins[pn].MoveOn(new Vector2(0, 1));
					newCheckPins.AddLast(pin);
				}
				// left up
				else if (pin.position.y >= -ring &&
				    pin.position.x < -ring) {
					figure.pins[pn].MoveOn(new Vector2(1, 1));
					newCheckPins.AddLast(pin);
				}
			}
		}
		
		if (removePins.Count > 0) {
			isPinsRemoved = true;
		}
		
		if (newCheckPins.Count == 0) {
			return false;
		}
		
		checkPins = new Pin[newCheckPins.Count];
		newCheckPins.CopyTo(checkPins, 0);
		
		return checkPins.Length > 0;
	}
	
	public bool CheckLines()
	{
		// init actionsMap
		InitActionsMap();
		
		Vector2[] directions = {new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)};
		Vector2 v;
		int offset;
		
		foreach (Pin pin in checkPins) {
			// each pin check on 3 directions
			foreach (Vector2 direction in directions) {
				int color = pin.color;
				offset = 0;
				do {
					offset++;
					v = direction * offset + pin.position;
				} while (pinsMap[20+(int)v.x, 20+(int)v.y] == color);
				int positive_offset = offset - 1;
				offset = 0;
				do {
					offset++;
					v = direction * -offset + pin.position;
				} while (pinsMap[20+(int)v.x, 20+(int)v.y] == color);
				int negative_offset = offset - 1;
				
				if (positive_offset + negative_offset + 1 >= lineLength) {
					// line found
					for (int i = -negative_offset; i <= positive_offset; i++) {
						v = direction * i + pin.position;
						if (actionsMap[20+(int)v.x, 20+(int)v.y] != -1) {
							actionsMap[20+(int)v.x, 20+(int)v.y] = -1;
							FallPins(actionsMap, v);
						}
					}
				}
			}
		}
		
		// remove pins
		LinkedList<Pin> newPins = new LinkedList<Pin>();
		LinkedList<Pin> newCheckPins = new LinkedList<Pin>();
		foreach (Pin pin in figure.pins) {
			newPins.AddLast(pin);
		}
		foreach (Pin pin in figure.pins) {
			if (actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] == -1) {
				pin.DestroyPin();
				newPins.Remove(pin);
				isPinsRemoved = true;
			} else if (actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] > 0) {
				int direction = actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] % 6;
				offset = Mathf.FloorToInt(actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] / 6) + 1;
				v = HexVector2.GetBaseVector(direction) * offset;
				pin.MoveOn(v);
				newCheckPins.AddLast(pin);
			}
		}
		figure.pins = new Pin[newPins.Count];
		newPins.CopyTo(figure.pins, 0);

		if (newCheckPins.Count == 0) {
			return false;
		}
		checkPins = new Pin[newCheckPins.Count];
		newCheckPins.CopyTo(checkPins, 0);
		
		return checkPins.Length > 0;
	}
	
	public bool CheckGroups()
	{	
		// init actionsMap
		InitActionsMap();
		pinsMap = figure.GetPinsMap(true);
		
		Vector2 direction;
		Vector2 v;
		
		foreach (Pin pin in figure.pins) {
			if (pin.type != Pin.PIN_TYPE_PILL) {
				// each pin check on 6 directions
				int pillCount = 0;
				for (int i = 0; i < 6; i++) {
					direction = HexVector2.baseVectors[i];
					v = direction + pin.position;
					if (pinsMap[20+(int)v.x, 20+(int)v.y] == pin.color) {
						pillCount++;
					}
				}
				if (pillCount >= groupSize) {
					for (int i = 0; i < 6; i++) {
						direction = HexVector2.baseVectors[i];
						v = direction + pin.position;
						if (pinsMap[20+(int)v.x, 20+(int)v.y] == pin.color && actionsMap[20+(int)v.x, 20+(int)v.y] != -1) {
							actionsMap[20+(int)v.x, 20+(int)v.y] = -1;
							FallPins(actionsMap, v);
						}
					}
					// center pin
					v = pin.position;
					if (actionsMap[20+(int)v.x, 20+(int)v.y] != -1) {
						actionsMap[20+(int)v.x, 20+(int)v.y] = -1;
						FallPins(actionsMap, v);
					}
				}
				
			}
		}
		
		// remove pins
		LinkedList<Pin> newPins = new LinkedList<Pin>();
		LinkedList<Pin> newCheckPins = new LinkedList<Pin>();
		foreach (Pin pin in figure.pins) {
			newPins.AddLast(pin);
		}
		foreach (Pin pin in figure.pins) {
			if (actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] == -1) {
				pin.DestroyPin();
				newPins.Remove(pin);
				isPinsRemoved = true;
			} else if (actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] > 0) {
				int direction2 = actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] % 6;
				int offset = Mathf.FloorToInt(actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] / 6) + 1;
				v = HexVector2.GetBaseVector(direction2) * offset;
				pin.MoveOn(v);
				newCheckPins.AddLast(pin);
			}
		}
		figure.pins = new Pin[newPins.Count];
		newPins.CopyTo(figure.pins, 0);

		if (newCheckPins.Count == 0) {
			return false;
		}
		checkPins = new Pin[newCheckPins.Count];
		newCheckPins.CopyTo(checkPins, 0);
		
		return checkPins.Length > 0;
	}
	
	public bool CheckHexagons()
	{
		// init actionsMap
		InitActionsMap();
		//pinsMap = figure.GetPinsMap(true);
		
		Vector2 direction;
		Vector2 v;
		
		foreach (Pin pin in figure.pins) {
			// each pin check on 6 directions
			int pinCount = 0;
			for (int i = 0; i < 6; i++) {
				direction = HexVector2.baseVectors[i];
				v = direction + pin.position;
				if (pinsMap[20+(int)v.x, 20+(int)v.y] == pin.color) {
					pinCount++;
				}
			}
			if (pinCount == 6) {
				for (int i = 0; i < 6; i++) {
					direction = HexVector2.baseVectors[i];
					v = direction + pin.position;
					if (actionsMap[20+(int)v.x, 20+(int)v.y] != -1) {
						actionsMap[20+(int)v.x, 20+(int)v.y] = -1;
						FallPins(actionsMap, v);
					}
				}
				// center pin
				v = pin.position;
				if (actionsMap[20+(int)v.x, 20+(int)v.y] != -1) {
					actionsMap[20+(int)v.x, 20+(int)v.y] = -1;
					FallPins(actionsMap, v);
				}
			}
		}
		
		// remove pins
		LinkedList<Pin> newPins = new LinkedList<Pin>();
		LinkedList<Pin> newCheckPins = new LinkedList<Pin>();
		foreach (Pin pin in figure.pins) {
			newPins.AddLast(pin);
		}
		foreach (Pin pin in figure.pins) {
			if (actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] == -1) {
				pin.DestroyPin();
				newPins.Remove(pin);
				isPinsRemoved = true;
			} else if (actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] > 0) {
				int direction2 = actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] % 6;
				int offset = Mathf.FloorToInt(actionsMap[20+(int)pin.position.x, 20+(int)pin.position.y] / 6) + 1;
				v = HexVector2.GetBaseVector(direction2) * offset;
				pin.MoveOn(v);
				newCheckPins.AddLast(pin);
			}
		}
		figure.pins = new Pin[newPins.Count];
		newPins.CopyTo(figure.pins, 0);

		if (newCheckPins.Count == 0) {
			return false;
		}
		checkPins = new Pin[newCheckPins.Count];
		newCheckPins.CopyTo(checkPins, 0);
		
		return checkPins.Length > 0;	
	}
	
	private void InitActionsMap()
	{
		for (int i = 0; i < 41; i++) {
			for (int j = 0; j < 41; j++) {
				actionsMap[i,j] = 0;
			}
		}
	}
	
	private void FallPins(int[,] actionsMap, Vector2 pos)
	{
		Vector2 v;
		Vector2 direction;
		int vectorNum;
		if (pos.y >= pos.x * 2 && pos.y > -pos.x) {
			vectorNum = HexVector2.HEX_VECTOR_DIRECTION_UP;
		} else if (pos.y >= pos.x / 2 && pos.y < pos.x * 2) {
			vectorNum = HexVector2.HEX_VECTOR_DIRECTION_UP_RIGHT;
		} else if (pos.y >= -pos.x && pos.y < pos.x / 2) {
			vectorNum = HexVector2.HEX_VECTOR_DIRECTION_DOWN_RIGHT;
		} else if (pos.y <= pos.x * 2 && pos.y < -pos.x) {
			vectorNum = HexVector2.HEX_VECTOR_DIRECTION_DOWN;
		} else if (pos.y <= pos.x / 2 && pos.y > pos.x * 2) {
			vectorNum = HexVector2.HEX_VECTOR_DIRECTION_DOWN_LEFT;
		} else if (pos.y <= -pos.x && pos.y > pos.x / 2) {
			vectorNum = HexVector2.HEX_VECTOR_DIRECTION_UP_LEFT;
		} else { // 0, 0
			vectorNum = HexVector2.HEX_VECTOR_DIRECTION_UP;
		}
		direction = HexVector2.GetBaseVector(vectorNum);
		
		int offset = 1;
		while (Math.Abs(pos.x + direction.x * offset) <= 20 && Math.Abs(pos.y + direction.y * offset) <= 20) {
			v = pos + direction * offset;
			if (actionsMap[20 + (int)v.x, 20 + (int)v.y] == 0) {
				actionsMap[20 + (int)v.x, 20 + (int)v.y] = (vectorNum + 3) % 6; // invert vector direction
			} else if (actionsMap[20 + (int)v.x, 20 + (int)v.y] >= 0) {
				actionsMap[20 + (int)v.x, 20 + (int)v.y] += 6;
			}
			offset++;
		}
	}
	
	private int RingNum(Vector2 pos)
	{
		if (pos.x * pos.y >= 0) {
			return (int)Mathf.Max(Mathf.Abs(pos.x), Mathf.Abs(pos.y));
		} else {
			return (int)(Mathf.Abs(pos.x) + Mathf.Abs(pos.y));
		}
	}
	
}
