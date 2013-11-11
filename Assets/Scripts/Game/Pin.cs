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
	
	private int[] _action;
	public int[] action {
		get {
			return this._action;
		}
		set {
			_action = value;
			InitNewAction();
		}
	}
	
	// destroy delay
	private float destroyTime = 0;
	// action delay
	private float actionTime = 0;
	// interpolation movement
	private bool inMoving = false;
	private float startTime;
	private Vector3 startPos;
	private Vector3 endPos;
	
	void Update ()
	{
		// destroing
		if (destroyTime != 0 && Time.timeSinceLevelLoad >= destroyTime) {
			Destroy(gameObject);
		}
		// action
		if (actionTime != 0 && Time.timeSinceLevelLoad >= actionTime) {
			OnAction();
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
			foreach (GameObject mobSprite in mobSprites) {
				mobSprite.SetActive(false);
			}
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
	
	private void InitNewAction()
	{
		if (action.Length > 0) {
			actionTime = Time.timeSinceLevelLoad + action[0] + Random.Range(0f, 1f) * action[1];
		}
	}
	
	private void OnAction()
	{
		if (Random.Range(0, 100) < action[2]) {
			switch (_type) {
				case PIN_TYPE_MOB1: Mob1Action(); break;
				case PIN_TYPE_MOB2: Mob2Action(); break;
				case PIN_TYPE_MOB3: Mob3Action(); break;	
			}
		}
		InitNewAction();
	}
	
	public void Mob1Action()
	{
		return;
	}
	
	public void Mob2Action()
	{
		Debug.Log("mob 2 action");
		int colorsCount = action.Length - 3;
		if (colorsCount == 0) {
			return;
		} else if (colorsCount == 1) {
			color = action[3];
		} else if (colorsCount == 2) {
			color = (color == action[3]) ? action[4] : action[3];
		} else {
			int tmp_color;
			do {
				int colorId = Random.Range(0, colorsCount);
				tmp_color = action[3 + colorId];
			} while (color == tmp_color);
			color = tmp_color;
		}
	}
	
	public void Mob3Action()
	{
		Vector2[] checkVectors = {
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0),
			new Vector2(0, -1),
			new Vector2(-1, -1),
			new Vector2(-1, 0)
		};
		
		Debug.Log("mob 3 action");
		Figure f = Game.GetInstance().GetLevelController().core.figure;
		for (int i = 0; i < action[3]; i++) {
			Vector2 dstVector = new Vector2();
			bool vectorFound = false;
			foreach (Vector2 v in checkVectors) {
				if (!f.IsFilled(position + v)) {
					dstVector = v;
					vectorFound = true;
					break;
				}
			}
			if (vectorFound) {
				Pin pin = Game.GetInstance().GetLevelController().pinFactory.GetPin(f, action[4], action[5], _position, action[6]);
				f.AddPin(pin);
				pin.MoveOn(dstVector);
			}
		}
	}
}
