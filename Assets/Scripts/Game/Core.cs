using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Core : MonoBehaviour {
	
	private Figure figure;
	private int score = 0;
	private Vector2[][] rings;
	private int[,] pinMap = new int[41,41];
	
	public Figure currentFigure;
	//public UILabel scoreLabel = null;
	public LevelFactory levelFactory;
	public AudioClip rotateAudioClip = null;
	public AudioClip breakAudioClip = null;
	public AudioClip connectAudioClip = null;
		
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
	
	public void Reinit()
	{
		figure = GetComponent("Figure") as Figure;
		figure.Init(0, 0);
		figure.Reinit();
		figure.pins = levelFactory.GetLevel(1, this);
		figure.UpdatePosition();
	}

	public void RotateCW()
	{
		if (!figure.isCollisionRotateCW(currentFigure)) {
			figure.RotateCW();
			
			if (rotateAudioClip != null) {
				audio.PlayOneShot(rotateAudioClip);
			}
		}
	}
	
	public void RotateCCW()
	{
		if (!figure.isCollisionRotateCCW(currentFigure)) {
			figure.RotateCCW();
			
			if (rotateAudioClip != null) {
				audio.PlayOneShot(rotateAudioClip);
			}
		}
	}
	
	public void Connect(Figure _figure)
	{
		_figure.ConnectTo(figure);
		
		if (connectAudioClip != null) {
			audio.PlayOneShot(connectAudioClip);
		}
		
		CheckRings(_figure);
	}
	
	public void CheckRings(Figure _figure)
	{
		// reset pinMap
		for (int i = 0; i < 41; i++) {
			for (int j = 0; j < 41; j++) {
				pinMap[i,j] = -1;
			}
		}
		// update pinMap
		foreach (Pin pin in figure.pins) {
			pinMap[(int)pin.position.x+21, (int)pin.position.y+21] = pin.color;
		}
		
		// figure pins rings
		HashSet<int> ringNumsSet = new HashSet<int>();
		foreach (Pin pin in _figure.pins) {
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
			foreach (Vector2 pos in rings[ringNum-1]) {
				if (pinMap[(int)pos.x+21, (int)pos.y+21] == -1) {
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
			Destroy(pin.gameObject);
		}
		
		// move up rings
		foreach(int ring in foundRingNumsSet) {
			for (int pn = 0; pn < figure.pins.Length; pn++) {
				Pin pin = figure.pins[pn];
				// up
				if (pin.position.x >= -ring &&
				    pin.position.y > pin.position.x + ring) {
					figure.pins[pn].position = pin.position + new Vector2(1, 0);
				}
				// right up
				else if (pin.position.y <= pin.position.x + ring &&
				    pin.position.y > ring) {
					figure.pins[pn].position = pin.position + new Vector2(0, -1);
				}
				// right down
				else if (pin.position.x > ring &&
				    pin.position.y <= ring) {
					figure.pins[pn].position = pin.position + new Vector2(-1, -1);
				}
				// down
				else if (pin.position.x <= ring &&
				    pin.position.y < pin.position.x - ring) {
					figure.pins[pn].position = pin.position + new Vector2(-1, 0);
				}
				// left down
				else if (pin.position.y < -ring &&
				    pin.position.y >= pin.position.x - ring) {
					figure.pins[pn].position = pin.position + new Vector2(0, 1);
				}
				// left up
				else if (pin.position.y >= -ring &&
				    pin.position.x < -ring) {
					figure.pins[pn].position = pin.position + new Vector2(1, 1);
				}
			}
		}

		figure.UpdatePosition();
		
		if (removePins.Count > 0) {
			GameController.addToScore(removePins.Count);
			
			if (breakAudioClip != null) {
				audio.PlayOneShot(breakAudioClip);
			}
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
	
	void onGameRestarted() {
		Reinit();
	}
	
}
