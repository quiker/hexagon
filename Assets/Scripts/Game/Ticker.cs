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
	private bool stopped = false;
	
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
	
	public void Stop()
	{
		stopped = true;
	}
	
	public void Resume()
	{
		stopped = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!stopped) {
			sumDeltaTime += Time.deltaTime;
			if (sumDeltaTime >= tick) {
				DoTick();
			}
		}
	}	

}
