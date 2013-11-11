using UnityEngine;
using System.Collections;

public class Pin : MonoBehaviour
{
	// constants
	public const int PIN_TYPE_PILL = 1;
	public const int PIN_TYPE_MOB1 = 2;
	public const int PIN_TYPE_MOB2 = 3;
	public const int PIN_TYPE_MOB3 = 4;
	
	// inspector fields
	public GameObject pillSprite =  null;
	public GameObject splashSprite =  null;
	public GameObject[] mobSprites =  null;
	
	
	private int _color = 0;
	public int color {
		get {
			return this._color;
		}
		set {
			if (_color != value) {
				_color = value;
				UpdateSprite();
			}
		}
	}
	private int _type = 0;
	public int type {
		get {
			return this._type;
		}
		set {
			if (_type != value) {
				_type = value;
				UpdateSprite();
			}
		}
	}
	private Vector2 _position;
	public Vector2 position {
		get {
			return this._position;
		}
		set {
			if (_position != value) {
				_position = value;
				UpdatePosition();
			}
		}
	}

	private float destroyTime = 0;
	// interpolation movement
	private bool inMoving = false;
	private float startTime;
	private Vector3 startPos;
	private Vector3 endPos;
	
//	void Start ()
//	{
//		SetType(type, false);
//		SetColor(color);
//	}

	void Update ()
	{
		// destroing
		if (Time.timeSinceLevelLoad >= destroyTime && destroyTime != 0) {
			Destroy(gameObject);
		}
		// moving
		if (inMoving) {
			float tLerp = Time.timeSinceLevelLoad - startTime;
			float seconds = 0.3f;
			if (tLerp < seconds) {
				transform.localPosition = Vector3.Lerp(startPos, endPos, tLerp/seconds);
			} else {
				inMoving = false;
				UpdatePosition();
			}
		}
	}
	
	private void UpdateSprite() 
	{
		if (_type == 0 || _color == 0) return;
		if (_type == PIN_TYPE_PILL) {
			foreach (GameObject mobSprite in mobSprites) {
				mobSprite.SetActive(false);
			}
			pillSprite.SetActive(true);
			Vector2 offset = new Vector2 ((_color-1) * 0.1f, 1.0f);
			pillSprite.renderer.material.SetTextureOffset ("_MainTex", offset);
		} else {
			mobSprites[(_type-2)*5+_color-1].SetActive(true);
			pillSprite.SetActive(false);
		}
	}
	
	public void DestroyPin()
	{
		foreach (GameObject mobSprite in mobSprites) {
			mobSprite.SetActive(false);
		}
		pillSprite.SetActive(false);
		splashSprite.SetActive(true);
		destroyTime = Time.timeSinceLevelLoad + 0.5f;
	}
	
	public void MoveOn(Vector2 vector)
	{
		startTime = Time.timeSinceLevelLoad;
		startPos = transform.localPosition;
		_position += vector;
		endPos = HexVector2.ConvertHexVector(_position);
		inMoving = true;
	}
	
	public void UpdatePosition()
	{
		transform.localPosition = HexVector2.ConvertHexVector(_position);
	}
}
