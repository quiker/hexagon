using UnityEngine;
using System.Collections;

public class Pin : MonoBehaviour
{
	public int color;
	public int type;

	public Vector2 position;
	
	private Color[] colors = {Color.white, Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.grey};

	// Use this for initialization
	void Start () {
		transform.GetChild(0).renderer.material.color = colors[color];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SetColor(int _color)
	{
		color = _color;
		transform.GetChild(0).renderer.material.color = colors[color];	
	}
	
	public void UpdatePosition()
	{
		transform.localPosition = HexVector2.ConvertHexVector(position);
	}
}
