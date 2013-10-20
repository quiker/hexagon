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
	
	public static Vector3 ConvertHexVector(Vector2 pos)
	{
		Vector3 hexOX = new Vector3(0.866025f, -0.5f, 0f);
		Vector3 hexOY = new Vector3(0f, 1f, 0f);
		float scale = 36f;
		
		return hexOX * scale * pos.x + hexOY * scale * pos.y;
	}
	
}
