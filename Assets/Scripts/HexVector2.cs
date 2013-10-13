using UnityEngine;
using System.Collections;

public class HexVector2 {

	public static Vector2 RotateCW(Vector2 pos)
	{
		Vector2 newPos;
		newPos.y = pos.y - pos.x;
		newPos.x = pos.y;
		
		return newPos;
	}
	
	public static Vector2 RotateCCW(Vector2 pos)
	{
		Vector2 newPos;
		newPos.x = pos.x - pos.y;
		newPos.y = pos.x;
		
		return newPos;
	}
}
