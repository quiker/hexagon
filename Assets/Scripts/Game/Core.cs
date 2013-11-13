using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Core : MonoBehaviour
{
	[HideInInspector]
	public Figure figure;
	private Vector2[][] rings;
	private int[,] pinMap = new int[41,41];
	private float delayEndTime = 0;
	private bool isPinsRemoved = false;
	
	public LevelController levelController = null;
	public AudioClip rotateAudioClip = null;
	public AudioClip breakAudioClip = null;
	public AudioClip connectAudioClip = null;
	
	
	// data for checking
	private Pin[] checkPins;
		
	// Use this for initialization
	void Start () {
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
		// update pinMap
		pinMap = figure.GetPinMap();
		
		bool needDalay = false;
		if (!needDalay) {
			needDalay = CheckRings();
		}
		if (!needDalay) {
			needDalay = CheckLines();
		}
		if (!needDalay) {
			needDalay = CheckGroups();
		}
		if (!needDalay) {
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
				if (pinMap[(int)pos.x+21, (int)pos.y+21] == 0) {
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
		int lineLength = Game.GetInstance().levelController.lineLength;
		
		int[,] actionsMap = new int[41,41];
		
		// init actionsMap
		for (int i = 0; i < 41; i++) {
			for (int j = 0; j < 41; j++) {
				actionsMap[i,j] = 0;
			}
		}
		
		Vector2[] directions = {new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)};
		Vector2 v;
		int offset;
		
		foreach (Pin pin in checkPins) {
			if (actionsMap[21+(int)pin.position.x, 21+(int)pin.position.y] == -1) {
				continue;
			}
			// each pin check on 3 directions
			foreach (Vector2 direction in directions) {
				int color = pin.color;
				offset = 0;
				do {
					offset++;
					v = direction * offset + pin.position;
				} while (pinMap[21+(int)v.x, 21+(int)v.y] == color);
				int positive_offset = offset - 1;
				offset = 0;
				do {
					offset++;
					v = direction * -offset + pin.position;
				} while (pinMap[21+(int)v.x, 21+(int)v.y] == color);
				int negative_offset = offset - 1;
				
				if (positive_offset + negative_offset + 1 >= lineLength) {
					// line found
					for (int i = -negative_offset; i <= positive_offset; i++) {
						v = direction * i + pin.position;
						actionsMap[21+(int)v.x, 21+(int)v.y] = -1;
					}
				}
			}
		}
		
		// remove pins
		LinkedList<Pin> newPins = new LinkedList<Pin>();
		foreach (Pin pin in figure.pins) {
			newPins.AddLast(pin);
		}
		foreach (Pin pin in figure.pins) {
			if (actionsMap[21+(int)pin.position.x, 21+(int)pin.position.y] == -1) {
				pin.DestroyPin();
				newPins.Remove(pin);
				isPinsRemoved = true;
			}
		}
		figure.pins = new Pin[newPins.Count];
		newPins.CopyTo(figure.pins, 0);
		
		LinkedList<Pin> newCheckPins = new LinkedList<Pin>(); /////////////////////
		if (newCheckPins.Count == 0) {
			return false;
		}
		checkPins = new Pin[newCheckPins.Count];
		newCheckPins.CopyTo(checkPins, 0);
		
		return checkPins.Length > 0;
	}
	
	public bool CheckGroups()
	{
		return false;
	}
	
	public bool CheckHexagons()
	{
		return false;	
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
