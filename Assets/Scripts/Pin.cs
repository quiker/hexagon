using UnityEngine;
using System.Collections;

public class Pin : MonoBehaviour {
	
	public Core core;
	public int color;
	public Vector2 position;
	public Vector2 figurePosition;
	
	private Vector3 newPosition;
	
	private Color[] colors = {Color.white, Color.red, Color.green, Color.blue, Color.yellow};

	// Use this for initialization
	void Start () {
		transform.GetChild(0).renderer.material.color = colors[color];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void UpdatePosition() {
		Vector3 hexOX = new Vector3(0.866025f, -0.5f, 0f);
		Vector3 hexOY = new Vector3(0f, 1f, 0f);
		float y_scale = 36.2f;
		float x_scale = 36.9f;
		
		newPosition = ((position.x + figurePosition.x) * hexOX * x_scale) + ((position.y + figurePosition.y) * hexOY * y_scale) + core.transform.position;
	
		transform.position = newPosition;
	}
}
