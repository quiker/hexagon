using UnityEngine;
using System.Collections;

public class Core : MonoBehaviour {
	
	private Figure figure;
	
	public Figure currentFigure;
	
	// Use this for initialization
	void Start () {
		figure = GetComponent("Figure") as Figure;
		figure.Init(0, 0);
	}
	
	public void Reinit()
	{
		figure.Reinit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Connect(Figure _figure) {
		_figure.ConnectTo(figure);
	}
	
	public void RotateCW()
	{
		if (!figure.isCollisionRotateCW(currentFigure)) {
			figure.RotateCW();
		}
	}
	
	public void RotateCCW()
	{
		if (!figure.isCollisionRotateCCW(currentFigure)) {
			figure.RotateCCW();
		}
	}
}
