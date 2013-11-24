using UnityEngine;
using System.Collections;

public class Pin : MonoBehaviour
{
	// constants
	public const int PIN_TYPE_PILL = 1;
	public const int PIN_TYPE_MOB1 = 2;
	public const int PIN_TYPE_MOB2 = 3;
	public const int PIN_TYPE_MOB3 = 4;
	public const int PIN_TYPE_MOB4 = 5;
	public const int PIN_TYPE_MOB5 = 6;
	
	public class MobAction {
		public int id;
		public float inactiveInterval;
		public float activeInterval;
		public float chance;
		public float[] parameters;
	}
	
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
	
	private int[] _actions;
	public int[] actions {
		get {
			return this._actions;
		}
		set {
			_actions = value;
			InitActions();
		}
	}
	
	// interpolation movement
	private bool inMoving = false;
	private float startTime;
	private Vector3 startPos;
	private Vector3 endPos;
	
	void Update ()
	{
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
		Invoke("RealDestroyPin", 0.5f);
	}
	
	public void RealDestroyPin()
	{
		AchievementManager.GetInstance().EventDestroyPin(_type, _color);
		Destroy(gameObject);
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
	
	private void InitActions()
	{
		int i = 0;
		foreach (int action in _actions) {
			if (action != null) {
				StartCoroutine(ActionRoutine(action));
			}
			i++;
		}
	}
	
	private IEnumerator ActionRoutine(int actionNum)
	{
		MobAction action = Game.GetInstance().levelController.actions[actionNum];
		yield return new WaitForSeconds(action.inactiveInterval + Random.Range(0f, 1f) * action.activeInterval);
		if (Random.Range(0, 100) < action.chance) {
			OnAction(actionNum);
		}
	}
	
	private void OnAction(int actionNum)
	{
		MobAction action = Game.GetInstance().levelController.actions[actionNum];
		switch (action.id) {
			case 1: MobAction1(actionNum); break;
			case 2: MobAction2(actionNum); break;
			case 3: MobAction3(actionNum); break;
			case 4: MobAction4(actionNum); break;
			case 5: MobAction5(actionNum); break;
		}
		StartCoroutine(ActionRoutine(actionNum));
	}
	
	public void MobAction1(int actionNum)
	{
		Debug.Log("mob 1 action");
		MobAction action = Game.GetInstance().levelController.actions[actionNum];
		
		Vector2[] checkVectors = {
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0),
			new Vector2(0, -1),
			new Vector2(-1, -1),
			new Vector2(-1, 0)
		};
		ArrayUtils.RandomSort<Vector2>(checkVectors);
		
		Figure f = Game.GetInstance().GetLevelController().core.figure;
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
			int distance = Random.Range((int)action.parameters[0], (int)action.parameters[1]+1);
			Vector2 v;
			for (int i = distance; i > 0; i--) {
				v = dstVector * i;
				if (!f.IsFilled(position + v)) {
					dstVector = v;
					break;
				}
			}
			MoveOn(dstVector);
		}
		
		return;
	}
	
	public void MobAction2(int actionNum)
	{
		Debug.Log("mob 2 action");
		MobAction action = Game.GetInstance().levelController.actions[actionNum];
		int colorsCount = action.parameters.Length;
		if (colorsCount == 0) {
			return;
		} else if (colorsCount == 1) {
			color = (int)action.parameters[0];
		} else if (colorsCount == 2) {
			color = (color == (int)action.parameters[0]) ? (int)action.parameters[1] : (int)action.parameters[0];
		} else {
			int tmp_color;
			do {
				int colorId = Random.Range(0, colorsCount);
				tmp_color = (int)action.parameters[colorId];
			} while (color == tmp_color);
			color = tmp_color;
		}
	}
	
	public void MobAction3(int actionNum)
	{
		Debug.Log("mob 3 action");
		MobAction action = Game.GetInstance().levelController.actions[actionNum];
		
		Vector2[] checkVectors = {
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0),
			new Vector2(0, -1),
			new Vector2(-1, -1),
			new Vector2(-1, 0)
		};
		ArrayUtils.RandomSort<Vector2>(checkVectors);
		
		Figure f = Game.GetInstance().GetLevelController().core.figure;
		Vector2 dstVector = new Vector2();
		bool vectorFound = false;
		Pin pin = null;
		foreach (Vector2 v in checkVectors) {
			pin = f.GetPin(position + v);
			if (pin != null && pin.type == Pin.PIN_TYPE_PILL && ArrayUtils.Contains(action.parameters, pin.color) ) {
				dstVector = v;
				vectorFound = true;
				break;
			}
		}
		if (vectorFound) {
			MoveOn(dstVector);
			f.RemovePin(pin);
			pin.DestroyPin();
		}
		
		return;
	}
	
	public void MobAction4(int actionNum)
	{
		Debug.Log("mob 4 action");
		MobAction action = Game.GetInstance().levelController.actions[actionNum];
		
		Vector2[] checkVectors = {
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0),
			new Vector2(0, -1),
			new Vector2(-1, -1),
			new Vector2(-1, 0)
		};
		ArrayUtils.RandomSort<Vector2>(checkVectors);
		
		Figure f = Game.GetInstance().GetLevelController().core.figure;
		for (int i = 0; i < action.parameters[0]; i++) {
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
				int[] actionIds = ArrayUtils.Slice(action.parameters, 3);
				Pin pin = Game.GetInstance().GetLevelController().pinFactory.GetPin(f, (int)action.parameters[1], (int)action.parameters[2], _position, actionIds);
				f.AddPin(pin);
				pin.MoveOn(dstVector);
			}
		}
	}
	
	public void MobAction5(int actionNum)
	{
		Debug.Log("mob 5 action");
		MobAction action = Game.GetInstance().levelController.actions[actionNum];
		
		float[] actions = action.parameters;
		ArrayUtils.RandomSort<float>(actions);
		
		int randActionNum = (int)actions[0];
		action = Game.GetInstance().levelController.actions[randActionNum];
		switch (action.id) {
			case 1: MobAction1(randActionNum); break;
			case 2: MobAction2(randActionNum); break;
			case 3: MobAction3(randActionNum); break;
			case 4: MobAction4(randActionNum); break;
			case 5: Debug.LogError("MobAction5: recursive action"); break;
		}
		
		return;
	}
}

