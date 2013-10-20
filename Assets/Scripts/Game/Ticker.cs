using UnityEngine;
using System.Collections;

public class Ticker : MonoBehaviour {
	
	public interface TickListener
	{
		void OnTick();
	}
	
	public float startTick = 1f;
	public GameObject[] listeners;
	
	private float tick;
	private float sumDeltaTime = 0;
	
	// Use this for initialization
	void Start () {
		tick = startTick;
	}
	
	void DoTick() {
		foreach (GameObject listener in listeners ) {
			(listener.GetComponent(typeof(TickListener)) as TickListener).OnTick();
		}
		sumDeltaTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		sumDeltaTime += Time.deltaTime;
		if (sumDeltaTime >= tick) {
			DoTick();
		}
			
	}	

}
