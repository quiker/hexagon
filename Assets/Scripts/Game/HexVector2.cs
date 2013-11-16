using UnityEngine;
using System.Collections;

public class HexVector2
{
	public const int HEX_VECTOR_DIRECTION_UP         = 0;
	public const int HEX_VECTOR_DIRECTION_UP_RIGHT   = 1;
	public const int HEX_VECTOR_DIRECTION_DOWN_RIGHT = 2;
	public const int HEX_VECTOR_DIRECTION_DOWN       = 3;
	public const int HEX_VECTOR_DIRECTION_DOWN_LEFT  = 4;
	public const int HEX_VECTOR_DIRECTION_UP_LEFT    = 5;
	
	public static Vector2[] baseVectors = new Vector2[6] {
		new Vector2(0, 1),
		new Vector2(1, 1),
		new Vector2(1, 0),
		new Vector2(0, -1),
		new Vector2(-1, -1),
		new Vector2(-1, 0),
	};
	
	public static Vector2 GetBaseVector(int direction)
	{
		return HexVector2.baseVectors[direction];
	}
	
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
